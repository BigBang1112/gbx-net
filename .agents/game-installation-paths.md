# Game installation paths

Agent reference for locating installed game assets for the games in the [README](../README.md#supported-games). These are common Windows path candidates, not the maintainer's personal installation paths. Confirm the actual directory before using it.

## Common roots

The table uses these placeholders:

- `<Programs>`: `C:\Program Files (x86)`. Also check `C:\Program Files`, especially for older installers.
- `<UbisoftGames>`: `<Programs>\Ubisoft\Ubisoft Game Launcher\games`.
- `<SteamCommon>`: `<Programs>\Steam\steamapps\common`, or `steamapps\common` in another Steam library.

| Game | Installation directory candidates |
| --- | --- |
| Trackmania (2020) | `<UbisoftGames>\Trackmania`, `<SteamCommon>\Trackmania`, `C:\Program Files\Trackmania` |
| ManiaPlanet 4 / 4.1 (Trackmania² / Shootmania) | `<Programs>\ManiaPlanet` |
| Trackmania Turbo | `<UbisoftGames>\Trackmania Turbo`, `<SteamCommon>\Trackmania Turbo` |
| ManiaPlanet 3 (Trackmania² / Shootmania) | `<Programs>\ManiaPlanet`, or a separately preserved installation |
| ManiaPlanet 2 (Trackmania² / Shootmania) | `<Programs>\ManiaPlanet`, or a separately preserved installation |
| ManiaPlanet 1 (Trackmania²) | `<Programs>\ManiaPlanet`, or a separately preserved installation |
| TrackMania Forever (Nations) | `<Programs>\TmNationsForever`, `<SteamCommon>\TrackMania Nations Forever` |
| TrackMania Forever (United, including TMUnlimiter) | `<Programs>\TmUnitedForever`, `<SteamCommon>\TrackMania United`. Check the installation used by TMUnlimiter. |
| Virtual Skipper 5 | `<Programs>\Vsk5`, `<Programs>\Vsk5Online` (Online edition) |
| TrackMania United | `<Programs>\TrackMania United`. Confirm that it has not been updated to Forever. |
| TrackMania Nations ESWC | `<Programs>\TrackMania Nations ESWC` |
| TrackMania Sunrise eXtreme | `<Programs>\TrackMania Sunrise` |
| TrackMania Original | `<Programs>\TmOriginal`, `<Programs>\TrackMania Original` |
| TrackMania Sunrise | `<Programs>\TrackMania Sunrise` |
| TrackMania Power Up | `<Programs>\TrackMania` |
| TrackMania (1.0) | `<Programs>\TrackMania`, `<Programs>\TrackMania2003` |

ManiaPlanet updates can reuse the same directory. A folder named `ManiaPlanet` does not identify its version. Steam installations of Trackmania² Canyon, Stadium, Valley, Lagoon, and Shootmania Storm can have separate game directories under `<SteamCommon>`. Resolve those through Steam rather than assuming they share the standalone installation.

## Asset folders

For a confirmed installation root `<Game>`, inspect these folders recursively:

| Location | Contents to look for |
| --- | --- |
| `<Game>\GameData\` | Loose Gbx files (`*.Gbx`) and other assets. "Raw" here means files outside an archive, not necessarily uncompressed Gbx data. |
| `<Game>\Packs\` | Pak archives (`*.pak`), ZIP archives (`*.zip`), and, in newer games, title/content packs (`*.Pack.Gbx`). |

The exact contents vary by game and installed edition. Do not assume every release has all archive types or that an asset is available as a loose Gbx. A `*.Pack.Gbx` file is a pack container despite its extension.

## Custom installations

Use an explicitly supplied local path first. Otherwise, check the launcher installation location or the target of the game's shortcut. For Steam, inspect `steamapps\libraryfolders.vdf` under the Steam installation and each library's `steamapps\appmanifest_*.acf` (`installdir`) to resolve custom library paths. For older games, Windows uninstall entries can also provide an `InstallLocation`.

Verify the game/version and inspect `GameData` and `Packs` before selecting a source. Documents folders contain user content and are not a substitute for the installation root. ManiaPlanet can also store downloaded packs under `Documents\ManiaPlanet\Packs` and `%ProgramData%\ManiaPlanet\PacksCache`. Check `Nadeo.ini` and game logs for redirected content locations when needed.