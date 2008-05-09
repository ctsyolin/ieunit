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
    if ( $name eq "IeUnit" ) {
	print hr;
	open(LOG, ">> LoginDemo.log");
	print LOG "Name: $name\n";
	close(LOG);
    } 
} else {
    print "<font color='red'><center>", h2('Invalid User Name or Password'), "</center></font>", hr;
    print "<a href='/samples/LoginDemo/Login.html'>Return to Login Page</a>";
}
print "</center>";
print end_html;
