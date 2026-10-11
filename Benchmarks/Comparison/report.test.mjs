import assert from 'node:assert/strict';
import test from 'node:test';
import { formatDuration, renderDetails, renderHighlight, renderSummary, replaceResults, summarize } from './report.mjs';

test('summarize reports mean, sample standard deviation, and median', () => {
  assert.deepEqual(summarize([1, 2, 3]), { meanMs: 2, standardDeviationMs: 1, medianMs: 2 });
  assert.equal(summarize([4, 1, 3, 2]).medianMs, 2.5);
});

test('summarize rejects missing, invalid, and insufficient samples', () => {
  for (const samples of [null, [], [1], [0, 1], [-1, 1], [NaN, 1], [Infinity, 1]]) {
    assert.throws(() => summarize(samples));
  }
});

test('durations use readable units and three significant digits', () => {
  assert.equal(formatDuration(0.032018816), '32 µs');
  assert.equal(formatDuration(0.327010829), '327 µs');
  assert.equal(formatDuration(170.258873), '170 ms');
  assert.equal(formatDuration(15165.89718), '15.2 s');
  assert.equal(formatDuration(1), '1 ms');
  assert.equal(formatDuration(1000), '1 s');
});

test('compact durations keep sub-millisecond means in milliseconds', () => {
  assert.equal(formatDuration(0.032018816, undefined, false), '0.032 ms');
  assert.equal(formatDuration(0.327010829, undefined, false), '0.327 ms');
  assert.equal(formatDuration(15165.89718, undefined, false), '15.2 s');
});

test('variability shares the mean unit without rounding small deviations to zero', () => {
  assert.equal(formatDuration(0.032018816, 0.004814696), '32 ± 4.81 µs');
  assert.equal(formatDuration(170.258873, 51.195118), '170 ± 51.2 ms');
  assert.equal(formatDuration(15165.89718, 826.945178), '15.2 ± 0.827 s');
  assert.equal(formatDuration(1, 0.000001), '1 ± 0.000001 ms');
  assert.equal(formatDuration(1, 0), '1 ± 0 ms');
});

test('README replacement preserves content outside the markers and is repeatable', () => {
  const readme = 'Before\n<!-- gbx-comparison:start -->\nOld\n<!-- gbx-comparison:end -->\nAfter';
  const expected = 'Before\n<!-- gbx-comparison:start -->\nNew\n<!-- gbx-comparison:end -->\nAfter';
  assert.equal(replaceResults(readme, 'New'), expected);
  assert.equal(replaceResults(expected, 'New'), expected);
  assert.throws(() => replaceResults('No markers', 'New'));
  assert.throws(() => replaceResults('<!-- gbx-comparison:end --><!-- gbx-comparison:start -->', 'New'));
});

