@echo off
REM run the cheat, on windows 🚀
REM
REM   run.bat --step          RECOMMENDED 🪜 pauses at every act and every cheat
REM   run.bat                 straight through, slow (1.9x). ~7.5 min.
REM   run.bat --speed 4       n = multiplier. 4 is a hostage situation.
REM   run.bat --normal        1.0x. too fast to read.
REM   run.bat --fast          no delays at all, for cowards
REM
REM same as run.sh. also does not check whether dotnet is installed. 💀
cd /d "%~dp0"
dotnet run --project BeyondBeyond.csproj -- %*
