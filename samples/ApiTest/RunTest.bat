echo off
if  "%1"=="gui" cscript/NoLogo ..\..\lib\IeGuiRunner.wsf
if not "%1"=="gui" cscript/NoLogo ..\..\lib\IeTextRunner.wsf -run %1
