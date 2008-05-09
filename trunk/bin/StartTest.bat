@echo off
setlocal
SET OPTIONS=-run
if EXIST "%1" SET OPTIONS=-runfiles 
cscript/NoLogo "%IEUNIT_HOME%lib\IeTextRunner.wsf" %OPTIONS% "%1" %2 %3 %4 %5 %6 %7 %8
endlocal
