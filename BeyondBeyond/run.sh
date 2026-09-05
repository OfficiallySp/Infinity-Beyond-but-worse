#!/usr/bin/env bash
# run the cheat 🚀
#
#   ./run.sh                 the intended experience. slow, because you asked. 🐌
#   ./run.sh --normal        the old speed (1.0x) — too fast to read the good bits
#   ./run.sh --speed 3       n = multiplier. 3 is a hostage situation.
#   ./run.sh --speed 0.5     brisk
#   ./run.sh --fast          no delays at all, for cowards
#
# --slow still works. it sets the default to the default. we kept it so nobody's
# muscle memory breaks. 🫡
#
# NOTE: this script does not check whether dotnet is installed.
# if it isnt, you'll find out. 💀
set -u
cd "$(dirname "$0")"
exec dotnet run --project BeyondBeyond.csproj -- "$@"
