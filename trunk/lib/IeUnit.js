/**
 * @file
 * The core of IeUnit test framework, it implements the
 * class IeTestCase and the global function assimilate(). 
 * All test case in the IeUnit framework must assimilate a
 * IeTestCase object.
 */
 
/** An object that combines an IeTestCase and an IeDhtml object. */
function IeUnit() {
    assimilate(this, new IeTestCase());
    assimilate(this, new IeDhtml());
    assimilate(this, new Win32Dom());
};
 

/** 
 *Constructor for the IeTestCase class. 
 */
function IeTestCase() {
    /** 
     *The name of the current test.  
     */
    this.currentTestName = "_";

    /**
     * Asserts that a value must be true. 
     * errMsg is a optional error message.
     */
    this.assertTrue = function assertTrue( ok, errMsg ) {
        if ( ! ok ) {
            var txt = "assertTrue failed";
            if ( errMsg != undefined ) txt += ": " + errMsg;
            throw new IeError(1000, txt);
        }
    };

    /**
     * Asserts that a value must be false.
     * errMsg is a optional error message.
     */
    this.assertFalse = function assertFalse( ok, errMsg ) {
        if ( ok ) {
            var txt = "assertFalse failed";
            if ( errMsg != undefined ) txt += ": " + errMsg;
            throw new IeError(1012, txt);
        }
    };

    /** 
     * Assert that calling a function must cause exception. 
     * If argument err is present the err must be an error 
     * number that matches the exception's error number.
     */
    this.assertMustFail = function assertMustFail(fct, err)  {
        try {
            fct();
        } catch(ex) {
            if (err == undefined) return;
            if ( (ex.number != undefined) && (ex.number == err) )  return;
            throw new IeError(1001, "Wrong exception number raised. Expected: " 
                + err + ", got: " + ex.number);
        }
        throw new IeError(1002, "Expected exception not raised");
    };
    
    /**
     * Force the current test to fail.
     * errMsg is an optional error message.
     */
     this.assertFail = function assertFail( errMsg ) {
        throw new IeError(1010, (errMsg == undefined) ? "Forced failure" : errMsg);
     };

    /** 
     * Assert that a value must equal an expected value. 
     * errMsg is an optional error message.
     */
    this.assertEquals = function assertEquals(v1, v2, errMsg) {
        if ( v1 != v2 ) {
            var txt = "assertEquals failed. Expected: " + v1 + ", got: " + v2;
            if ( errMsg != undefined ) txt += ": " + errMsg;
            throw new IeError(1003, txt);
        }
    };

    /** 
     * Assert that text contains another string.
     * errMsg is an optional error message.
     */
    this.assertContains = function assertContains(v1, v2, errMsg) {
        if ( v1.indexOf( v2 ) < 0 ) {
            var txt = "assertContains failed: '" + v1 + "' doesn't contain '" + v2 +"'";
            if ( errMsg != undefined ) txt += ": " + errMsg;            
            throw new IeError(1008, txt);
        }
    };

    /**
     * Assert that a value is not null.
     * errMsg is an optional error message.
     */
    this.assertNotNull = function assertNotNull(v, errMsg){
        if ( v == null ) {
            var txt = "assertNotNull failed";
            if ( errMsg != undefined ) txt += ": " + errMsg;            
            throw new IeError(1006, txt);
        }
    };

    /**
     * Assert that a value is null.
     * errMsg is an optional error message.
     */
    this.assertNull = function assertNull(v, errMsg){
        if ( v != null ) {
            var txt = "assertNull failed";
            if ( errMsg != undefined ) txt += ": " + errMsg;
            throw new IeError(1007, txt);
        }
    };
    
    /**
     * Wait until a function call has successed.
     *
     * This method repeatedly call a function till it has succeeded or timeouted.
     * The parameter fct is the function to be called.
     * A function call is considered success if:
     *  -# It returns without return-value and doesn't through an exception.
     *  -# It returns the boolean value 'true'.
     *  -# It returns an object that is not null and not 'false'.
     * 
     * Examples:
     *  -# this.waitForSuccess( function() { this.assertPageHasText("OK"); } );
     *  -# this.waitForSuccess( function() { return (this.findByText("OK") != null); } );
     *  -# this.waitForSuccess( function() { return this.findByText("OK"); } );
     *
     * Notice: this method is intended to deal with self-refreshing pages. Normally, IeUnit's
     * assert methods check the page's content only once after the page entered "complete"
     * state. waitForSuccess() continouesly checks the page's content
     * regardless of the page's state until the desired content become present or timed out.
     *
     */
    this.waitForSuccess = function waitForSuccess(fct) {
        var startTime = (new Date()).getTime();

        while(true) {
            this._sleep(200);
            try {
                // make sure that fct is called in the context of calling object
                // we can't simple call fct(), since that will be called in the
                // context of static this object which is IeTestCase.
                this._tmpFctName = fct;
                var ret = this._tmpFctName();
                var ret_type = typeof(ret);

                if ( ret_type == "undefined" ) {
                    return; //empty return;
                } else if ( ret_type == "boolean" ) {
                    if ( ret == true ) return; // returned 'true'
                } else if ( ret_type == "object" ) {
                    if ( ret != null ) return;
                }
            } catch (e) { };
            var timeNow =  (new Date()).getTime();
            if ( (timeNow-startTime) > this.findTimeout ) {
                throw new Error(1102, "waitForSuccess call timed out");
            }  
        }
        return this;
    };


    /**
     * Log a message on the console.
     */
    this.log = function log( msg ) {
        WScript.Echo(msg);
    };

    /**
     * Suspend the calling thread for given number of milliseconds.
     */
    this.sleep = function sleep( milliseconds ) {
        WScript.Sleep(milliseconds);
        return this;
    };


    /**
     * Return the list of test names. All methods whose name begins with 'test' 
     * are considered test methods.
     */
    this.getAllTests = function getAllTests() {
        var testList = new Array();
        for (var memberName in this ) {
            if ( memberName.match(/^test/) ) {
                testList.push(memberName);
            }
        }
        return testList;
    };
        
    /**
     * Fixture callback method.
     * This method is called before each test.
     */
    this.setUp    = function setup() {};

    /**
     * Fixture callback method.
     * This method is called after each test.
     */
    this.tearDown = function tearDown() {};


    /**
     * Fixture callback method.
     * this method is called before each test case.
     */
    this.setUpCase = function setupCase(){};

    /**
     * Fixture callback method.
     * This method is called after each test case.
     */
    this.tearDownCase = function tearDownCase(){};


    /**
     * Run a test with given name.
     */
    this.runTest = function runTest(testName) {
        if ( this[testName] == undefined ) {
            return new TestResult(testName, new IeError(1004, "Test " +testName+ " not found"), 0);
        }
        var exp = null;
        var duration=0;
        try {
            var tBegin = new Date();
            this.currentTestName = testName;
            if ( _isDebugging ) debugger;  
            this.setUp();     // setup the test.
            this[testName](); // invoke the test.
            this.tearDown();  // tear down the test.
            var tEnd = new Date();
            duration = tEnd.getTime() - tBegin.getTime();
        } catch (e) {
            tEnd = new Date();
            duration = tEnd.getTime() - tBegin.getTime();
            exp = e;
            try { this.tearDown(); } catch(e) {}
        }
        return new IeTestResult(testName, exp, duration);
    };

    /**
     * Return the constructor name of an object.
     */
    this.getCaseName = function getCaseName() {
        var re  = new RegExp("function (\\w+)",  "");
        re.exec( this.constructor );
        return RegExp.$1;
    };



    /**
     * Return the directory of the source of the test case
     * This method only works when started under IeTextRunner or IeGuiRunner.
     */
    this.getCaseDir = function getCaseDir() {
        var p = _orgSrc;
        if ( p == null ) {            
            p = getCasePath(this.getCaseName());
        }
        return  p.substring(0, p.lastIndexOf("\\"));
    };

    /**
     * Start a program in the background without waiting for it to finish.
     */
    this.startProgram = function startProgram(filePath) {
        var cmdShell = new ActiveXObject("WScript.Shell");
        cmdShell.Run(filePath, 0, false);
    }

    /**
     * Span a background process that executs a SBK script.
     * The argument must be valid sbk script wrapped into a string.
     */
    this.spanSbkProcess = function spanSbkProcess(sbkScript) {
        var fso = new ActiveXObject("Scripting.FileSystemObject");
        var tfolder = fso.GetSpecialFolder(2);
        var tpath = tfolder.Path +"\\"+fso.GetTempName() + ".sbk";
        var tfile = fso.CreateTextFile(tpath, true);

        tfile.Write(sbkScript);
        tfile.Close();

        this.startProgram(tpath);
    };

} // end of IeTestCase

