"""Pack metadata, publish results, and release notes for publish-nuget.yml."""

import json
import os
import re
import subprocess
import sys
import textwrap
import time
import urllib.error
import urllib.request
import xml.etree.ElementTree as ET
import zipfile
from pathlib import Path


PACKAGE_DIR = Path("packages")
MANIFEST = PACKAGE_DIR / "manifest.json"
PUBLISH_RESULTS = PACKAGE_DIR / "publish-results.json"
NEWLY_UPLOADED = PACKAGE_DIR / "newly-uploaded.json"
RELEASE_NOTES = Path("release-notes.md")
PUSHED = re.compile(r"(?m)^\s*Your package was pushed\.\s*$")
CREATED = re.compile(r"(?m)^\s*Created\s+https?://")
DUPLICATE = re.compile(r"\b(?:already exists|409|conflict)\b", re.I)


def run(command):
    return subprocess.run(command, capture_output=True, text=True, errors="replace", check=False)


def package_identity(package):
    with zipfile.ZipFile(package) as archive:
        nuspecs = [name for name in archive.namelist() if name.endswith(".nuspec")]
        if len(nuspecs) != 1:
            raise ValueError(f"Expected one .nuspec in {package}")
        metadata = ET.fromstring(archive.read(nuspecs[0])).find("{*}metadata")
        if metadata is None:
            raise ValueError(f"Missing metadata in {package}")
        package_id = metadata.findtext("{*}id")
        version = metadata.findtext("{*}version")
        if not package_id or not version:
            raise ValueError(f"Missing package ID or version in {package}")
        return package_id, version


def project_properties(project):
    result = run([
        "dotnet", "msbuild", str(project), "-nologo",
        "-property:Configuration=Release", "-property:ContinuousIntegrationBuild=true",
        "-getProperty:PackageId,Version,PackageReleaseNotes",
    ])
    if result.returncode:
        raise RuntimeError(f"Could not read package properties from {project}: {result.stderr}")
    return json.loads(result.stdout)["Properties"]


def prepare():
    projects = sorted(Path("Src").glob("*/*.csproj")) + [Path("Templates/GBX.NET.Templates.csproj")]
    by_identity = {}
    for project in projects:
        properties = project_properties(project)
        package_id = properties["PackageId"] or project.stem
        version = properties["Version"]
        key = (package_id.casefold(), version.casefold())
        if key in by_identity:
            raise ValueError(f"Multiple projects produce {package_id} {version}")
        by_identity[key] = (project, properties["PackageReleaseNotes"])

    packages = sorted(PACKAGE_DIR.glob("*.nupkg"))
    if not packages:
        raise ValueError("No NuGet packages were packed")

    manifest = []
    for package in packages:
        package_id, version = package_identity(package)
        key = (package_id.casefold(), version.casefold())
        if key not in by_identity:
            raise ValueError(f"No csproj matches packed package {package_id} {version}")
        project, notes = by_identity[key]
        manifest.append({
            "path": str(package),
            "id": package_id,
            "version": version,
            "project": str(project),
            "notes": textwrap.dedent(notes).strip(),
        })

    MANIFEST.write_text(json.dumps(manifest, indent=2, ensure_ascii=False), encoding="utf-8")
    print(f"Prepared metadata for {len(manifest)} packages")


def classify_push(returncode, output):
    if returncode != 0:
        return "failed"
    if DUPLICATE.search(output):
        return "duplicate"
    if PUSHED.search(output) or CREATED.search(output):
        return "uploaded"
    return "unconfirmed"


def push(package, feed, url, key, no_symbols=False):
    if not key:
        print(f"{package.name} -> {feed}: missing API key")
        return "failed"
    command = [
        "dotnet", "nuget", "push", str(package),
        "--api-key", key, "--source", url,
        "--skip-duplicate", "--force-english-output",
    ]
    if no_symbols:
        command.append("--no-symbols")
    result = run(command)
    output = (result.stdout + "\n" + result.stderr).replace(key, "[redacted]")
    status = classify_push(result.returncode, output)
    print(f"{package.name} -> {feed}: {status}")
    if status in ("failed", "unconfirmed"):
        print(output)
    return status


def append_output(key, value):
    if path := os.environ.get("GITHUB_OUTPUT"):
        with open(path, "a", encoding="utf-8") as output:
            output.write(f"{key}={value}\n")


