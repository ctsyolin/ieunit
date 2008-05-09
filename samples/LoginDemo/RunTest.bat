REM Notice: test in this directory only works when the login page 
REM is properly installed on the web server. The URL for the login page 
REM is: http://ieunit.sourceforge.net/samples/LoginDemo/Login.html
REM
Rem commands to copy the test files to the server.
Rem scp Login.pl jamesxli@ieunit.sourceforge.net:/home/groups/i/ie/ieunit/cgi-bin
Rem scp Login.html jamesxli@ieunit.sourceforge.net:/home/groups/i/ie/ieunit/samples/LoginDemo
Rem
Rem cscript ..\..\lib\SmartBookmark.wsf LoginBookmark.sbk
cscript.exe ..\..\lib\IeTextRunner.wsf -run
