@echo off
REM File: SendTo.bat
REM Description: called by Send-To menu to run a set of selected jst files
REM
for %%I in (%1) do cd %%~dI%%~pI
cscript/NoLogo "%IEUNIT_HOME%lib\IeTextRunner.wsf" -runfiles %*
pause
