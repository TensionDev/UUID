# TensionDev.UUID

# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Changed
- Changed folder structure to match the TensionDev.UUID namespace.


## [v2.4.0] - 2026-01-30
[v2.4.0](https://github.com/TensionDev/UUID/releases/tag/v2.4.0)

### Changed
- Changed ToVariant1 to ToRfc4122Variant for clarity.
- Changed ToVariant2 to ToMicrosoftVariant for clarity.


## [v2.3.1] - 2025-07-01
[v2.3.1](https://github.com/TensionDev/UUID/releases/tag/v2.3.1)

### Fixed
- Fixed Node ID for generated UUID v1 to check for valid MAC address.


## [v2.3.0] - 2025-03-30
[v2.3.0](https://github.com/TensionDev/UUID/releases/tag/v2.3.0)

### Added
- Added functions to test which version of UUID it is.
- Added ToDateTime functions to return the DateTime objects for date and time related UUIDs.


## [v2.2.0] - 2025-02-22
[v2.2.0](https://github.com/TensionDev/UUID/releases/tag/v2.2.0)

### Changed
- Changed Classes to static with private fields.


## [v2.1.0] - 2024-06-05
[v2.1.0](https://github.com/TensionDev/UUID/releases/tag/v2.1.0)


## [v2.1.0-beta] - 2024-06-05
[v2.1.0-beta](https://github.com/TensionDev/UUID/releases/tag/v2.1.0-beta)

### Changed
- Added UUID v7 generated based on current system date and time as well as Fixed Bit-Length Dedicated Counter (Method 1).
- Added UUID v7 generated based on current system date and time as well as Replace Leftmost Random Bits with Increased Clock Precision (Method 3).


## [v2.1.0-alpha] - 2023-06-15
[v2.1.0-alpha](https://github.com/TensionDev/UUID/releases/tag/v2.1.0-alpha)

### Changed
- Changed license from GNU General Public License Version 3 to Apache License Version 2.0.


## [v2.0.0] - 2023-06-15
[v2.0.0](https://github.com/TensionDev/UUID/releases/tag/v2.0.0)

### Changed
- Changed license from GNU General Public License Version 3 to Apache License Version 2.0.


## [v1.2.0-alpha] - 2022-12-15
[v1.2.0-alpha](https://github.com/TensionDev/UUID/releases/tag/v1.2.0-alpha)

### Added
- Added UUID v6 generated based on current system date and time as well as local Network MAC Address.
- Added UUID v6 generated based on current system date and time as well as supplied Network MAC Address.
- Added UUID v6 generated based on supplied system date and time as well as local Network MAC Address.
- Added UUID v6 generated based on supplied system date and time as well as supplied Network MAC Address.
- Added UUID v7 generated based on current system date and time as well as generated random fields.
- Added UUID v7 generated based on supplied system date and time as well as generated random fields.
- Added UUID v7 generated based on supplied system date and time as well as supplied random fields.


## [v1.1.0] - 2022-05-31
[v1.1.0](https://github.com/TensionDev/UUID/releases/tag/v1.1.0)

### Added
- Added support to convert System.Guid to TensionDev.UUID.Uuid and vice-versa.


## [v1.0.0] - 2022-03-26
[v1.0.0](https://github.com/TensionDev/UUID/releases/tag/v1.0.0)

### Added
- Added TensionDev.UUID.Uuid as an implementation based on RFC 4122.
- Added UUID v3 generated based on MD5, System.Security.Cryptography.MD5.
- Added UUID v5 generated based on SHA-1, System.Security.Cryptography.SHA1.

### Removed
- Changed implementation from System.Guid to TensionDev.UUID.Uuid.


## [v0.2.0] - 2021-09-10
[v0.2.0](https://github.com/TensionDev/UUID/releases/tag/v0.2.0)

### Added
- Added UUID v4 generated based on Pseudo Random Number Generator, System.Security.Cryptography.RNGCryptoServiceProvider.


## [v0.1.1] - 2021-09-10
[v0.1.1](https://github.com/TensionDev/UUID/releases/tag/v0.1.1)

### Changed
- Changed Namespace from UUIDUtil to TensionDev.UUID to reflect official prefix for Package Id.


## [v0.1.1-alpha] - 2021-09-08
[v0.1.1-alpha](https://github.com/TensionDev/UUID/releases/tag/v0.1.1-alpha)

### Fixed
- Fixed Variant Field for generated UUID v1 to be of the correct range. (0x8xxx - 0xbxxx)


## [v0.1.0-alpha] - 2021-09-04
[v0.1.0-alpha](https://github.com/TensionDev/UUID/releases/tag/v0.1.0-alpha)

### Added
- Added UUID v1 generated based on current system date and time as well as local Network MAC Address.
- Added UUID v1 generated based on current system date and time as well as supplied Network MAC Address.
- Added UUID v1 generated based on supplied system date and time as well as local Network MAC Address.
- Added UUID v1 generated based on supplied system date and time as well as supplied Network MAC Address.
