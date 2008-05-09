REM this script installs the debug version
REM of Win32Dom built by Visual Studio for debugging purpose.
REM
cd debug

gacutil.exe -u Win32Dom
regasm /unregister Win32Dom.dll

regasm.exe Win32Dom.dll
gacutil.exe -i Win32Dom.dll

cd ..
