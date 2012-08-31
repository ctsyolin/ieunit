if not exist "%ProgramFiles(x86)%" goto :eof
if not exist %windir%\Microsoft.NET\Framework64\v2.0.50727\regasm.exe goto :eof
%windir%\Microsoft.NET\Framework64\v2.0.50727\regasm.exe /unregister "%ProgramFiles(x86)%\IeUnit\bin\Win32Dom.dll"