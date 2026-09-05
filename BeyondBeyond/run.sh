#!/usr/bin/env bash
# run the cheat 🚀
# usage: ./run.sh          <- the full experience (recommended, ~90 seconds of pain)
#        ./run.sh --fast   <- for cowards
#
# NOTE: this script does not check whether dotnet is installed.
# if it isnt, you'll find out. 💀
set -u
cd "$(dirname "$0")"
exec dotnet run --project BeyondBeyond.csproj -- "$@"
