# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Changed
- Inside RaycastHit3DAdapter, wrap ArticulationBody using 2020.1 directive

### Removed
- Collider2DAdapter

## [1.1.0] - 2022-01-10
### Added
- Capsule Cast to 3D Adapter
- Capsule Cast to 2D Adapter
- Draw param to Cast functions

### Changed
- Update Shapes package to 1.2.0
- Hides inherited member for collider fields
- Improve Cast functions

## [1.0.1] - 2022-01-05
### Added
- Shapes package dependency

## [1.0.0] - 2022-01-04
### Added
- TriggerActionDetector and TriggerEventDetector
- AbstractColliderAdapter and ColliderAdapters (2D/3D)
- Collider Extensions (2D/3D)
- RaycastHit Extensions (2D/3D)
- IRaycastHit and RaycastHitAdapter (2D/3D)
- CastFilter (2D/3D)
- CHANGELOG
- README
- Initial commit

[Unreleased]: https://bitbucket.org/nostgameteam/[repo_url]/branches/compare/master%0D1.1.0
[1.1.0]: https://bitbucket.org/nostgameteam/[repo_url]/src/1.1.0/
[1.0.1]: https://bitbucket.org/nostgameteam/[repo_url]/src/1.0.1/
[1.0.0]: https://bitbucket.org/nostgameteam/[repo_url]/src/1.0.0/