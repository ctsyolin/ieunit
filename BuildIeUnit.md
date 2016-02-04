System Requirements to build IeUnit:
  * Visual Studio C#.NET (2003).
  * Microsoft HTML Help Workshop.
  * Doxygen version 1.3.9.1 or higher


Do the following to build IeUnit:
  * Checkout the source tree:
```
    svn checkout https://ieunit.googlecode.com/svn/trunk/ ieunit --username {your-user-name}

    Or for read-only checkout:

    svn checkout http ://ieunit.googlecode.com/svn/trunk/ ieunit
```
  * Chdir into ieunit directory, and start Visual Studio C#.NEt and load the soluation IeUnit.sln.
  * In Visual Studio, select the menu build>build solution to the solution. After a short while the install program will be built and stored under the path ieunit/build/IeUnit.msi.

Command to checkout a subdirectory:
```
    svn checkout https://ieunit.googlecode.com/svn/trunk/dirName1/dirName2 localDirName --username {your-user-name}
```