def append_summary(text):
    if path := os.environ.get("GITHUB_STEP_SUMMARY"):
        with open(path, "a", encoding="utf-8") as summary:
            summary.write(text)


def publish():
    manifest = json.loads(MANIFEST.read_text(encoding="utf-8"))
    feeds = [
        ("NuGet.org", "https://api.nuget.org/v3/index.json", os.environ.get("NUGET_API_KEY", "")),
        ("GitHub Packages", f"https://nuget.pkg.github.com/{os.environ['GITHUB_REPOSITORY_OWNER']}/index.json", os.environ.get("GITHUB_PACKAGES_TOKEN", "")),
        ("GBX tools", os.environ["CUSTOM_FEED_URL"], os.environ.get("CUSTOM_FEED_API_KEY", "")),
    ]
    newly_uploaded = []
    results = []
    has_failures = False

    for package in manifest:
        package_path = Path(package["path"])
        statuses = {}
        for feed, url, key in feeds:
            status = push(package_path, feed, url, key, no_symbols=True)
            statuses[feed] = status
            has_failures |= status in ("failed", "unconfirmed")

        # A symbols-only upload does not make a package newly published.
        symbol_path = package_path.with_suffix(".snupkg")
        if symbol_path.exists():
            _, url, key = feeds[0]
            symbol_status = push(symbol_path, "NuGet.org symbols", url, key)
            statuses["NuGet.org symbols"] = symbol_status
            has_failures |= symbol_status in ("failed", "unconfirmed")

        results.append({"id": package["id"], "version": package["version"], "feeds": statuses})
        if any(status == "uploaded" for feed, status in statuses.items() if feed != "NuGet.org symbols"):
            newly_uploaded.append(package)

    PUBLISH_RESULTS.write_text(json.dumps(results, indent=2), encoding="utf-8")
    NEWLY_UPLOADED.write_text(json.dumps(newly_uploaded, indent=2, ensure_ascii=False), encoding="utf-8")
    append_output("has-new", str(bool(newly_uploaded)).lower())
    append_output("has-failures", str(has_failures).lower())

    lines = ["## NuGet publishing", "", "| Package | NuGet.org | GitHub Packages | GBX tools | Symbols |", "| --- | --- | --- | --- | --- |"]
    for result in results:
        statuses = result["feeds"]
        lines.append(f"| {result['id']} {result['version']} | {statuses['NuGet.org']} | {statuses['GitHub Packages']} | {statuses['GBX tools']} | {statuses.get('NuGet.org symbols', 'none')} |")
    lines.extend(["", f"Newly uploaded packages: {len(newly_uploaded)}.", ""])
    append_summary("\n".join(lines))


def compose_release_notes(packages):
    main = select_main_package(packages)
    sections = []
    if description := os.environ.get("OPTIONAL_DESCRIPTION", "").strip():
        sections.append(description)
    sections.append(main["notes"] or "*No release notes provided.*")
    for package in sorted((package for package in packages if package is not main), key=lambda item: item["id"].casefold()):
        notes = package["notes"] or "*No release notes provided.*"
        sections.append(f"## {package['id']} {package['version']}\n\n{notes}")
    repository = os.environ["GITHUB_REPOSITORY"]
    run_id = os.environ["GITHUB_RUN_ID"]
    sections.append(f"Assets were automatically generated using the [publish workflow](<https://github.com/{repository}/actions/runs/{run_id}>).")
    return "\n\n".join(sections) + "\n"


def version_key(package):
    core, _, prerelease = package["version"].split("+", 1)[0].partition("-")
    numbers = tuple(int(part) for part in core.split("."))
    return numbers, not prerelease, prerelease


def select_main_package(packages):
    main_package_id = os.environ.get("MAIN_PACKAGE", "GBX.NET")
    return next((package for package in packages if package["id"].casefold() == main_package_id.casefold()), None) or max(packages, key=version_key)


