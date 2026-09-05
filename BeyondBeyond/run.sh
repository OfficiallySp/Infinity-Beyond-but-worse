#!/usr/bin/env bash
# run the cheat 🚀
#
#   ./run.sh --step          RECOMMENDED 🪜 pauses at every act and every cheat
#                            so you can actually read it. press enter to advance.
#   ./run.sh                 straight through, slow (2.0x). ~8 min.
#   ./run.sh --speed 4       n = multiplier. 4 is a hostage situation.
#   ./run.sh --normal        1.0x. too fast to read. we tried it. thats why
#                            --step exists.
#   ./run.sh --fast          no delays at all, for cowards
#
# it is ~18,000 words. thats an hour of real reading. --step is not a gimmick,
# it is the only way anyone has finished it 📖
#
# --slow still works. it sets the default to the default. kept so nobody's
# muscle memory breaks 🫡
#
# NOTE: this script does not check whether dotnet is installed.
# if it isnt, you'll find out. 💀
set -u
cd "$(dirname "$0")"
exec dotnet run --project BeyondBeyond.csproj -- "$@"
