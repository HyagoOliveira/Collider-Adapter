# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [3.5.0] - 2024-10-24
### Changed
- Increase unity minimum version to 2023.3
- Update deprecated functions for Unity 6 or greater

## [3.4.0] - 2024-08-01
### Added
- GetIntersection functions

## [3.3.0] - 2024-05-10
### Added
- TryGetOverlapingComponent

### Changed
- Rename functions typo

## [3.2.0] - 2024-03-09
### Added
- ICollider.Raycast function
- IsOverlapingPoint function

## [3.1.0] - 2023-04-12
### Added
- Abstract SyncTransforms function

## [3.0.0] - 2023-04-03
### Removed
- Check for Angle Limit

## [2.0.0] - 2022-03-19
### Added
- Extensions for CapsuleCollider
- Abstract2DColliderAdapter
- BoxCollider2DAdapter
- CapsuleCollider2DAdapter
- CircleCollider2DAdapter
- CompositeCollider2DAdapter
- EdgeCollider2DAdapter
- Abstract3DColliderAdapter
- BoxCollider3DAdapter
- CapsuleCollider3DAdapter
- SphereCollider3DAdapter
- ColliderAdapterFactory

### Changed
- Inside RaycastHit3DAdapter, wrap ArticulationBody using 2020.1 directive

### Removed
- Collider2DAdapter
- Collider3DAdapter

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

[Unreleased]: https://github.com/HyagoOliveira/Collider-Adapter/compare/3.5.0...main
[3.5.0]: https://github.com/HyagoOliveira/Collider-Adapter/tree/3.5.0
[3.4.0]: https://github.com/HyagoOliveira/Collider-Adapter/tree/3.4.0
[3.3.0]: https://github.com/HyagoOliveira/Collider-Adapter/tree/3.3.0
[3.2.0]: https://github.com/HyagoOliveira/Collider-Adapter/tree/3.2.0
[3.1.0]: https://github.com/HyagoOliveira/Collider-Adapter/tree/3.1.0
[3.0.0]: https://github.com/HyagoOliveira/Collider-Adapter/tree/3.0.0
[2.0.0]: https://github.com/HyagoOliveira/Collider-Adapter/tree/2.0.0
[1.1.0]: https://github.com/HyagoOliveira/Collider-Adapter/tree/1.1.0
[1.0.1]: https://github.com/HyagoOliveira/Collider-Adapter/tree/1.0.1
[1.0.0]: https://github.com/HyagoOliveira/Collider-Adapter/tree/1.0.0