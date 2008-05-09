@echo off
setlocal
SET NAME=%1
SET NAME=%NAME:~-4%
IF %NAME%==.sbk GOTO SmartBookmark

SET OPTIONS=-run
IF EXIST "%1" SET OPTIONS=-runfiles
cscript /NoLogo /X /D "%IEUNIT_HOME%lib\IeTextRunner.wsf" -d %OPTIONS% "%1"
goto :EOF

:SmartBookmark
cscript /NoLogo /X /D "%IEUNIT_HOME%lib\SmartBookmark.wsf" -d "%1"

endlocal
