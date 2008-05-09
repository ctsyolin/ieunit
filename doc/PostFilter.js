// This script post-filters results produced by doxygen
//
// Usage: cscript.exe PostFilter.js <DoxygenHTMLFile>
//
var fso = new ActiveXObject("Scripting.FileSystemObject");
var fname = WScript.Arguments(0);
fso
var f   = fso.GetFile(WScript.Arguments(0));
var tmpFname = "TempFilterFile.html";
var tmpF = fso.CreateTextFile(tmpFname, true);
var fs  = f.OpenAsTextStream(1);


var methodReg = new RegExp("\}<span class=\"comment\">\/\/DoxygenFix<\/span>", "");

while( !fs.AtEndOfStream ) {
    var line = fs.ReadLine();
    tmpF.WriteLine(line.replace("\}<span class=\"comment\">\/\/DoxygenFix<\/span>", ""));
}

tmpF.Close();
fs.Close();
fso.DeleteFile(fname);
fso.MoveFile(tmpFname, fname);



