setlocal
echo on

for %%f in ( IeUnit_*.msi ) do set MSI_FN=%%f

REM the last field is the component id of the TARGET 
REM directory declared in Component table.

Cscript WiRunSQL.vbs %MSI_FN% "INSERT INTO `Environment` (`Environment`,`Name`,`Value`,`Component_`) VALUES ('IEUNIT_HOMEpkey1', 'IEUNIT_HOME','[TARGETDIR]', 'C__E84E124545C44D30B635D2A787F97E17')" 

Cscript WiRunSQL.vbs %MSI_FN% "UPDATE `Shortcut` SET `Target`='[TARGETDIR]bin\SendTo.bat', `Icon_`='' WHERE `Name`='IEUNIT|IeUnit'"

endlocal
