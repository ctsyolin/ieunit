#!/usr/bin/perl
use CGI qw/:standard/;
print header,start_html(-title=>'Upload File', -bgcolor=>'#ccccff');
print "File Uploaded";
print end_html;
