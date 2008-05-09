@REM This script runs all tests within this driectory and subdirectories
@REM
@echo off
(for %%d in ( ApiTest, RunTestCase, HelloWorld, LoginDemo, Win32DomTest) do cd %%d & RunTest.bat  & cd .. ) | findstr "Rpt Running"
@pause
