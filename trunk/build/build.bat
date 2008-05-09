pushd ..\doc
call BuildApiRef.bat
popd
"C:\Program Files\Microsoft Visual Studio .NET 2003\Common7\IDE\devenv.exe" build.sln  /build Debug /project build  /projectconfig Debug
