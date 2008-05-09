Overview
--------

This package demonstrates how to extended 
IeUnit with additional third-party module.

This package provides a solution for the problem
that some website requries basic HTTP authentication
which cause the IE to prompt for user name and
password through a non-html window. IeUnit current
doesn't support interfaction with those non-html
window. This package uses a ActiveX object named 
Win32Dom.dll that allows JavaScript scripts to drive any
native window on the Windows desktop.

How to install this package?
----------------------------

1. Download and install Microsoft .Net Runtime 1.1 and SDK from
   here: http://www.microsoft.com/downloads/, if you not already
   have instaled it.

2. Install IeUnit version 1.4.2 or latter, if you not already
   have installed it.

3. Unpack the zip file of this package into any directory.

3. Copy the file Win32Dom.js to the directory "%IEUNIT_HOME%\lib".

4. Intall the the ActiveX object Win32Dom.dll with the following
   two commands:
        regasm.exe Win32Dom.dll
        gacutil.exe -i Win32Dom.dll

   Both command regasm.exe and gacutil.exe are available from the
   .Net library.


How to uninstall Win32Dom.dll?
------------------------------

Issue the following two commands:

    gacutil.exe -u Win32Dom
    regasm /unregister Win32Dom.dll


How to test this package?
-------------------------

Click on the IeUnit->"Create Tests" from the windows start 
menu to open prompt window. Change into the directory where 
you unpackage this package. Then start the two unit test 
with the following command:

    StartTest.bat

