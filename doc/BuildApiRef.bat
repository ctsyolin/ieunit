c:\app\doxygen\doxygen.exe 
for %%f in ( "ApiDoc\*-source.html" ) do cscript PostFilter.js %%f