def release():
    packages = json.loads(NEWLY_UPLOADED.read_text(encoding="utf-8"))
    if not packages:
        return

    RELEASE_NOTES.write_text(compose_release_notes(packages), encoding="utf-8")
    main = select_main_package(packages)
    tag = os.environ["GITHUB_REF_NAME"] if os.environ.get("GITHUB_REF_TYPE") == "tag" else f"v{main['version']}"
    title = f"{main['id']} {tag.removeprefix('v')}"

    assets = []
    for package in packages:
        package_path = Path(package["path"])
        assets.append(str(package_path))
        symbol_path = package_path.with_suffix(".snupkg")
        if symbol_path.exists():
            assets.append(str(symbol_path))

    command = [
        "gh", "release", "create", tag, *assets,
        "--title", title, "--notes-file", str(RELEASE_NOTES),
        "--target", os.environ["GITHUB_SHA"], "--repo", os.environ["GITHUB_REPOSITORY"],
    ]
    if os.environ.get("GITHUB_REF_TYPE") == "tag":
        command.append("--verify-tag")
    result = run(command)
    if result.returncode:
        raise RuntimeError(f"GitHub release creation failed: {result.stderr}")
    url = result.stdout.strip()
    print(f"Created {url} with {len(packages)} newly uploaded packages")
    append_output("created", "true")
    append_output("url", url)


def compose_discord_message(packages, release_url, nuget_available):
    main = select_main_package(packages)
    main_section = [f"## {main['id']} {main['version']}"]
    if description := os.environ.get("OPTIONAL_DESCRIPTION", "").strip():
        main_section.append(description)
    main_section.append(main["notes"] or "*No release notes provided.*")
    sections = ["\n\n".join(main_section)]

    for package in packages:
        if package is main:
            continue
        notes = package["notes"] or "*No release notes provided.*"
        sections.append(f"### {package['id']} {package['version']}\n\n{notes}")

    links = [f"GitHub: <{release_url}>"]
    if nuget_available:
        nuget_url = f"https://www.nuget.org/packages/{main['id']}/{main['version']}"
        links.append(f"NuGet: <{nuget_url}>")
    links.append("Explorer: <https://explorer.gbx.tools/>")
    sections.append("\n".join(links))
    return "\n\n".join(sections) + "\n"


def utf16_length(value):
    return len(value.encode("utf-16-le")) // 2


def split_discord_message(message, limit=1900):
    chunk = ""
    for line in message.splitlines(keepends=True):
        if not chunk:
            if utf16_length(line) > limit:
                raise ValueError("A Discord message line exceeds the limit and cannot be split at a line break")
            chunk = line
        elif utf16_length(chunk + line) <= limit:
            chunk += line
        else:
            yield chunk
            if utf16_length(line) > limit:
                raise ValueError("A Discord message line exceeds the limit and cannot be split at a line break")
            chunk = line
    if chunk:
        yield chunk


def discord():
    webhook = os.environ.get("DISCORD_WEBHOOK_URL")
    if not webhook:
        raise ValueError("notify-discord was selected, but DISCORD_WEBHOOK_URL is not configured")
    packages = json.loads(NEWLY_UPLOADED.read_text(encoding="utf-8"))
    results = json.loads(PUBLISH_RESULTS.read_text(encoding="utf-8"))
    main = select_main_package(packages)
    nuget_available = any(
        result["id"] == main["id"] and result["version"] == main["version"]
        and result["feeds"].get("NuGet.org") in ("uploaded", "duplicate")
        for result in results
    )
    message = compose_discord_message(packages, os.environ["RELEASE_URL"], nuget_available)
    opener = urllib.request.build_opener()
    opener.addheaders = []
    # Validate every line before sending anything, so an overlong line cannot cause a partial announcement.
    for chunk in list(split_discord_message(message)):
        payload = json.dumps({"content": chunk}).encode("utf-8")
        for attempt in range(4):
            request = urllib.request.Request(webhook, payload, {
                "Content-Type": "application/json",
            }, method="POST")
            try:
                with opener.open(request, timeout=30):
                    break
            except urllib.error.HTTPError as error:
                if error.code == 429 and attempt < 3:
                    time.sleep(float(error.headers.get("Retry-After", "1")))
                    continue
                body = error.read().decode("utf-8", errors="replace").strip()
                try:
                    detail = json.loads(body).get("message", body)
                except json.JSONDecodeError:
                    detail = body
                suffix = f": {detail}" if detail else ""
                raise RuntimeError(f"Discord webhook request failed with HTTP {error.code}{suffix}") from None


if __name__ == "__main__":
    operations = {"prepare": prepare, "publish": publish, "release": release, "discord": discord}
    if len(sys.argv) != 2 or sys.argv[1] not in operations:
        sys.exit(f"Usage: {sys.argv[0]} <{'|'.join(operations)}>")
    operations[sys.argv[1]]()