test('reports label partial parses and failures without inventing timings', () => {
  const report = {
    measuredAt: '2026-10-10T12:00:00.000Z',
    environment: { cpu: 'Test CPU', os: 'Test OS' },
    source: { revision: 'local', libraryDirty: false },
    dependencies: { gbxPyRevision: 'pinned' },
    settings: { samples: 2, warmupSeconds: 2, sampleMs: 200 },
    libraries: ['GBX.NET', 'gbx-py', 'gbx-ts'],
    maps: [{ name: 'Test|Map.Gbx', bytes: 100, sha256: 'hash' }],
    results: [
      { library: 'GBX.NET', map: 'Test|Map.Gbx', status: 'ok', statistics: summarize([1, 1]), validation: { blockCount: 1, problems: [] } },
      { library: 'gbx-py', map: 'Test|Map.Gbx', status: 'partial', statistics: summarize([2, 2]), validation: { blockCount: null, problems: ['Unknown chunk'] } },
      { library: 'gbx-ts', map: 'Test|Map.Gbx', status: 'error', error: 'Parser failed' },
    ],
  };
  const summary = renderSummary(report);
  assert.match(summary, /Test\\\|Map.Gbx/);
  assert.match(summary, /1 ± 0 ms \| 2 ± 0 ms \(partial\) \| Failed/);
  assert.match(summary, /not equivalent full parses/);
  const highlight = renderHighlight(report);
  assert.match(highlight, /^### Benchmark/);
  assert.match(highlight, /\*\*1 ms\*\* for \*\*Test\\\|Map\*\* on average with GBX.NET/);
  assert.match(highlight, /\*\*1 ms\*\* \| 2 ms \(partial\) \| Failed/);
  assert.doesNotMatch(highlight, /±/);
  assert.match(highlight, /Values are mean times per parse/);
  assert.match(highlight, /Benchmarks\/README.md#measured-results\) include sample standard deviations/);
  assert.match(highlight, /including LZO decompression/);
  assert.match(highlight, /Startup and file I\/O are not timed/);
  assert.match(highlight, /not a like-for-like speedup/);
  assert.match(highlight, /does not prove exhaustive coverage/);
  assert.match(highlight, /2026-10-10/);
  assert.match(highlight, /Benchmarks\/README.md#cross-library-map-comparison/);
  const partialBaseline = renderHighlight({ ...report, results: report.results.map(result => result.library === 'GBX.NET' ? { ...result, status: 'partial' } : result) });
  assert.doesNotMatch(partialBaseline, /on average with GBX.NET/);
  assert.match(partialBaseline, /\*\*1 ms\*\* \(partial\)/);
  const details = renderDetails(report);
  assert.match(details, /Unknown chunk/);
  assert.match(details, /Parser failed/);
  assert.match(details, /hash/);
});

test('reports include every library column and ManiaPlanetSharp source details', () => {
  const report = {
    measuredAt: '2026-10-11T12:00:00.000Z',
    environment: { cpu: 'Test CPU', os: 'Test OS' },
    source: { revision: 'local', libraryDirty: true },
    dependencies: { gbxPyRevision: 'python-pinned', maniaPlanetSharpRevision: 'dfac322673d7a5260381d107a7ee34191da2a5fd' },
    settings: { samples: 2, warmupSeconds: 2, sampleMs: 200 },
    libraries: ['GBX.NET', 'gbx-py', 'gbx-ts', 'ManiaPlanetSharp'],
    maps: [{ name: 'Test.Map.Gbx', bytes: 100, sha256: 'hash' }],
    results: ['GBX.NET', 'gbx-py', 'gbx-ts', 'ManiaPlanetSharp'].map((library, index) => ({
      library, map: 'Test.Map.Gbx', status: index ? 'partial' : 'ok',
      statistics: summarize([index + 1, index + 1]),
      runtime: '.NET 10', parserMode: 'runtime-generated', lzoImplementation: 'upstream MiniLZO',
      validation: { blockCount: 1, problems: index ? ['Incomplete coverage'] : [] },
    })),
  };
  const summary = renderSummary(report);
  assert.match(summary, /\| Map \| File bytes \| GBX.NET \| gbx-py \| gbx-ts \| ManiaPlanetSharp \|/);
  assert.match(summary, /\| --- \| ---: \| ---: \| ---: \| ---: \| ---: \|/);
  assert.match(summary, /4 ± 0 ms \(partial\) \|/);
  assert.match(summary, /ManiaPlanetSharp source dfac322673d7, runtime-generated, upstream MiniLZO/);
  assert.match(summary, /ManiaPlanetSharp source: `dfac322673d7a5260381d107a7ee34191da2a5fd`/);
  const highlight = renderHighlight(report);
  assert.match(highlight, /\| Map \| GBX.NET \| gbx-py \| gbx-ts \| ManiaPlanetSharp \|/);
  assert.match(highlight, /\| --- \| ---: \| ---: \| ---: \| ---: \|/);
  assert.match(highlight, /4 ms \(partial\) \|/);
});
