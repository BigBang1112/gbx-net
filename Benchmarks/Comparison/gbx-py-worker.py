import contextlib
import importlib.metadata
import json
import os
from pathlib import Path
import platform
import sys
import time
import traceback
import math
import re


def inspect_result(data):
    problems = list(data.get("_errors", [])) + list(data.get("_warns", []))
    problem_counts = {}
    visited = set()

    def visit(value, location):
        if isinstance(value, (dict, list)):
            if id(value) in visited:
                return
            visited.add(id(value))
        if isinstance(value, dict):
            for key, child in value.items():
                if key in ("_unknownChunkId", "_chunkParseFailed", "parse_header_chunk_failed", "unknown_chunk_in_node_ref"):
                    diagnostic = re.sub(r"/\d+(?=/|$)", "/[*]", f"{location}: {key}")
                    problem_counts[diagnostic] = problem_counts.get(diagnostic, 0) + 1
                elif key == "rest" and child:
                    problems.append(f"{location}: unparsed trailing bytes")
                elif not isinstance(key, str) or not key.startswith("_"):
                    visit(child, f"{location}/{key}")
        elif isinstance(value, list):
            for index, child in enumerate(value):
                visit(child, f"{location}/{index}")

    visit(data, "map")
    problems.extend(f"{diagnostic} ({count} occurrences)" for diagnostic, count in problem_counts.items())
    block_chunk = data.body.get(0x0304301F, {})
    blocks = block_chunk.get("Blocks")
    if not isinstance(blocks, list):
        problems.append("No parsed block list (0x0304301F).")
    if 0xFACADE01 not in data.body:
        problems.append("Map body end marker was not reached.")
    return {
        "blockCount": sum(block.flags != -1 for block in blocks) if isinstance(blocks, list) else None,
        "problems": list(dict.fromkeys(problems)),
    }


def run():
    map_path, output_path, sample_count, warmup_seconds, sample_ms, source_path = sys.argv[1:]
    try:
        sys.path.insert(0, source_path)
        from src.parser import parse_bytes

        raw_bytes = Path(map_path).read_bytes()
        with open(os.devnull, "w", encoding="utf-8") as sink, contextlib.redirect_stdout(sink), contextlib.redirect_stderr(sink):
            last_result = parse_bytes(raw_bytes)
            validation = inspect_result(last_result)
            warmup_start = time.perf_counter()
            warmup_operations = 0
            while True:
                last_result = parse_bytes(raw_bytes)
                warmup_operations += 1
                if time.perf_counter() - warmup_start >= float(warmup_seconds):
                    break
            operations = min(10000, max(1, math.ceil(float(sample_ms) * warmup_operations / ((time.perf_counter() - warmup_start) * 1000))))
            samples = []
            for sample_index in range(int(sample_count)):
                start = time.perf_counter()
                for operation_index in range(operations):
                    last_result = parse_bytes(raw_bytes)
                samples.append((time.perf_counter() - start) * 1000 / operations)
        result = {
            "status": "partial" if validation["problems"] else "ok",
            "version": importlib.metadata.version("gbx-py"),
            "constructVersion": importlib.metadata.version("construct"),
            "runtime": f"CPython {platform.python_version()}",
            "architecture": platform.machine(),
            "validation": validation,
            "warmupOperations": warmup_operations,
            "operationsPerSample": operations,
            "samplesMs": samples,
        }
    except Exception:
        result = {"status": "error", "error": traceback.format_exc()}
    Path(output_path).write_text(json.dumps(result, indent=2), encoding="utf-8")
    return 1 if result["status"] == "error" else 0


if __name__ == "__main__":
    sys.exit(run())
