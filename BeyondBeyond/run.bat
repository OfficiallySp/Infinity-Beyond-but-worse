@echo off
REM run the cheat (windows)
REM
REM   run.bat --step          RECOMMENDED. pauses at every act and every cheat
REM                           so you can actually read it. press enter to advance.
REM   run.bat                 straight through, slow (1.9x). ~7.5 min.
REM   run.bat --speed 4       n = multiplier. 4 is a hostage situation.
REM   run.bat --fast          no delays at all, for cowards
REM   run.bat --no-color      if your terminal shows escape codes instead of colour
REM
REM chcp 65001 switches the console to UTF-8 so the emoji render instead of
REM turning into "?". the default codepage is 437, designed in 1981, which
REM does not contain a clown emoji. an oversight.
chcp 65001 >nul 2>&1
setlocal
cd /d "%~dp0"
dotnet run --project BeyondBeyond.csproj -- %*
endlocal
