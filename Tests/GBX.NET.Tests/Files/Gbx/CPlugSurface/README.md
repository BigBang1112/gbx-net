# Minimal GmSurf fixtures

These 23 bespoke shapes contain only constructed geometry and synthetic
material tables, with no third-party items or external references.
The fixtures and accompanying contribution are dedicated to the public
domain and may be relicensed at the maintainer's convenience.

## Generation and native verification

The initial primitives were created from new CPlugSurface nodes using
Editor++ in Trackmania 2020. The remaining shapes were constructed as minimal
GBX v6/CPlugSurface archives using native serializer layouts, then preloaded
and saved by TM2020 through Editor++. All bytes are bespoke, not extracted
from an existing item. The compressed files here are those native saves.
The three Version0 files retain their constructed, uncompressed archive to
exercise the older version instead of TM2020's current save format.

On 2026-09-12 at 09:00 AEST, every exact file below was copied to a new
User-drive path, confirmed unloaded, and disk-preloaded using Fids::Preload
through ControlFids. Every result reported alreadyLoaded: false, loaded: true,
and className: CPlugSurface. Editor++ then saved each loaded node to a
different path. All 20 current-version files resaved byte-identically.
The three Version0 files loaded and saved successfully, but the native save
upgraded their archive version; those saves are not byte-identical.

At 09:11 AEST, a second pass parsed all 23 fixtures with this GBX.NET patch,
changed gameplay direction and applicable primitive dimensions, and saved
new files. All 23 GBX.NET outputs disk-preloaded and resaved in TM2020.
The entire decompressed native output matched the GBX.NET-written archive,
including the edited fields. Compressed encodings differed.

This proves native disk preload and archive/resave compatibility, **not**
collision simulation, item placement, or map-editor behavior.
CompoundInstance and Voxel have no subclass archive payload; the game's
serializer does not persist their runtime geometry.

GmSurfTests parses these exact files through the public GBX.NET API, saves
and reparses them, and compares the entire uncompressed archive against the
fixture, including material tables and the chunk terminator.

## Contents

| ID | Type | Minimal content |
| --- | --- | --- |
| -1 | Null | No surface or gameplay direction |
| 0 | Sphere | Radius 1 |
| 1 | Ellipsoid | Scale (1, 2, 3) |
| 6 | Box | Origin-centered unit half-extents |
| 7 | Mesh | One triangle, three vertices |
| 8 | VCylinder | One primitive |
| 9 | MultiSphere | One radius-1 sphere at the origin |
| 10 | ConvexPolyhedron | Unit tetrahedron; Procedural uses an octagonal prism |
| 11 | Capsule | Center (0, 0, 0), direction (0, 1, 0), length 1 |
| 12 | Circle | One circle |
| 13 | Compound | One sphere with an identity transform |
| 14 | SphereLocated | Center (0, 0, 0), radius 1 |
| 15 | CompoundInstance | Empty subclass payload |
| 16 | Cylinder | One primitive |
| 17 | SphericalShell | Inner radius 0.5, outer radius 1 |
| 18 | Voxel | Empty subclass payload |
| 19 | Diggable | Origin-centered unit box, five zero archive words |

Primary and Fallback exercise nonempty material-ID arrays. Version0 exercises
the unconditional surface index in types 8, 14, and 16.
Legacy IDs 2–5 (Plane/height/polygon variants) are not constructed by TM2020's
GmSurf factory and are intentionally not claimed as supported here.

## Verified SHA-256

| File | SHA-256 |
| --- | --- |
| GmSurfFixture-1.Shape.Gbx | `52b0ed129db617748fe5a206671efac3a867706299ed59b832ebbef383f614a9` |
| GmSurfFixture0.Shape.Gbx | `09f044f838df98182f80c1be4e3861cbec51d8225623b09866d598cd31cd0bee` |
| GmSurfFixture1.Shape.Gbx | `8f7adf98fed72188d82dd6a36e953985811e4bb2deecf469d75a2cd5b5883124` |
| GmSurfFixture10.Shape.Gbx | `b233031c8f044d17e5913b31aaf7d60d0a040d558e8f63a3531d08ed66515587` |
| GmSurfFixture10Procedural.Shape.Gbx | `7f0fa9fcb21b2ee995944ab6b8e1162b7f1934efeefc1d56516fd67418e865bf` |
| GmSurfFixture11.Shape.Gbx | `5a9636c6e4badd3e617ccee332a056e734c99996caa585ba8d650f0d3944a00a` |
| GmSurfFixture12.Shape.Gbx | `69226cc835876481cd0f40c0531fe813e30c49cb66d09ddded8fc87163c756e7` |
| GmSurfFixture13.Shape.Gbx | `fccaaf36ccd478f5da16d3eddc0bf4c433809731065c66d065c1c2b2a980ded1` |
| GmSurfFixture14.Shape.Gbx | `faa44b12a85b1b1ea85b235f6921e18205fbe2b330f29d8df602cfdef0be69f1` |
| GmSurfFixture14Version0.Shape.Gbx | `24c69cb309401bd33661cba012d85c910d3d3a3d55b3c48622ce27c80054c8a7` |
| GmSurfFixture15.Shape.Gbx | `b8e94b747fed35a0c89784a4c6bfdf8e36ba8f5c902f667375d07a71fa5eec0d` |
| GmSurfFixture16.Shape.Gbx | `84110134ed4422ee86cbfd7d3f1b60ca521128af76ccba7e49bd869c31a784cd` |
| GmSurfFixture16Version0.Shape.Gbx | `413e02a6832aab95bc4d0416f996c5bae545bb4c58657b5ac8f23c506b13f180` |
| GmSurfFixture17.Shape.Gbx | `56a3498dfba9c797a38a3007a4ff0028080461285efa30f7aca235018b88605f` |
| GmSurfFixture18.Shape.Gbx | `2a96d9d3ed381514ff36317c196371b3deeaf89e3474033fabe382d40970ba93` |
| GmSurfFixture19.Shape.Gbx | `818d9d3510632a0d070092fd7f2ac79b2daa4573756546d48f7428651a29d67a` |
| GmSurfFixture6.Shape.Gbx | `a7e5a28b496d0a1c4b7eafb94895e2ffc1771564b1e7af5a3ab1d102f38a7a72` |
| GmSurfFixture7.Shape.Gbx | `47547b11e5c63b399a266d02abc0f5a90acd17a978ae20e1dcf0bcf66dbe764f` |
| GmSurfFixture8.Shape.Gbx | `6ac75631f42fe83c0a22b2f22f0b03616ea5660869d97617b320102c08206cce` |
| GmSurfFixture8Version0.Shape.Gbx | `47dfeec841c72c522128fcd911e78239fb96baaa57752c705f076968fa5679ed` |
| GmSurfFixture9.Shape.Gbx | `0db5ea85a394513d24883cbb48eb5555acea4dc82794d466a1d619ed4c6b0d35` |
| GmSurfFixture9Fallback.Shape.Gbx | `a638c098686ca6b25864e6ac21e34cd51fefb359a2ade6d5cbedfa98038b7f81` |
| GmSurfFixture9Primary.Shape.Gbx | `81092e50d6f738220f8e51d9451b1c99b5f611d4b420eab93dc72fdd022a23d3` |
