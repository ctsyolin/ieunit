// This script filters JavaScript files so that
// doxygen produces better results.
//
// Usage: cscript.exe JsFilter.js <SrcJsFile>
//
var fso = new ActiveXObject("Scripting.FileSystemObject");
var f   = fso.GetFile(WScript.Arguments(0));
var fs  = f.OpenAsTextStream(1);

var methodReg = new RegExp("^function (.*) {$", "");

while( !fs.AtEndOfStream ) {
    var line = fs.ReadLine();
    if ( methodReg.exec( line ) ) {
       WScript.Echo("function " + RegExp.$1 + "{}//DoxygenFix");
    } else {
        WScript.Echo(line);
    }
}
fs.Close();

