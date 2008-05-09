#!/usr/bin/perl
use CGI qw/:standard/;
print header,start_html(-title=>'Start Page', -bgcolor=>'#ccccff');

$pwds{'IeUnit'} = 'DemoPwd';
$pwds{'James'}  = 'AdminPwd';
$name = param('UserName');
$pwd =  param('Password');

print "<center>";
if ( defined($pwds{$name}) && ($pwd eq $pwds{$name}) ) {
    print h2('Hi! '. param('UserName')), hr;
    print "<a href='/samples/LoginDemo/Login.html'>Logout</a>";
} else {
    print "<font color='red'><center>", h2('Invalid User Name or Password'), "</center></font>", hr;
    print "<a href='/samples/LoginDemo/Login.html'>Return to Login Page</a>";
}
print "</center>";
