export function summarize(samples) {
  if (!Array.isArray(samples) || samples.length < 2 || samples.some(value => !Number.isFinite(value) || value <= 0)) {
    throw new Error('Expected at least two positive, finite timing samples.');
  }
  const mean = samples.reduce((total, value) => total + value, 0) / samples.length;
  const standardDeviation = Math.sqrt(samples.reduce((total, value) => total + (value - mean) ** 2, 0) / (samples.length - 1));
  const sorted = [...samples].sort((left, right) => left - right);
  const middle = Math.floor(sorted.length / 2);
  return { meanMs: mean, standardDeviationMs: standardDeviation,
    medianMs: sorted.length % 2 ? sorted[middle] : (sorted[middle - 1] + sorted[middle]) / 2 };
}

function escapeCell(value) {
  return String(value).replaceAll('|', '\\|').replaceAll('\n', ' ');
}

export function formatDuration(milliseconds, standardDeviationMs, useMicroseconds = true) {
  const [scale, unit] = useMicroseconds && milliseconds < 1 ? [1000, 'µs'] : milliseconds < 1000 ? [1, 'ms'] : [0.001, 's'];
  const format = value => (value * scale).toLocaleString('en-US', { maximumSignificantDigits: 3 });
  const deviation = standardDeviationMs === undefined ? '' : ` ± ${format(standardDeviationMs)}`;
  return `${format(milliseconds)}${deviation} ${unit}`;
}

function renderTiming(result, emphasize = false, detailed = true) {
  if (result.status === 'error') return 'Failed';
  const timing = formatDuration(result.statistics.meanMs, detailed ? result.statistics.standardDeviationMs : undefined, detailed);
  return `${emphasize ? `**${timing}**` : timing}${result.status === 'partial' ? ' (partial)' : ''}`;
}

function mapDisplayName(name) {
  if (name === 'Deep_Dip_2r1.Map.Gbx') return 'Deep Dip 2';
  return name.replace(/\.(?:(?:Map|Challenge)\.)?Gbx$/i, '');
}

export function renderHighlight(report) {
  const checked = report.maps.map(map => report.results.find(result => result.map === map.name && result.library === 'GBX.NET' && result.status === 'ok')).filter(Boolean);
  const featured = checked.length > 1 ? [checked[0], checked.at(-1)] : checked;
  const lines = ['### Benchmark', ''];
  if (featured.length) {
    const timings = featured.map(result => `**${formatDuration(result.statistics.meanMs, undefined, false)}** for **${escapeCell(mapDisplayName(result.map))}**`);
    lines.push(`${timings.join(', ')} on average with GBX.NET.`, '');
  }
  lines.push('Repeated in-memory parsing of the original compressed files, including LZO decompression. Values are mean times per parse. Startup and file I/O are not timed. The [detailed results](Benchmarks/README.md#measured-results) include sample standard deviations.', '',
    '| Map | GBX.NET | gbx-py | gbx-ts |',
    '| --- | ---: | ---: | ---: |');
  for (const map of report.maps) {
    const cells = report.libraries.map(library => renderTiming(report.results.find(result => result.map === map.name && result.library === library), library === 'GBX.NET', false));
    lines.push(`| ${escapeCell(mapDisplayName(map.name))} | ${cells.join(' | ')} |`);
  }
  lines.push('', '**Parsing coverage differs.** `(partial)` means skipped chunks or an unfinished body, not equivalent full parsing. Passing the runner\'s checks does not prove exhaustive coverage, so these timings are not a like-for-like speedup.', '',
    `Measured ${report.measuredAt.slice(0, 10)} on ${escapeCell(report.environment.cpu.trim())} (${escapeCell(report.environment.os)}), with ${report.settings.samples} samples per case after warmup. [Methodology, parsing limitations, versions, and reusable runner](Benchmarks/README.md#cross-library-map-comparison).`, '');
  return lines.join('\n');
}

