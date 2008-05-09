requireIeUnit(2,1);

function GetPwd(pwdName) {
    var winTitle = "IeUnit Password Holder";

    var wins = WScript.CreateObject("Shell.Application").Windows();
    var win = null;
    var doc = null;
    for( var i=0; i < wins.Count; i++) {
        try {
            doc = wins.item(i).document;
            if ( _._contains(doc.title, winTitle) ) { 
                win = wins.item(i);
                break;
            }
        } catch(e) {}
    }

    if ( win == null ) {
        win = WScript.CreateObject("InternetExplorer.Application");
        var envs = WScript.CreateObject("WScript.Shell").Environment("Process");
        win.navigate(envs("IEUNIT_HOME") + "local\\PwdHolder.html");
        win.visible = true;
    }
    
    for(var t=0; t<=30000; t+=100) {
       _.sleep(100);
       try {
           if ( win.document.readyState == "complete" ) {
                if ( win.document.parentWindow.pwdReady ) {
                    break;
                }
           }
       } catch(ex) {}
    }
    
	return win.document.all(pwdName).value;
}
