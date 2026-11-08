# TensionDev.UUID

[![.NET](https://github.com/TensionDev/UUIDUtil/actions/workflows/dotnet.yml/badge.svg)](https://github.com/TensionDev/UUIDUtil/actions/workflows/dotnet.yml)
[![Package Release](https://github.com/TensionDev/UUIDUtil/actions/workflows/package-release.yml/badge.svg)](https://github.com/TensionDev/UUIDUtil/actions/workflows/package-release.yml)
[![CodeQL](https://github.com/TensionDev/UUIDUtil/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/TensionDev/UUIDUtil/actions/workflows/github-code-scanning/codeql)

TensionDev.UUID is a .NET library for working with Universally Unique Identifiers (UUIDs). 
This project references the following documents for implementation.  
- [Universally unique identifier - Wikipedia](https://en.wikipedia.org/wiki/Universally_unique_identifier)
- [MySQL :: MySQL 8.0 Reference Manual :: 12.24 Miscellaneous Functions](https://dev.mysql.com/doc/refman/8.0/en/miscellaneous-functions.html#function_uuid)
- [rfc4122](https://datatracker.ietf.org/doc/html/rfc4122)
- [rfc9562](https://datatracker.ietf.org/doc/html/rfc9562)

---

## Features  

- Support for multiple UUID versions:  
  - v1 (date-time and MAC address)  
  - v3 (MD5 namespace name-based)  
  - v4 (random)  
  - v5 (SHA-1 namespace name-based)  
  - v6 (date-time-ordered)  
  - v7 (Unix time + randomness)  
- Converters:  
  - `ToGuid()` and `FromGuid(Guid)`  
  - `ToByteArray()` and `FromBytes(byte[])`  
- Parsing utilities:  
  - `Parse(string)`  
  - `TryParse(string, out Uuid)`  
- Formatting options:  
  - `N`, `D`, `B`, `P` (same as System.Guid)  
- Comparison operators:  
  - `==`, `!=`, `<`, `>`, `<=`, `>=`  
- Equality and hashing for dictionary/set usage  
- Strict RFC compliance for parsing and validation  

---

## Installation
```
dotnet add package TensionDev.UUID
```

---

## Usage Examples

### Generate UUID v1 (date-time and MAC address)
```csharp
using TensionDev.UUID;

Uuid uuid = UUIDv1.NewUUIDv1();
Console.WriteLine(uuid); // Example: 164a714c-0c79-11ec-82a8-0242ac130003
```

### Generate UUID v4 (random)
```csharp
using TensionDev.UUID;

Uuid uuid = UUIDv4.NewUUIDv4();
Console.WriteLine(uuid); // Example: 550e8400-e29b-41d4-a716-446655440000
```

### Generate UUID v5 (SHA-1 namespace name-based)
```csharp
using TensionDev.UUID;

Uuid uuid = UUIDv5.NewUUIDv5(UUIDNamespace.URL, "https://www.contoso.com");
Console.WriteLine(uuid); // Example: 1bf6935b-49e6-54cf-a9c8-51fb21c41b46
```

### Parse and validate
```csharp
using TensionDev.UUID;

bool isValid = Uuid.TryParse("550e8400-e29b-41d4-a716-446655440000", out var parsed);
```

### Byte Conversions
```csharp
using TensionDev.UUID;

byte[] bytes = uuid.ToByteArray();
Uuid fromBytes = Uuid.FromBytes(bytes);
```

### String Formatting
```csharp
using TensionDev.UUID;

Console.WriteLine(uuid.ToString("N")); // 32 hex digits
Console.WriteLine(uuid.ToString("D")); // canonical form
Console.WriteLine(uuid.ToString("B")); // with braces
Console.WriteLine(uuid.ToString("P")); // with parentheses
```

