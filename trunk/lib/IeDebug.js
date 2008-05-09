/**
 * @file
 * Implements functions for debugging.
 */

/**
 * Print the current program stack. If the returnFlag is set to any value, this
 *  method won't print out the stack but just return it as a string.
 *  @see printVariable
 */
function printStackTrace(returnFlag ) {
    var str = "";
    var re  = new RegExp("^function\\s+(\\w+)|^function\(\)",  "");
    var level = 0;
    for(var c = printStackTrace.caller; c!=null; c=c.caller) {
        re.exec( c );
        str += level + ": " + RegExp.$1 + printVariable(c.arguments, true) +"\n"; 
        level++;
    }
    if ( returnFlag == undefined ) {
        WScript.Echo(str);
    } else {
        return str;
    }
};

/**
 * Construct an error object with stack trace.
 */
function IeError(errNo, desc) {
    this.stackTrace = printStackTrace(true);
    if ( errNo != undefined) {
        this.number = errNo;
    }
    if ( desc != undefined ) {
        this.description = desc;
    }
};

/**
 * Print the variable of type object, function, string, or numeric values. 
 * If the returnFlag is set to any value, this method just return the object's string
 * representation without printing it out.
 * @see printStackTrace
 */
function printVariable(obj, returnFlag) {
    var str = "";
    var otype = typeof(obj);
    switch( otype ) {
    case "object":
        if ( obj.callee != undefined ) {
            // the object is Arguments object which is not an array
            str += "(";
            for(var i=0; i<obj.length; i++) {
                if ( typeof(obj[i]) == "string" ) {
                    str += "'" + obj[i] + "',";
                } else {
                    str += obj[i] + ",";
                }
            }
            if(str.charAt(str.length-1) == ',') { str = str.substr(0, str.length-1); }
            str += ")";
        } else {
            for (var key in obj) {
                str += key + ": " + obj[key] + "\n"; 
            }
        }
        break;
    
    default:
        str += otype + ":" + obj;
    }

    if ( returnFlag == undefined ) {
        WScript.Echo(str);
    } else {
        return str;
    }
};

