<h1 align="center"> 🔧 Multi Tekla 🪛</h1>

MultiTekla is like a multi-tool, but for Tekla. It utilizes the internal API of Tekla Structures to access and manipulate models in a headless mode.

MultiTekla is in alpha and may contain bugs and missing features. APIs, commands and plugins can be subject to change in any subsequent commit.

## How to build

Make sure .NET 8.0 and .NET Framework 4.8 are intsalled.

- clone MultiTekla repository using git
- cd in directory with cloned project
- Run `dotnet restore`
- Run `dotnet build -f net48 -c Debug --no-restore`

Build output would be in `src/MultiTekla.CLI/bin/Debug/net48`.

## How to develop your plugin for MultiTekla

1. Create project for your plugin
