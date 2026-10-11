import { spawnSync } from 'node:child_process';
import { createHash } from 'node:crypto';
import { existsSync, mkdirSync, readFileSync, writeFileSync } from 'node:fs';
import os from 'node:os';
import path from 'node:path';
import { fileURLToPath } from 'node:url';
import { renderDetails, renderHighlight, renderSummary, replaceResults, summarize } from './report.mjs';

const directory = path.dirname(fileURLToPath(import.meta.url));
const root = path.resolve(directory, '../..');
const settings = { samples: 15, warmupSeconds: 2, sampleMs: 200, timeoutSeconds: 600 };
let python = path.join(directory, process.platform === 'win32' ? '.venv/Scripts/python.exe' : '.venv/bin/python');
let output = path.join(directory, 'results', new Date().toISOString().replaceAll(':', '-'));
let updateReadme = false;
let build = true;
const maps = [];
const args = process.argv.slice(2);
for (let index = 0; index < args.length; index++) {
  const option = args[index];
  if (option === '--help') {
    console.log('node Benchmarks/Comparison/run.mjs [--python PATH] [--map PATH ...] [--samples N] [--warmup-seconds N] [--sample-ms N] [--timeout-seconds N] [--output DIR] [--no-build] [--update-readme]');
    process.exit(0);
  }
  if (option === '--update-readme') { updateReadme = true; continue; }
  if (option === '--no-build') { build = false; continue; }
  const value = args[++index];
  if (!value || value.startsWith('--')) throw new Error(`Missing value for ${option}`);
  switch (option) {
    case '--python': python = value; break;
    case '--map': maps.push(path.resolve(value)); break;
    case '--output': output = path.resolve(value); break;
    case '--samples': settings.samples = Number(value); break;
    case '--warmup-seconds': settings.warmupSeconds = Number(value); break;
    case '--sample-ms': settings.sampleMs = Number(value); break;
    case '--timeout-seconds': settings.timeoutSeconds = Number(value); break;
    default: throw new Error(`Unknown option ${option}`);
  }
}
if (!Number.isInteger(settings.samples) || settings.samples < 2 ||
    Object.values(settings).some(value => !Number.isFinite(value) || value <= 0)) throw new Error('Sample count must be an integer >= 2, and all durations must be positive.');
if (!maps.length) maps.push(...['A01-Race.Challenge.Gbx', 'Canyon A01.Map.Gbx', 'Deep_Dip_2r1.Map.Gbx'].map(name => path.join(root, 'Benchmarks', name)));
if (new Set(maps.map(map => path.basename(map))).size !== maps.length) throw new Error('Map basenames must be unique.');

function command(executable, argumentsList, options = {}) {
  return spawnSync(executable, argumentsList, { cwd: root, encoding: 'utf8', timeout: settings.timeoutSeconds * 1000, maxBuffer: 4 * 1024 * 1024, ...options });
}

function git(argumentsList) {
  const result = command('git', argumentsList);
  if (result.error || result.status !== 0) throw new Error(result.error?.message ?? result.stderr);
  return result.stdout.trim();
}

