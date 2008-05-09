/**
 * Open the configuration panel of a given application.
 * 'appCpl' is the Control panel file for the particular application. 
 * 'title' is the window title of the configuration panel.
 *  Notice: the function ListProcess() can be used to find the name of the
 *          application control panel file.
 */
function OpenControlPanel(appCpl, title) {
    var shell  = WScript.CreateObject("WScript.Shell");
    var root   = shell.Environment("Process")("SystemRoot");
    var root32 = root + "\\system32\\";
    shell.Run( 
        root32 + "rundll32.exe " 
        + root32 + "shell32.dll,Control_RunDLL " 
        + root32 + appCpl, 1, false);
    return _.waitForWindow(title);
}

/**
 * Clean all information cached by Internet Explorer, e.g. cookies, file caches and 
 * history list.
 */
function CleanIeCaches() {
    var ieCfg = OpenControlPanel("inetcpl.cpl", "Internet Properties");

    // Clean the cookies
    _.findWinButton(ieCfg, "Delete Cook&ies...").click();
    prompt = _.waitForWindow("Delete Cookies", 5000);
    _.sleep(500);
    _.findWinButton(prompt, "OK").click();
    _.sleep(1000);

    // Clear the history list.
    _.findWinButton(ieCfg, "Clear &History").click();
    prompt = _.waitForWindow("Internet Options", 5000);
    _.sleep(500);
    _.findWinButton(prompt, "&Yes").click();
    _.sleep(1000);


    // Clean the cache
    _.findWinButton(ieCfg, "Delete &Files...").click();
    prompt = _.waitForWindow("Delete Files", 5000);
    _.findWinButton(prompt, "&Delete all offline content").click();
    _.sleep(500);
    _.findWinButton(prompt, "OK").click();
    _.sleep(1000);

    // Close the ieCfg window.
    _.findWinButton(ieCfg, "OK").click();
}

/**
 * Clean all information cached by Internet Explorer version 7.0, 
 * e.g. cookies, file caches and history list.
 */
function CleanIe70Caches() {
    var ieCfg = OpenControlPanel("inetcpl.cpl", "Internet Properties");

    _.findWinButton(ieCfg, "&Delete...").click();
    prompt = _.waitForWindow("Delete Browsing History", 5000);

    _.findWinButton(prompt, "Delete c&ookies...").click();
    prompt = _.waitForWindow("Delete Cookies", 5000);
    _.sleep(500);
    _.findWinButton(prompt, "&Yes").click();
    _.sleep(1000);

    prompt = _.waitForWindow("Delete Browsing History", 5000);
    _.sleep(500);
    _.findWinButton(prompt, "Delete &all...").click();
    _.sleep(500);
    prompt = _.waitForWindow("Delete Browsing History", 5000);
    _.sleep(500);
    _.findWinButton(prompt, "Also &delete files and settings stored by add-ons.").click();
    _.sleep(500);
    _.findWinButton(prompt, "&Yes").click();
    _.sleep(500);
    _.findWinButton(ieCfg, "OK").click();
}

/** 
 * List the currently running processes.
 */
function ListProcess() {
    var e = new Enumerator(GetObject("winmgmts:").InstancesOf("Win32_process"))
    var columns = new Array("Name", "ProcessId", "ParentProcessId", "CommandLine");
    var line = "";
    var blanks = "                       ";

    function paddingTo(str, length) {
        str = "" + str;
        var len = length - str.length;
        if ( len > 0 ) {
            str = str.concat(blanks.substring(0, len));
        }
        return str;
    }

    line += "    ";
    for(var i=0; i<columns.length; i++) {
       line += paddingTo(columns[i], 18);
    }
    WScript.echo(line);

    var n =1;
    for (; !e.atEnd(); e.moveNext())
    {
        var Process = e.item();
        line = paddingTo(n + ": ", 4);
        n++;
        for(i=0; i<columns.length; i++) {
            line += paddingTo(Process[columns[i]], 18);
        }
        WScript.echo(line);
    }
}

/**
 * Terminate all processes with given name.
 */
function KillAll(procName) {
    var e= new Enumerator(GetObject("winmgmts:").InstancesOf("Win32_process"))
    for (; !e.atEnd(); e.moveNext())
    {
        var proc = e.item();
        if (proc["Name"] == procName) {
            if( proc.Terminate() != 0 ) {
                WScript.echo("Cannot terminate " + procName);
            }
        }
    }
}
