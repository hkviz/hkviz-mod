#!/usr/bin/env zsh
set -euo pipefail
cd "$(dirname "$0")/.."
dotnet build HKViz.sln -c Hollow

