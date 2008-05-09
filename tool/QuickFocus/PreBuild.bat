setlocal
cd "%1"
call "C:\Program Files\Microsoft Visual Studio 8\VC\vcvarsall.bat" x86
IF NOT EXIST AxSHDocVw.dll aximp /keyfile:QuickFocus.snk c:\Windows\system32\shdocvw.dll 
endlocal
