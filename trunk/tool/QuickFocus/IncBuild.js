// This script increments the build number of the AssebmlyVersion attribute.
// Usage:
//
//   cscript/nologo IncBuild.js AssemblyInfo.cs
//
var fso = new ActiveXObject("Scripting.FileSystemObject");
var f   = fso.GetFile(WScript.Arguments(0));
var fs  = f.OpenAsTextStream(1);
var newContent = "";

var methodReg = /(.*AssemblyVersion.*\d+\.\d+\.)(\d+)(.*)/;

while( !fs.AtEndOfStream ) {
    var line = fs.ReadLine();
    if ( methodReg.exec( line ) ) {
        var newNum = RegExp.$2  - 0 + 1;
        newContent += RegExp.$1 + newNum + RegExp.$3 +"\n";
    } else {
        newContent += line + "\n";
    }
}
fs.Close();

fs = f.OpenAsTextStream(2);
fs.Write(newContent);
fs.Close();
