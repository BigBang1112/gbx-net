import io
import json
import os
import subprocess
import tempfile
import unittest
import zipfile
from pathlib import Path
from unittest.mock import patch

import publish_nuget as target


class PublishNugetTests(unittest.TestCase):
    def test_prepare_matches_packed_identity_to_csproj_release_notes(self):
        with tempfile.TemporaryDirectory() as directory:
            old_directory = Path.cwd()
            try:
                os.chdir(directory)
                Path("Src/PackageX").mkdir(parents=True)
                Path("Templates").mkdir()
                Path("packages").mkdir()
                Path("Src/PackageX/PackageX.csproj").touch()
                Path("Templates/GBX.NET.Templates.csproj").touch()
                with zipfile.ZipFile("packages/Package.X.1.0.0.nupkg", "w") as archive:
                    archive.writestr(
                        "Package.X.nuspec",
                        '<package xmlns="http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd">'
                        '<metadata><id>Package.X</id><version>1.0.0</version></metadata></package>',
                    )

                def properties(project):
                    if project.stem == "PackageX":
                        return {"PackageId": "Package.X", "Version": "1.0.0", "PackageReleaseNotes": "Fixed parsing."}
                    return {"PackageId": "GBX.NET.Templates", "Version": "0.1.0", "PackageReleaseNotes": ""}

                with patch.object(target, "project_properties", side_effect=properties):
                    target.prepare()
                manifest = json.loads(target.MANIFEST.read_text(encoding="utf-8"))
                self.assertEqual("Src/PackageX/PackageX.csproj", manifest[0]["project"])
                self.assertEqual("Fixed parsing.", manifest[0]["notes"])
            finally:
                os.chdir(old_directory)

    def test_push_result_must_confirm_new_upload(self):
        self.assertEqual("uploaded", target.classify_push(0, "Your package was pushed.\n"))
        self.assertEqual("uploaded", target.classify_push(0, "  Created https://example.test/api/v2/package/\n"))
        self.assertEqual("duplicate", target.classify_push(0, "warn : 409 Conflict: package already exists\n"))
        self.assertEqual("duplicate", target.classify_push(0, "already exists\nYour package was pushed.\n"))
        self.assertEqual("unconfirmed", target.classify_push(0, "Push finished\n"))
        self.assertEqual("failed", target.classify_push(1, "Your package was pushed.\n"))

    def test_only_confirmed_uploads_enter_release_manifest(self):
        with tempfile.TemporaryDirectory() as directory:
            old_directory = Path.cwd()
            try:
                os.chdir(directory)
                Path("packages").mkdir()
                packages = [
                    {"path": f"packages/{name}.1.0.0.nupkg", "id": name, "version": "1.0.0", "notes": name}
                    for name in ("New", "Duplicate", "Partial")
                ]
                target.MANIFEST.write_text(json.dumps(packages), encoding="utf-8")
                output = Path("output.txt")
                summary = Path("summary.md")

                def push(package, feed, url, key, no_symbols=False):
                    if package.name.startswith("New"):
                        return "uploaded" if feed == "NuGet.org" else "duplicate"
                    if package.name.startswith("Partial"):
                        return "uploaded" if feed == "GitHub Packages" else "failed"
                    return "duplicate"

                environment = {
                    "GITHUB_REPOSITORY_OWNER": "owner",
                    "CUSTOM_FEED_URL": "https://example.test/v3/index.json",
                    "GITHUB_OUTPUT": str(output.resolve()),
                    "GITHUB_STEP_SUMMARY": str(summary.resolve()),
                }
                with patch.dict(os.environ, environment), patch.object(target, "push", side_effect=push):
                    target.publish()

                uploaded = json.loads(target.NEWLY_UPLOADED.read_text(encoding="utf-8"))
                self.assertEqual(["New", "Partial"], [package["id"] for package in uploaded])
                self.assertIn("has-new=true", output.read_text(encoding="utf-8"))
                self.assertIn("has-failures=true", output.read_text(encoding="utf-8"))
            finally:
                os.chdir(old_directory)

    def test_release_notes_use_only_uploaded_packages_and_optional_text(self):
        environment = {
            "OPTIONAL_DESCRIPTION": "Introduction",
            "MAIN_PACKAGE": "Package.X",
            "GITHUB_REPOSITORY": "owner/repo",
            "GITHUB_RUN_ID": "123",
        }
        packages = [
            {"id": "Package.X", "version": "1.0.0", "notes": "Fixed a bug."},
            {"id": "Package.Y", "version": "9.0.0", "notes": "Added a feature."},
        ]
        with patch.dict(os.environ, environment):
            notes = target.compose_release_notes(packages)
        self.assertEqual(
            "Introduction\n\nFixed a bug.\n\n## Package.Y 9.0.0\n\nAdded a feature.\n\n"
            "Assets were automatically generated using the "
            "[publish workflow](<https://github.com/owner/repo/actions/runs/123>).\n",
            notes,
        )

    def test_release_attaches_only_uploaded_package_assets(self):
        with tempfile.TemporaryDirectory() as directory:
            old_directory = Path.cwd()
            try:
                os.chdir(directory)
                Path("packages").mkdir()
                package = {"path": "packages/New.1.0.0.nupkg", "id": "New", "version": "1.0.0", "notes": "New notes"}
                target.NEWLY_UPLOADED.write_text(json.dumps([package]), encoding="utf-8")
                environment = {
                    "GITHUB_REPOSITORY": "owner/repo",
                    "GITHUB_RUN_ID": "123",
                    "GITHUB_REF_NAME": "v1.0.0",
                    "GITHUB_REF_TYPE": "tag",
                    "GITHUB_SHA": "abc123",
                }
                with patch.dict(os.environ, environment), patch.object(
                    target, "run", return_value=subprocess.CompletedProcess([], 0, "https://github.com/owner/repo/releases/tag/v1.0.0\n", "")
                ) as run:
                    target.release()
                command = run.call_args.args[0]
                self.assertIn("packages/New.1.0.0.nupkg", command)
                self.assertNotIn("packages/Duplicate.1.0.0.nupkg", command)
                self.assertIn("--verify-tag", command)
            finally:
                os.chdir(old_directory)

    def test_discord_chunks_respect_utf16_limit(self):
        message = ("\U0001F600" * 400 + "\n") * 5
        chunks = list(target.split_discord_message(message))
        self.assertGreater(len(chunks), 1)
        self.assertEqual(message, "".join(chunks))
        self.assertTrue(all(chunk.endswith("\n") for chunk in chunks))
        self.assertTrue(all(len(chunk.encode("utf-16-le")) // 2 <= 1900 for chunk in chunks))

    def test_discord_never_splits_a_long_line(self):
        with self.assertRaisesRegex(ValueError, "cannot be split at a line break"):
            list(target.split_discord_message("a" * 1901))

    def test_discord_message_formats_packages_and_links(self):
        packages = [
            {"id": "GBX.NET", "version": "2.4.4", "notes": "- Added a chunk"},
            {"id": "GBX.NET.PAK", "version": "2.4.4", "notes": "- Fixed blocks"},
            {"id": "GBX.NET.Crypto", "version": "1.2.2", "notes": "- Added MD5"},
        ]
        environment = {"OPTIONAL_DESCRIPTION": "Introduction"}
        release_url = "https://github.com/owner/repo/releases/tag/v2.4.4"
        with patch.dict(os.environ, environment):
            message = target.compose_discord_message(packages, release_url, True)
            message_without_nuget = target.compose_discord_message(packages, release_url, False)
        self.assertEqual(
            "## GBX.NET 2.4.4\n\nIntroduction\n\n- Added a chunk\n\n"
            "### GBX.NET.PAK 2.4.4\n\n- Fixed blocks\n\n"
            "### GBX.NET.Crypto 1.2.2\n\n- Added MD5\n\n"
            f"GitHub: <{release_url}>\n"
            "NuGet: <https://www.nuget.org/packages/GBX.NET/2.4.4>\n"
            "Explorer: <https://explorer.gbx.tools/>\n",
            message,
        )
        self.assertNotIn("NuGet:", message_without_nuget)

    def test_discord_posts_multiple_messages_without_extra_text(self):
        with tempfile.TemporaryDirectory() as directory:
            old_directory = Path.cwd()
            try:
                os.chdir(directory)
                Path("packages").mkdir()
                package = {"id": "GBX.NET", "version": "2.4.4", "notes": ("- Changed a thing\n" * 200).strip()}
                target.NEWLY_UPLOADED.write_text(json.dumps([package]), encoding="utf-8")
                target.PUBLISH_RESULTS.write_text(json.dumps([
                    {"id": "GBX.NET", "version": "2.4.4", "feeds": {"NuGet.org": "uploaded"}}
                ]), encoding="utf-8")
                environment = {
                    "DISCORD_WEBHOOK_URL": "https://discord.example/webhook",
                    "RELEASE_URL": "https://github.com/owner/repo/releases/tag/v2.4.4",
                    "OPTIONAL_DESCRIPTION": "",
                }
                with patch.dict(os.environ, environment), patch.object(target.urllib.request, "build_opener") as build_opener:
                    target.discord()
                opener = build_opener.return_value
                self.assertEqual([], opener.addheaders)
                contents = [json.loads(call.args[0].data)["content"] for call in opener.open.call_args_list]
                self.assertGreater(len(contents), 1)
                self.assertNotIn("Continued from previous message", "".join(contents))
                self.assertTrue(all(len(content.encode("utf-16-le")) // 2 <= 1900 for content in contents))
                self.assertIn("NuGet: <https://www.nuget.org/packages/GBX.NET/2.4.4>", contents[-1])
            finally:
                os.chdir(old_directory)

    def test_discord_reports_discord_error_details_without_webhook_url(self):
        with tempfile.TemporaryDirectory() as directory:
            old_directory = Path.cwd()
            try:
                os.chdir(directory)
                Path("packages").mkdir()
                package = {"id": "GBX.NET", "version": "2.4.4", "notes": "- Changed a thing"}
                target.NEWLY_UPLOADED.write_text(json.dumps([package]), encoding="utf-8")
                target.PUBLISH_RESULTS.write_text(json.dumps([
                    {"id": "GBX.NET", "version": "2.4.4", "feeds": {"NuGet.org": "uploaded"}}
                ]), encoding="utf-8")
                environment = {
                    "DISCORD_WEBHOOK_URL": "https://discord.example/private-webhook",
                    "RELEASE_URL": "https://github.com/owner/repo/releases/tag/v2.4.4",
                    "OPTIONAL_DESCRIPTION": "",
                }
                error = target.urllib.error.HTTPError(
                    "https://discord.example/private-webhook", 403, "Forbidden", {}, io.BytesIO(b'{"message":"Missing Access"}')
                )
                opener = unittest.mock.MagicMock()
                opener.open.side_effect = error
                with patch.dict(os.environ, environment), patch.object(target.urllib.request, "build_opener", return_value=opener):
                    with self.assertRaisesRegex(RuntimeError, "HTTP 403: Missing Access") as context:
                        target.discord()
                self.assertNotIn("private-webhook", str(context.exception))
            finally:
                os.chdir(old_directory)

    def test_discord_rejects_overlong_line_before_posting(self):
        with tempfile.TemporaryDirectory() as directory:
            old_directory = Path.cwd()
            try:
                os.chdir(directory)
                Path("packages").mkdir()
                package = {"id": "GBX.NET", "version": "2.4.4", "notes": "- Short\n" * 200 + "x" * 1901}
                target.NEWLY_UPLOADED.write_text(json.dumps([package]), encoding="utf-8")
                target.PUBLISH_RESULTS.write_text(json.dumps([
                    {"id": "GBX.NET", "version": "2.4.4", "feeds": {"NuGet.org": "uploaded"}}
                ]), encoding="utf-8")
                environment = {
                    "DISCORD_WEBHOOK_URL": "https://discord.example/webhook",
                    "RELEASE_URL": "https://github.com/owner/repo/releases/tag/v2.4.4",
                    "OPTIONAL_DESCRIPTION": "",
                }
                with patch.dict(os.environ, environment), patch.object(target.urllib.request, "build_opener") as build_opener:
                    with self.assertRaisesRegex(ValueError, "cannot be split at a line break"):
                        target.discord()
                build_opener.assert_not_called()
            finally:
                os.chdir(old_directory)


if __name__ == "__main__":
    unittest.main()