const dependencies = JSON.parse(readFileSync(path.join(directory, 'dependencies.json')));
const pythonSource = path.join(directory, '.deps/gbx-py');
const maniaPlanetSharpSource = path.join(directory, '.deps/ManiaPlanetSharp');
if (!existsSync(python) || !existsSync(path.join(directory, 'node_modules/gbx'))) throw new Error('Run Benchmarks/Comparison/setup.ps1 first, or specify --python.');
if (git(['-C', pythonSource, 'rev-parse', 'HEAD']) !== dependencies.gbxPyRevision || git(['-C', pythonSource, 'status', '--porcelain', '--untracked-files=no'])) throw new Error('gbx-py must be an unmodified checkout of the pinned revision.');
if (!existsSync(maniaPlanetSharpSource)) throw new Error('Run Benchmarks/Comparison/setup.ps1 to download ManiaPlanetSharp.');
if (git(['-C', maniaPlanetSharpSource, 'rev-parse', 'HEAD']) !== dependencies.maniaPlanetSharpRevision || git(['-C', maniaPlanetSharpSource, 'status', '--porcelain', '--untracked-files=no'])) throw new Error('ManiaPlanetSharp must be an unmodified checkout of the pinned revision.');
const fixtureInfo = maps.map(map => {
  const bytes = readFileSync(map);
  return { name: path.basename(map), bytes: bytes.length, sha256: createHash('sha256').update(bytes).digest('hex') };
});
const project = path.join(directory, 'GBX.NET.Comparison');
const maniaPlanetSharpProject = path.join(directory, 'ManiaPlanetSharp.Comparison');
if (build) {
  for (const workerProject of [project, maniaPlanetSharpProject]) {
    const result = command('dotnet', ['build', workerProject, '-c', 'Release', '--disable-build-servers', '--verbosity', 'minimal']);
    if (result.error || result.status !== 0) throw new Error(`Release build failed:\n${result.stdout}\n${result.stderr}\n${result.error ?? ''}`);
  }
}
const dll = path.join(project, 'bin/Release/net10.0/GBX.NET.Comparison.dll');
const maniaPlanetSharpDll = path.join(maniaPlanetSharpProject, 'bin/Release/net10.0/ManiaPlanetSharp.Comparison.dll');
if (!existsSync(dll) || !existsSync(maniaPlanetSharpDll)) throw new Error('Release worker is missing. Run without --no-build.');
mkdirSync(output, { recursive: true });
if (existsSync(path.join(output, 'report.json'))) throw new Error('Output already has a report. Choose a new directory.');
const report = {
  measuredAt: new Date().toISOString(),
  environment: { cpu: os.cpus()[0]?.model ?? 'unknown', logicalProcessors: os.cpus().length, os: `${os.type()} ${os.release()} ${os.arch()}` },
  source: { revision: git(['rev-parse', 'HEAD']), libraryDirty: Boolean(git(['status', '--porcelain', '--', 'Src', 'Generators', 'Resources'])), noBuild: !build },
  dependencies, settings, maps: fixtureInfo, libraries: ['GBX.NET', 'gbx-py', 'gbx-ts', 'ManiaPlanetSharp'], results: [],
};
const workers = [
  { library: 'GBX.NET', executable: 'dotnet', prefix: [dll], suffix: [] },
  { library: 'gbx-py', executable: python, prefix: [path.join(directory, 'gbx-py-worker.py')], suffix: [pythonSource] },
  { library: 'gbx-ts', executable: process.execPath, prefix: [path.join(directory, 'gbx-ts-worker.mjs')], suffix: [] },
  { library: 'ManiaPlanetSharp', executable: 'dotnet', prefix: [maniaPlanetSharpDll], suffix: [] },
];
for (const [mapIndex, map] of maps.entries()) {
  for (const [workerIndex, worker] of workers.entries()) {
    console.log(`Parsing ${path.basename(map)} with ${worker.library}...`);
    const resultPath = path.join(output, `${mapIndex}-${workerIndex}.json`);
    const result = command(worker.executable, [...worker.prefix, map, resultPath, String(settings.samples), String(settings.warmupSeconds), String(settings.sampleMs), ...worker.suffix]);
    let data;
    try {
      if (result.error) throw result.error;
      data = JSON.parse(readFileSync(resultPath, 'utf8'));
      if (!['ok', 'partial', 'error'].includes(data.status)) throw new Error('Invalid worker status.');
      if (data.status !== 'error') {
        if (result.status !== 0 || data.samplesMs.length !== settings.samples || !Array.isArray(data.validation?.problems)) throw new Error('Invalid worker result.');
        const reference = report.results.find(entry => entry.map === path.basename(map) && entry.library === 'GBX.NET');
        if (reference?.validation && data.validation.blockCount !== reference.validation.blockCount) {
          data.status = 'partial';
          data.validation.problems.push(`Block count differs from GBX.NET (${reference.validation.blockCount}).`);
        }
        data.statistics = summarize(data.samplesMs);
      }
    } catch (error) {
      data = { status: 'error', error: `${error.message}\n${result.stderr ?? ''}` };
    }
    report.results.push({ map: path.basename(map), library: worker.library, ...data });
    writeFileSync(path.join(output, 'report.json'), JSON.stringify(report, null, 2) + '\n');
  }
}
const summary = renderSummary(report);
writeFileSync(path.join(output, 'report.md'), renderDetails(report));
console.log(`\n${summary}\nReports: ${output}`);
if (updateReadme) {
  const readmes = [[path.join(root, 'README.md'), renderHighlight(report)], [path.join(root, 'Benchmarks/README.md'), summary]];
  const changes = readmes.map(([readme, content]) => [readme, replaceResults(readFileSync(readme, 'utf8'), content)]);
  for (const [readme, content] of changes) writeFileSync(readme, content);
}
process.exitCode = report.results.some(result => result.status === 'error') ? 1 : 0;
