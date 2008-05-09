@REM Script to open smart bookmark from windows desktop
@REM This script is associated with the *.sbk files.
@IF %1==-d (
    cscript/NoLogo /X /D "%IEUNIT_HOME%lib\SmartBookmark.wsf" -d %2 %3 %4 %5 %6 %7 %8 %9
) ELSE (
    cscript/NoLogo "%IEUNIT_HOME%lib\SmartBookmark.wsf" %1 %2 %3 %4 %5 %6 %7 %8 %9
)