export function renderSummary(report) {
  const lines = [
    `Measured ${report.measuredAt.slice(0, 10)} on ${escapeCell(report.environment.cpu.trim())} (${escapeCell(report.environment.os)}).`,
    '',
    'In-memory parse calls on the original compressed files, including LZO decompression. Mean ± sample standard deviation per parse, using µs, ms, or s as appropriate. Values are rounded to three significant digits. Lower is faster, but parsing coverage differs.',
    '',
    '| Map | File bytes | GBX.NET | gbx-py | gbx-ts |',
    '| --- | ---: | ---: | ---: | ---: |',
  ];
  for (const map of report.maps) {
    const cells = report.libraries.map(library => renderTiming(report.results.find(entry => entry.map === map.name && entry.library === library)));
    lines.push(`| ${escapeCell(map.name)} | ${map.bytes.toLocaleString('en-US')} | ${cells.join(' | ')} |`);
  }
  lines.push('', '**Partial results are not equivalent full parses.** They include skipped chunks or a body that was not fully parsed. Failed calls have no timing. These numbers do not establish a like-for-like speedup.', '');
  const versions = report.libraries.map(library => {
    const result = report.results.find(entry => entry.library === library && entry.runtime);
    const version = library === 'gbx-py' ? `parser ${report.dependencies.gbxPyRevision.slice(0, 12)}, LZO package ${result?.version ?? 'unavailable'}` : result?.version ?? 'unavailable';
    return `${library} ${version} (${result?.runtime ?? 'unavailable'})`;
  });
  lines.push(`${versions.join(', ')}. GBX.NET.LZO ${report.results.find(entry => entry.library === 'GBX.NET')?.lzoVersion ?? 'unavailable'}, lzo-ts ${report.results.find(entry => entry.library === 'gbx-ts')?.lzoVersion ?? 'unavailable'}, Construct ${report.results.find(entry => entry.library === 'gbx-py')?.constructVersion ?? 'unavailable'}.`, '',
    `GBX.NET source: \`${report.source.revision}\`${report.source.libraryDirty ? ' with local library changes' : ' (library sources unchanged)'}. gbx-py parser source: \`${report.dependencies.gbxPyRevision}\`.`, '',
    `${report.settings.samples} samples per case after at least ${report.settings.warmupSeconds} s of warmup, with an adaptive batch targeting ${report.settings.sampleMs} ms (at least one parse per sample). Worker processes run sequentially. Startup, imports, file reads, validation, and report writing are excluded. Normal garbage collection remains enabled.`, '');
  return lines.join('\n');
}

export function renderDetails(report) {
  const lines = ['# Gbx library comparison', '', renderSummary(report), '## Parsing checks', '',
    '| Map | Library | Status | Blocks excluding placeholders | Diagnostics |',
    '| --- | --- | --- | ---: | --- |'];
  for (const result of report.results) {
    const problems = result.error ? [result.error] : result.validation.problems;
    lines.push(`| ${escapeCell(result.map)} | ${result.library} | ${result.status} | ${result.validation?.blockCount ?? '-'} | ${problems.length ? escapeCell(problems.slice(0, 4).join(', ')) + (problems.length > 4 ? ` (+${problems.length - 4} more)` : '') : 'No fallback detected'} |`);
  }
  lines.push('', 'The JSON report retains all diagnostics and raw timing samples. A parsed block list does not prove identical coverage of nested nodes, embedded items, or MediaTracker data. These checks detect known fallbacks, not every possible parser omission.', '', '## Fixture hashes', '', '| Map | SHA-256 |', '| --- | --- |');
  for (const map of report.maps) lines.push(`| ${escapeCell(map.name)} | \`${map.sha256}\` |`);
  return lines.join('\n') + '\n';
}

export function replaceResults(readme, summary) {
  const startMarker = '<!-- gbx-comparison:start -->';
  const endMarker = '<!-- gbx-comparison:end -->';
  const start = readme.indexOf(startMarker);
  const end = readme.indexOf(endMarker);
  if (start < 0 || end < start) throw new Error('README comparison markers are missing or out of order.');
  return readme.slice(0, start + startMarker.length) + '\n' + summary + '\n' + readme.slice(end);
}
