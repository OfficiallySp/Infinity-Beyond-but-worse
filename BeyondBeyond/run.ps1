#!/usr/bin/env pwsh
# run the cheat 🚀 (windows / mac / linux - powershell works everywhere now)
#
#   ./run.ps1 --step          RECOMMENDED 🪜 pauses at every act and every cheat
#   ./run.ps1                 straight through, slow (1.9x). ~7.5 min.
#   ./run.ps1 --speed 4       n = multiplier. 4 is a hostage situation.
#   ./run.ps1 --fast          no delays at all, for cowards
#   ./run.ps1 --no-color      if you see escape codes instead of colour
#
# it is ~18,000 words. thats an hour of real reading. --step is not a gimmick,
# it is the only way anyone has finished it 📖

$ErrorActionPreference = 'Stop'
Set-Location -LiteralPath $PSScriptRoot

# we do not check whether dotnet is installed 💀 you will find out
dotnet run --project BeyondBeyond.csproj -- @args
