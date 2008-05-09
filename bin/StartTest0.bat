@IF %1==-d (
    cscript/NoLogo /X /D "%IEUNIT_HOME%lib\IeTextRunner.wsf" -d -runfiles %2 %3 %3 %4 %5 %6 %7 %8 %9
) else (
    cscript/NoLogo "%IEUNIT_HOME%lib\IeTextRunner.wsf" -runfiles %1 %2 %3 %3 %4 %5 %6 %7 %8 %9
)
pause
