setlocal 
set FN=Win32Dom.zip
del %FN%
zip %FN% Readme.txt
cd Debug
zip ..\%FN% Win32Dom.dll 
cd ..\ApiTest
zip ../%FN% Win32Dom.js *.jst
cd ..
