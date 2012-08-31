REM Register Win32Dom.dll as an 64bit DOM object so that it can be 
REM loaded on 64bit systems.
REM The default setup installation only register it as an 32 bit COM object
REM
if not exist "%ProgramFiles(x86)%" goto :eof
if not exist %windir%\Microsoft.NET\Framework64\v2.0.50727\regasm.exe goto :eof
%windir%\Microsoft.NET\Framework64\v2.0.50727\regasm.exe /codebase "%ProgramFiles(x86)%\IeUnit\bin\Win32Dom.dll"