/** 
 * Construct an IeTestResult object. 
 * An IeTestResult object contains the result of a single test. 
 * The test is assumed to be successful if the member exception is null.
 */
function IeTestResult(testName, exception, duration) {
    this.testName  = testName;
    this.exception = exception;
    this.duration  = duration;
}


/**
 * Assimilates an object.
 * This function lets a destination object (dstObj) assimilate a source
 * object (srcObj). If the parameter suffix is provided this suffix will be appended
 * to all new members moved from srcObj to dstObj. The policy parameter specifies how
 * to resolve name conflicts. Policy is interpreted as follows: 
 * -# : Reject the conflict by throwing exception.
 * -# : Override the member in dstObj.
 * -# : silently ignore the conflicting members in srcObj.
 */
function assimilate(dstObj, srcObj, suffix, policy) {
    if ( suffix == undefined ) suffix = "";
    if ( policy == undefined ) policy = 0;

    function getFunctionName(obj) {
        var re  = new RegExp("function (\\w+)",  "");
        re.exec( obj.constructor );
        return RegExp.$1;
    };

    for (var key in srcObj) {
        dstKey = key + suffix;
        if ( dstObj[dstKey] == undefined ) {
            dstObj[dstKey] = srcObj[key];
        } else {
            // we have member name collision
            switch(policy) {
            case 1: // override existing member.
                dstObj[dstKey] = srcObj[key];
                break;
            case 2: // keep the existing member.
                break;

            case 0: // reject the assimilation
            default:
                throw new IeError(1005, "Assimilation collision between " 
                  + getFunctionName(dstObj)  + " and " + getFunctionName(srcObj) 
                  +" on member " + key);
            }
        }
    }
    return dstObj;
}

