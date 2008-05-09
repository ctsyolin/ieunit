/**
 * @file
 * Win32Dom class provices direct access to Windows desktop window.
 */
 
/**
 * Contruct a Win32Dom object. 
 */
function Win32Dom() {
    this._desktop = null;

    function buildXPath(cls, cap) {
        if ( cap == undefined ) cap = null;
        if ( cls == undefined ) cls = null;
        var xpath = ".//Win";
        if ( (cls != null) && ( cap != null ) ) {
            xpath += "[@Cls='" + cls + "' and contains(@Cap, '" + cap + "')]";        
        } else if ( cls != null ) {
            xpath += "[@Cls='" + cls + "']";                
        } else if ( cap != null ) {
            xpath += "[contains(@Cap, '" + cap + "')]";        
        }
        return xpath;
    };

    /**
     * Search for a window with given caption and timeout value in milliseconds.
     * If timeoutMS is omitted this method will wait for 1500 milliseconds.
     */
    this.waitForWindow = function(caption, timeoutMS) {
        if ( this._desktop == null ) {
            this._desktop = new ActiveXObject("Win32Dom.Desktoop");
        }
        
        if( timeoutMS == undefined ) {
            timeoutMS = 1500;
        }
        
        for(var i=0; i<timeoutMS; i+=100) {
            var win = this._desktop.findTopWindow(caption);   
            if ( win != null ) {
                return win;
            }
            WScript.Sleep(100);
        }
        return null;
    };
    
    /**
     * Find the first top level window whose caption contains given text.
     */
    this.findTopWindow = function(caption) {
        return this._desktop.findTopWindow(caption);
    };
     
    /**
     * Find all top level windows 
     */
    this.findAllTopWindows = function() {
        return this._desktop.findAllTopWindows();
    };
    
    /**
     * Find all child windows of a window with given class and caption 
     */
    this.findWindowList = function(win, cls, cap) {
        return win.findWindowList( buildXPath(cls, cap) );
    };

    /**
     * Find a child window with given class and caption 
     */
    this.findWindow = function(win, cls, cap) {
        return win.findWindow( buildXPath(cls, cap) );
    };

    /**
     * Find a button child window with given label and index.
     */
    this.findWinButton = function(win, label, index) {
        var node;    
        if ( (index == undefined) || (index==0) ) {
            node = this.findWindow(win, "Button", label);
        } else {
            var nodes = this.findWindowList(win, "Button", label);
            if ( nodes.Count > index ) {
                node = nodes(index);
            }
        }     
        return (node == null) ? null : node.ToButton();
    };

    /**
     * Find a text edit field window with given index
     */ 
    this.findTextBox = function(win, index) {
        var nodes = this.findWindowList(win, "Edit");
        if ( nodes.Count > index ) {
            return nodes(index).ToTextBox();
        } else {
            return null;
        }
    };
    
    /**
     * Pass the basic login window with given user name and password.
     */
    this.openWindowAsUser = function(url, userName, userPwd) {
        var loginWinCap = url.substring(url.indexOf("//")+2);
        loginWinCap = loginWinCap.substring(0, loginWinCap.indexOf("/"));

        var portIndex = loginWinCap.indexOf(":");
        if (portIndex != -1) {
          loginWinCap = loginWinCap.substring(0, portIndex);
        }

        loginWinCap = "Connect to " + loginWinCap;

        this.openWindowAsync(url);
        var loginWin = this.waitForWindow(loginWinCap, this.findTimeout);        

        this.findTextBox(loginWin, 1).text = userName;
        this.findTextBox(loginWin, 2).text = userPwd;
        this.findWinButton(loginWin, "OK").Click();

        this.checkSubmit();    
    };
    
    
    /**
     * Open a URL and acknowledge possible popup window with given title by 
     * click the button with given label.
     */
    this.acceptPopupWindow = function(url, title, btnLable) {
        this.openWindowAsync(url);
        var alertWin = this.waitForWindow(title); 
        if ( alertWin != null ) {
            this.findWinButton(alertWin, btnLable).Click();
        }
        this.checkSubmit();
    };
    
    /**
     * Open a URL and skip possible security alert window
     */
    this.skipAlert = function(url) {
        this.acceptPopupWindow(url, "Security Alert", "Yes");
    };    


    /**
     * Attach to a IE modal dialog window. This function is usually called
     * in sbk script launched before opening a modal dialog.
     */
    this.waitForModalDialog = function(winTitle) { 
	    var popupWin = this.waitForWindow(winTitle, this.findTimeout); 
        if ( popupWin == null ) {
            this.log("Can't find modal dialog window with title: " + winTitle);
            return;
        }

	    var ie = this.findWindow(popupWin, "Internet Explorer_Server"); 

	    this.findScope = this.doc = ie.toDhtml(); 
	    this.comWin = this.win = this.doc.parentWindow; 
    }; 
};

