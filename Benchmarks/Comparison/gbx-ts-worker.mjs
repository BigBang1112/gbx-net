import { readFileSync, writeFileSync } from 'node:fs';
import { performance } from 'node:perf_hooks';
import { GBX, CGameCtnChallenge } from 'gbx';

const [mapPath, outputPath, sampleCount, warmupSeconds, sampleMs] = process.argv.slice(2);
const messages = new Set();
console.log = console.warn = console.error = (...values) => messages.add(values.map(String).join(' '));

function inspectChunks(value, problems, visited = new Set()) {
  if (value === null || typeof value !== 'object' || visited.has(value) || ArrayBuffer.isView(value)) return;
  visited.add(value);
  if (value.chunks) {
    for (const [chunkId, chunk] of Object.entries(value.chunks)) {
      if (chunk === null) problems.push(`Skipped chunk 0x${Number(chunkId).toString(16)}`);
    }
  }
  for (const child of Object.values(value)) inspectChunks(child, problems, visited);
}

try {
  const bytes = readFileSync(mapPath);
  let lastResult = await new GBX(bytes, 0).parse();
  if (!(lastResult instanceof CGameCtnChallenge) || !Array.isArray(lastResult.blocks)) {
    throw new Error('Parsing did not produce a map body with blocks.');
  }
  const problems = [...messages];
  inspectChunks(lastResult, problems);
  const validation = {
    blockCount: lastResult.blocks.filter(block => block.flags !== -1 && block.flags !== 0xffffffff).length,
    problems: [...new Set(problems)],
  };
  messages.clear();
  console.log = console.warn = console.error = () => {};
  const warmupStart = performance.now();
  let warmupOperations = 0;
  do {
    lastResult = await new GBX(bytes, 0).parse();
    warmupOperations++;
  } while (performance.now() - warmupStart < Number(warmupSeconds) * 1000);
  const operations = Math.min(10000, Math.max(1, Math.ceil(Number(sampleMs) * warmupOperations / (performance.now() - warmupStart))));
  const samples = [];
  for (let sampleIndex = 0; sampleIndex < Number(sampleCount); sampleIndex++) {
    const start = performance.now();
    for (let operationIndex = 0; operationIndex < operations; operationIndex++) {
      lastResult = await new GBX(bytes, 0).parse();
    }
    samples.push((performance.now() - start) / operations);
  }
  const packageInfo = JSON.parse(readFileSync(new URL('./node_modules/gbx/package.json', import.meta.url)));
  const lzoInfo = JSON.parse(readFileSync(new URL('./node_modules/lzo-ts/package.json', import.meta.url)));
  writeFileSync(outputPath, JSON.stringify({
    status: problems.length ? 'partial' : 'ok',
    version: packageInfo.version,
    lzoVersion: lzoInfo.version,
    runtime: `Node.js ${process.version}`,
    architecture: process.arch,
    validation,
    warmupOperations,
    operationsPerSample: operations,
    samplesMs: samples,
  }, null, 2));
} catch (error) {
  writeFileSync(outputPath, JSON.stringify({ status: 'error', error: error.stack, diagnostics: [...messages] }, null, 2));
  process.exitCode = 1;
}
