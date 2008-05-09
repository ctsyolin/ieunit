c:\app\doxygen\bin\doxygen.exe 
for %%f in ( "ApiDoc\*-source.html" ) do cscript PostFilter.js %%f
