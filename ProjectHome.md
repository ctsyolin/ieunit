# IeUnit #
version 2.3

IeUnit is a simple framework to test logical behaviors of web pages. It helps software engineers to create, organize and execute functional unit tests.

The architecture of IeUnit is based on the xUnit framework that has been adapted to various languages and platforms. IeUnit is implemented in JavaScript language and tailored to Windows platform. By leveraging services provided by Internet Explorer browser, its DHTML model and related COM technologies IeUnit offers a simple and efficient test framework.

Main features of IeUnit are:
  * IeUnit scripts test directly against real browser with real JavaScript engine, and thus provide higher confidence than other frameworks which test against simulated (and complex) browsers and JavaScript engines.
  * IeUnit test scripts are in JavaScript which is widely used and well documented. There is no need to learn proprietary scripting languages.
  * IeUnit offers a very simple installation procedure. There is no need to install other libraries or programs like local proxy server. If you know JavaScript and DHTML you can install IeUnit and start coding test cases within minutes.
  * IeUnit does not need internal access to the web server. You can use it to test any remote web site.
  * IeUnit allows extension in an object oriented way.  You can integrate your own test classes or test suites by dropping in you code into pre configured directories.
  * IeUnit provides a script helper, QuickFocus, that enables users to inspect web pages and generate script code with mouse clicks.

The following is a simple test case with IeUnit framework:

```
function HelloWorldTest() {
    assimilate(this, new IeUnit());

    this.setUp = function() {
        this.openWindow("http://localhost/HelloWorld.html");
    };

    this.tearDown = function() {
        this.closeWindow();
    };

    // Verify that the page contains only the text "Hello World!"
    this.testCheckText = function() {
        this.assertPageHasText("Hello World!");
    };

    // Verify that the style of the text is the HTML element H1
    this.testCheckStyle = function() {
        this.assertEquals("H1", this.findByText("Hello").tagName);
    };
}
```

The following shows the execution of above test case:

```
C:\Program Files\IeUnit\samples\HelloWorld>StartTest HelloWorldTest.jst

Running case HelloWorldTest ...
RptTest: testCheckText:                 OK
RptTest: testCheckStyle:                OK
RptCase: HelloWorldTest:                successes: 2, failures: 0
RptSuite: Total Duration: 4.375sec,          Successes: 2, Failures: 0
```

## Online Demo ##

The integrated GUI script helper QuickFocus significantly reduces the learning curve and simplifies the script development process. The following flash demos show how QuickFocus works:

  * [Creating a smart bookmark for searching keywords on google](http://ieunit.sourceforge.net/FlashDemo/IeUnitSmartbookmarker.htm).
  * [Creating a unit test with QuickFocus](http://ieunit.sourceforge.net/FlashDemo/IeUnitTest.htm).

## Documentation ##
  * [Getting Started](http://ieunit.sourceforge.net/GettingStarted.html)
  * [User Guide](http://ieunit.sourceforge.net/TestFramework.html)
  * [FAQ](http://ieunit.sourceforge.net/FAQ.html)
  * [Script API](http://ieunit.sourceforge.net/ApiDoc/files.html)