# NugetXray

[![NuGet](https://img.shields.io/nuget/v/NugetXray.svg?maxAge=2592000)](https://www.nuget.org/packages/NugetXray/) [![Gitter](https://img.shields.io/gitter/room/nwjs/nw.js.svg?maxAge=2592000)](https://gitter.im/NugetXray/Lobby)

NugetXray is a command line tool that analyses your nuget and project references. It contains commands that help you see 
how out of date your packages are, diagnose issues with multiple versions of the same package, and more. 

# Installation

NugetXray itself is a package available from [nuget](https://www.nuget.org/packages/NugetXray/). It has been compiled 
down to a native executable so a dotnet core installation is not needed.

You can also download a zip from the [releases](https://github.com/naeemkhedarun/NugetXray/releases) page.

```
> nuget install NugetXray
```

You can access the executable in the tools folder.

```
> cd .\NugetXray*\tools
> .\NugetXray.exe
```

## Building from source

You can clone this repository and create your own package using the build script.

This application is built with [dotnet core](https://www.microsoft.com/net/core) so you will need
to install the appropriate tools for your platform.

```
> cd build
> .\build.ps1
```

# Commands

## Diff

The diff command lets you see how out of date your packages are. It will read all the package.configs in your solution
and compare them to whats available in the package source. By default it will compare against [nuget.org](http://nuget.org).

```
> .\NugetXray.exe diff -d C:\git\ConveyorBelt\
Scanning https://www.nuget.org/api/v2 for packages.configs.
WindowsAzure.Storage.2.1.0.4                                           | -5.0.0      | 3   configs
Newtonsoft.Json.6.0.8                                                  | -3.0.0      | 4   configs
WindowsAzure.Storage.4.3.0                                             | -3.0.0      | 1   configs
```

You can also get more detailed output by using the verbose switch.

```
> .\NugetXray.exe diff -d C:\git\ConveyorBelt\ -v
Scanning https://www.nuget.org/api/v2 for packages.configs.
WindowsAzure.Storage.2.1.0.4 | -5.0.0
  C:\git\ConveyorBelt\src\ConveyorBelt.ConsoleWorker\packages.config
  C:\git\ConveyorBelt\src\ConveyorBelt.Tooling\packages.config
  C:\git\ConveyorBelt\src\ConveyorBelt.Worker\packages.config

Newtonsoft.Json.6.0.8 | -3.0.0
  C:\git\ConveyorBelt\src\ConveyorBelt.ConsoleWorker\packages.config
  C:\git\ConveyorBelt\src\ConveyorBelt.Tooling\packages.config
  C:\git\ConveyorBelt\src\ConveyorBelt.Worker\packages.config
  C:\git\ConveyorBelt\test\ConveyorBelt.Tooling.Test\packages.config
```

## Duplicate

The duplicate command finds multiple versions of the same package. Multiple versions should be consolidated to a single version 
to avoid runtime issues.

```
> .\NugetXray.exe duplicate -d C:\git\ConveyorBelt\
Scanning C:\git\ConveyorBelt\ for packages.configs.
WindowsAzure.Storage                                                   | 2 versions

Errors:   1
```

You can also get more detailed output by using the verbose switch.

```
> .\NugetXray.exe duplicate -d C:\git\ConveyorBelt\ -v
Scanning C:\git\ConveyorBelt\ for packages.configs.
WindowsAzure.Storage | 2 versions
  (2.1.0.4, C:\git\ConveyorBelt\src\ConveyorBelt.ConsoleWorker\packages.config)
  (2.1.0.4, C:\git\ConveyorBelt\src\ConveyorBelt.Tooling\packages.config)
  (2.1.0.4, C:\git\ConveyorBelt\src\ConveyorBelt.Worker\packages.config)
  (4.3.0, C:\git\ConveyorBelt\test\ConveyorBelt.Tooling.Test\packages.config)

Errors:   1
```

# Issues and feature requests

Please start raise an [issue](https://github.com/naeemkhedarun/NugetXray/issues) on github for any bugs or 
features you would like to see.