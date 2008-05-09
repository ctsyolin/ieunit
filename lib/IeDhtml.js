/**
 * @file
 * Implements services to interact with the DHTML model within
 * Internet Explorer windows.
 */  

/** Construct an IeDhtml object. */
function IeDhtml() {
     /** Reference to IE COM object. */
    this.comWin = null;
    
    /** The underlying DHTML document object. */
    this.doc = null;

    /** Reference to document's window object.*/
    this.win = null;

    /** The pause in milliseconds after submission. */
    this.submitPause = IeDhtml_submitPause;

    /** Timeout value in milliseconds for find-operations. */
    this.findTimeout = IeDhtml_findTimeout;

    /** Search scope object for most find-operations. */
    this.findScope   = null;   

    /** The index, name or id of the default frame for the doc object. */
    this.defaultFrame = -1;         
	
    /** Sleeps for given number of milliseconds. */
    this._sleep = function _sleep(milliseconds) {
        WScript.Sleep(milliseconds);
    };

    /**
     * Start IE and navigate it to given URL. 
     * Don't wait for the document changes to completed status.
     * @see openWindow
     */
    this.openWindowAsync = function openWindowAsync(url) {
        this.comWin = WScript.CreateObject("InternetExplorer.Application");
        if (this.ieWindowVisible!=false) {
            this.ieWindowVisible=true;
        }
        this.comWin.visible = this.ieWindowVisible;
        if ( (url.substr(0,5)!="http:") && (url.substr(0, 6)!="https:") && (url.charAt(1)!=':') ) {
            // url is probably a local relative file path, we convert to
            // absolute path here
	        if ( url.substr(0, 6) != "about:" ) {
                var fso = new ActiveXObject("Scripting.FileSystemObject");
                var f = fso.GetFile(url);
                url = f.Path;
	        }
        }
        this.comWin.navigate(url);
        return this;
    };

    /**
     * Start IE and navigate it to given URL.
     * @see closeWindow
     * @see seekWindow
     * @see setWindow
     */
    this.openWindow = function openWindow(url) {
        this.openWindowAsync(url);
        this.checkSubmit();
        return this;
    };

    /** Navigate current IE browser to a new URL. */
    this.navigateTo = function navigateTo(url) {
        this.comWin.navigate(url);
        this.checkSubmit();
    };
    
    /** 
     * Navigate IE to a new URL and waits until the document's 
     * status becomes completed 
     */
    this.navigateToAsyn = function navigateToAsyn(url) {
        this.comWin.navigate(url);
    };

    /**
     * Close the current IE window.
     * @see openWindow
     * @see seekWindow
     * @see setWindow
     */
    this.closeWindow = function closeWindow() {
        this.comWin.quit();
        this._sleep(300);
        this.comWin = null;
        this.win = null;
        this.doc = null;
        return this;
    };

    /**
     * Seek a window with given pattern in its titles. If there is more than one such windows,
     * the last one will be returned, unless returnFirst is set to true in which
     * case the first window found will be returned.
     *
     * Together with method setWindow() you can attach an IeDhtml to 
     * any open Internet Explorer window. 
     *
     * @see setWindow
     */
    this.seekWindow = function seekWindow(titlePattern, returnFirst) {
        var doc = null;
        var win = null;

        for(var t=0; t<=this.findTimeout; t+=100) {
            this._sleep(200);
            var wins = (WScript.CreateObject("Shell.Application")).Windows();
            for( var i=0; i < wins.Count; i++) {
                try {
                    doc = wins.item(i).document;
                    if ( this._contains(doc.title, titlePattern) ) { 
                        win = wins.item(i);
                        if (returnFirst) break;
                    }
                } catch(e) {}
            }
            if (win != null ) return win;
        }
            
        throw new Error(1100, "Can't find window containing title '" + titlePattern + "'");
    };
    
    /**
     * Find IE window whose title matches titlePattern and set the found window
     * as the window object of current IeDhtml object. If the second argument is provided
     * and is the boolean true, the first matching window will be set, otherwise
     * the last one.
     */
     this.seekAndSetWindow = function(titlePattern, returnFirst) { 
	     var newWin=this.seekWindow(titlePattern, returnFirst); 
	     this.setWindow(newWin); 
	     return newWin;
     };    
                 
    //================================================================
    // Some set methods.
    //================================================================

    /**
     * Change the default frame index for window with frames. After each
     * submit action, the document object of the frame with the default
     * index/id/name will become the default document object this.doc. 
     * The argument may be the index, the name or the id of a frame.
     * If the frames are nested through multiple document the argument
     * must be a string of syntax "idx_or_id1/idx_or_id2/...".
     *
     * @see setFindScopeToFrame
     */
    this.setFrame = function setFrame(idx_or_id) {
        this.defaultFrame = idx_or_id;
        if ( this.comWin != null ) {
            this.checkSubmit();
        }
        return this;
    };
    	
    /**
     * Set the find scope to the document object of frame with given index.
     */
    this.setFindScopeToFrame = function setFindScopeToFrame(idx) {
        if ( (this.doc.frames != undefined ) && (this.doc.frames.length>idx) ) {
            this.doc = this.doc.frames(idx).document;
        }
        this.findScope = this.doc;    
        return this;
    };

    /**
     * Set given IE window object as current window object of this IeDhtml object.
     */
    this.setWindow = function setWindow( win ) {
        this.comWin = win;
        this.checkSubmit();
        return this;
    };

    /**
     * Set the timeout values. 'pause' specifies the minimum time in milliseconds
     * a submit action will wait before returning. 'timeout'
     * specifies the maximum time in milliseconds that a find operation
     * will wait for completion. If no object can be found, an exception will normally
     * be thrown.
     */ 
    this.setTime = function setTime(pause, timeout) {
        if (pause > 0) { 
            this.submitPause = pause; 
        }

        if (timeout>0) {
            this.findTimeout = timeout;
        }
        return this;
    };

    /** 
     * Set the find scope to a DHTML object. The default find scope is the whole 
     * document object of the default frame.  Using this method we can restrict 
     * the searching scopes. Most find operations search within the find scope.
     */
    this.setFindScope = function setFindScope( scope ) {
        this.findScope = scope;
        return this;
    };

    //================================================================
    // Methods to localize objects. All find methods should return 
    // the object found, or null if the object is not found. No HTTP
    // requested should be submitted.
    //================================================================

    /** 
     * Find an anchor object whose description contains given string.
     * If the index paramter is present, the index-th anchor object
     * containing the given string will be returned.
     * If the parameter str_or_reg is regular expression object, e.g. /Hello.*Word/i, 
     * the regular expression will be used to find matching text.
     */
    this.findLink = function findLink(str_or_reg, index) {
        if ( index == undefined) index = 0; 

        var list = this.findScope.all.tags("A");
        for (var i=0; i<list.length; i++) {
            var label = list[i].innerText;
            var match = ( typeof(str_or_reg) == "string" ) 
                ? this._contains(label, str_or_reg) : str_or_reg.test(label);
            if ( match ) {
                index--;
                if ( index < 0 ) {
                    return list[i];
                }
            }
        }
        return null;
    };

    /**
     * Find an image object whose src attribute contains the given string
     */
    this.findImage = function findImage(src, index) {
        if ( index == undefined) index = 0; 
        var list = this.findScope.all.tags("IMG");
        for (var i=0; i<list.length; i++) {
            if ( this._contains(list[i].src, src) ) {
                index--;
                if ( index < 0 ) {
                    return list[i];
                }
            }
        }
        return null;
    };

    /** 
     * Find an object with given id or name. If the optional tagName and type are 
     * given, the object must have given tag name and
     * type. If there are multiple object with the same id or name which satisfy 
     * the tagName and type condition, the first one found will be returned.
     * Notice: In DHTML element id is case-insensitive.
     */
    this.findObjById = function findObjById(id_or_name, tagName, type) {
        var obj = this.doc.all(id_or_name);
        if ( obj == null ) return null;

        if ( obj.tagName != undefined ) {
            if ( tagName != undefined ) {
                if ( tagName != obj.tagName ) {
                    return null;
                } else {
                    if ( (type != undefined ) && ( obj.type != type ) ) {
                        return null;
                    }
                } 
            }
            return obj;
        } else {
            for (var i=0; i<obj.length; i++) {
                var obj_i = obj[i];
                if ( tagName != undefined ) {
                    if ( tagName != obj_i.tagName ) {
                        continue;
                    } else {
                        if ( (type != undefined ) && ( obj_i.type != type ) ) {
                            continue ;
                        }
                    } 
                }
                return obj_i;
            }   
            return null;
        }
    };

    /**
     * Find a button object whose label contains given string, or whose
     * id equals given string. If the index parameter is given, index-th button
     * with given label will be returned.
     */
    this.findButton = function findButton(str, index) {
        if ( index == undefined) index = 0; 

        var list = this.findScope.all.tags("INPUT");
        for (var i=0; i<list.length; i++) {
            var e = list[i];
            if ( (e.type=="submit") || (e.type=="button") ) {
                if ( this._contains(e.value, str) )  index--;
            } else if ( e.type == "image" ) {
                if ( this._contains(e.alt, str) ) index--;
                if ( this._contains(e.src, str) ) index--;
            }
            if ( index < 0 ) {
                return e;
            }
        }

        list = this.findScope.all.tags("BUTTON");
        for (var j=0; j < list.length; j++) { 
            var e = list[j];
            if (this._contains(e.innerText, str) ) {
                index--;
            }
            if ( index < 0 ) {
                return e;
            }
        }
        return null;
    };

    /**
     * Find an INPUT object of type text or password with given index, id or name.
     */
    this.findField = function findField(idx_or_id) {
        if ( typeof( idx_or_id ) != "number" ) {
            var e = this.findObjById(idx_or_id, "INPUT");
            if ( e == null ) return null;
            if ( (e.type == "password")|| (e.type=="text") ) {
                return e;
            } 
            return null;
        }

        var list = this.findScope.all.tags("INPUT");
        for (var i=0; i<list.length; i++) {
            var e = list[i];
            if ( (e.type=="password") || (e.type=="text") ) idx_or_id--;
            if ( idx_or_id<0 ) {
                return e;
            }
        }
        return null;
    };

    /**
     * Find an check box with given index, id or name.
     */
    this.findCheckBox = function findCheckBox(idx_or_id) {
        if ( typeof(idx_or_id) == "number" ) {
            var list = this.findScope.all.tags("INPUT");
            for (var i=0; i<list.length; i++) {
                var e = list[i];
                if ( (e.type=="checkbox")) idx_or_id--;
                if ( idx_or_id<0 ) return e;
            }
            return null;
        } else {
            return this.findObjById(idx_or_id, "INPUT", "checkbox");
        }
    };

    /**
     * Find an radio box with given index, id or name.
     */
    this.findRadioButton = function findRadioButton(idx_or_id) {
        if ( typeof(idx_or_id) == "number" ) {
            var list = this.findScope.all.tags("INPUT");
            for (var i=0; i<list.length; i++) {
                var e = list[i];
                if ( e.type=="radio" ) idx_or_id--;
                if ( idx_or_id<0 ) return e;
            }
        } else {
            return this.findObjById(idx_or_id, "INPUT", "radio");
        }
        return null;
    };


    /**
     * Find a SELECT object with given index, id or name. If there are multiple 
     * SELECT object with the same id or name the first will be returned.
     */
    this.findSelect = function findSelect(idx_or_id) {
        if ( typeof(idx_or_id) == "number" ) { 
            var list = this.findScope.all.tags("SELECT");
            return ( idx_or_id<list.length) ? list(idx_or_id) : null;
        } else {
            return this.findObjById(idx_or_id, "SELECT");
        }
    };

    /**
     * Find a TEXTAREA object with given index, id or name.
     */
    this.findTextArea = function findTextArea(idx_or_id) {
        if ( typeof( idx_or_id ) != "number" ) {
            var e = this.findObjById(idx_or_id, "TEXTAREA");
            if ( e == null ) return null;
            if (e.type == "textarea") {
                return e;
            } 
            return null;
        }

        var list = this.findScope.all.tags("TEXTAREA");
        for (var i=0; i<list.length; i++) {
            var e = list[i];
            if (e.type == "textarea")  idx_or_id--;
            if ( idx_or_id<0 ) {
                return e;
            }
        }
        return null;
    };


    /**
     * Find the idx-th appearance of given text within the whole document object, 
     * then return the closest object containing the text.
     */
    this.findByText = function findByText(text, idx) {
        if (idx == undefined) idx = 0;
        var rng = this.doc.body.createTextRange();
        if ( rng==null ) return null;

        while(true) {
            if ( rng.findText(text) ) {
                if (idx==0) {
                    return rng.parentElement();
                } else {
                    idx--;
                    rng.collapse(false);
                }
            } else {
                return null;
            }
        }
    };

    /**
     * Find the first parent of given object with given tag name.
     */
    this.findParent = function findParent(obj, tag) {
        if ( obj==null ) return null;

        var e;
        while ( true ) {
            e = obj.parentElement;
            if ( e==null ) return null;
            if ( e.tagName==tag.toUpperCase() ) {
                return e;
            }
            obj = e;
        }
    };

    /**
     * Find a HTML element with given tag name. If the
     * idx argument is specified, the idx-th such element will be returned.  
     * If the tag parameter is the empty string all tags are searched.
     */
    this.findByTag = function findByTag(tag,  idx) {
        var list      = ( tag == "" ) ? this.findScope.all : this.findScope.all.tags(tag);
        if ( idx == undefined ) idx = 0;
        return list[idx];
    };

    /**
     * Find a HTML element with given tag name and attribute sub-string. If the
     * idx argument is specified, the idx-th such element will be returned. 
     * The syntax for the attrFilter is [attrName]~[SubString]. For instance
     * findByTagAndAttr("INPUT", "value~abc", 2) finds the 2-th INPUT object
     * whose value contains the substring abc.
     *
     * If the tag parameter is the empty string all tags are searched.
     * If the attrFilter parameter is empty all this method just returns
     * the idx-th object with the given tag name.
     */
    this.findByTagAndAttr = function findByTagAndAttr(tag, attrFilter, idx) {
        var list      = ( tag == "" ) ? this.findScope.all : this.findScope.all.tags(tag);
        var attrName  = attrFilter.substring(0, attrFilter.indexOf("~"));
        var attrValue = attrFilter.substring(attrFilter.indexOf("~") + 1);


        if ( idx == undefined ) idx = 0;

        if ( attrName=="class" ) attrName = "className"; // fix the problem with getAttribute().

        for (var i=0; i<list.length; i++) {

            if ( attrName.length == 0 ) {
                if (idx-- <= 0) return list[i];
            } else {
                var aValue = list[i].getAttribute(attrName);
                if ( (aValue != undefined)  && ( aValue != null)
                        && this._contains(aValue, attrValue) ) {
                    if ( idx-- <= 0 ) return list[i];
                }
            }
        }
    };
    
    /**
     * Find a cell in a table. The table is identified by given id or index.
     * The cell is identified by row and column index within the table.
     */
    this.findTableCell = function findTableCell(id_or_idx, rowIdx, columnIdx) {
        var table;
        if ( typeof(id_or_idx) == "number") {
            if ( this.findScope.all.tags("TABLE").length <= id_or_idx ){
                return null;
            }
            table = this.findScope.all.tags("TABLE")[id_or_idx];
        } else {
            table = this.findScope.all[id_or_idx];
        }
        if ( table == null ) {
            return null;
        }
        
        if ( table.rows.length <= rowIdx ) return null;
        var row = table.rows[rowIdx];
        
        if ( row.cells.length <= columnIdx) return null;
        return row.cells[columnIdx];            
    };
    
    //================================================================
    // Miscellaneous methods.
    //================================================================

    /** Set the value of a textarea object.  */
    this.setTextArea = function setTextArea(idx_or_id, value) {
        var e = this.findTextArea(idx_or_id);
        if ( e != null ) {
            e.value = value;
            return this;
        } else {
            throw new Error(1103, "Can't find textarea with index or id " + idx_or_id);
        }
    };



    //================================================================
    // Methods for various submit operations.
    //================================================================

    /**
     * Trigger a click event on an DHTML object, then wait until the document's status
     * becomes 'complete' which normally means that the response from the Web server has been
     * loaded completely into the document object.
     *
     * @see checkSubmit
     */
    this.clickObj = function clickObj(obj) {
        if ( obj == null ) {
            throw new Error(1108, "Can't click null object");
        }
        
        obj.fireEvent("onmouseover"); 
        obj.click();
        this.checkSubmit(); 
        return this;
    };

    /**
     * Click on an anchor object whose target contains specific text. If the
     * the index argument is specified, the index-th such anchor 
     * object will be clicked.
     */
    this.clickLink = function clickLink(str_or_reg, index) {
        var e = this.findLink(str_or_reg, index);
        if ( e == null ) {
            throw new Error(1105, "Can't find link whose text contains or matchs '" + str_or_reg + "'");
        }
            
        return this.clickObj( e ); 
    };

    /**
     * Click on a range of anchor objects whose target contains 
     * specific text as defined in argument list. Notice this 
     * method accepts variable number of arguments.
     */
    this.clickLinkRange = function clickLinkRange() {
        for (var x=0;x<arguments.length; x++) {
            this.clickLink(arguments[x]);
        }
    };

    /**
     * Click on a button object whose label contains given text or whose id equals
     * the given text. If the optional index argument is given this function returns
     * the index-th button with given label.
     */
    this.clickButton = function clickButton(text, index) {
        var b = this.findButton(text, index);
        if ( b == null ) {
            throw new Error(1104, "Can't find button whose label contains '" + text + "'");
        } else {
            return this.clickObj( b ); 
        }
    };

    /**
     * Click on an image object whose src attributes contains given string. 
     * If an index argument is given, the index-th such image object will be clicked. 
     */
    this.clickImage = function clickImage(src_sub_str, index) {
        var img = this.findImage(src_sub_str, index);
        if ( img == null ) {
            throw new Error(1109, "Can't find image whose src attributes contains '" + src_sub_str + "'");
        } else {
            return this.clickObj( img );
        }
    };
         
    /**
     * Click on an object with given id or name.
     */
    this.clickObjById = function clickById(id_or_name) {
        var e = this.findObjById(id_or_name);
        if ( e == null ) {
            throw new Error(1104, "Can't find object with id " + id_or_name);
        }
        return this.clickObj(e);
    };    

    /** 
     * Check the submit response.
     * This method is called internally by many methods which are expected to trigger
     * submit operation. This method waits until the document's status becomes 'complete',
     * then set this.doc to the document object of the default frame if the
     * window has frames. This method uses the members submitPause and findTimeout
     * to control the minimum and maximal waiting time.
     *
     * @see setTime
     */
    this.checkSubmit = function checkSubmit() {
        this._sleep(this.submitPause);
        var frmChain = null;
        
        if (this.defaultFrame != -1) {
            if (  typeof( this.defaultFrame ) == "string" ) {
                frmChain = this.defaultFrame.split("/");
            } else {            
                frmChain = new Array();
                frmChain[0] = this.defaultFrame;
            }
        }

        function findNestedDoc(rootDoc, frmC) {
            var theDoc = rootDoc;
            try {
                while( true ) {
                    var id_or_idx = frmC.shift();
                    if ( id_or_idx == null ) break;
                    if ( (typeof(id_or_idx)=="string") && id_or_idx.match(/^\d+$/) ) {
                        id_or_idx = id_or_idx-0;
                    }
                    theDoc = theDoc.frames(id_or_idx).document; 
                }
            } catch (e) {theDoc = null}
            return theDoc;
        };
                
        // Wait until the page is in ready state or timeouted
        for(var t=0; t<=this.findTimeout; t+=100) {
            this._sleep(100);
            try {
                this.doc = this.comWin.document;
                if (this.doc.readyState == "complete") {                    
                    if ( frmChain != null ) {
                        var nestedDoc = findNestedDoc(this.comWin.document, frmChain); 
                        if ( nestedDoc == null ) continue;
                        
                        // wait until the default frame become ready.
                        try {
                            if( nestedDoc.readyState != "complete" ) {
                                continue;
                            }
                            this.findScope = nestedDoc;
                            this.doc = nestedDoc; 
                        } catch(ex) {
                            // The frame probably hosts foreign domain document. 
                            this.findScope = this.doc; 
                        }
                    } else {
                        this.findScope = this.doc;
                    }
                    
                    // wait a little while for possible JavaScript code.
                    this._sleep(this.submitPause*0.2);
                    this.win = this.doc.parentWindow; 
                    return this;
                }
            } catch (ex) {}
        }

        throw new Error(1101, "Waiting for HTTP response timed out");
    };

    //================================================================
    // Methods to enter input values triggering submission.
    //================================================================

    /**
     * Select an option of a SELELT element. The argument
     * idx_or_id is either the index or the id of the SELECT object.
     * opt_str_or_idx is a substring the option or the index of the option.
     */
    this.setSelectOption = function setSelectOption(idx_or_id, substr_or_idx) {
        var s = this.findSelect(idx_or_id);
        if ( s == null ) {
            throw new Error(1106, "Can't find SELECT element with index or id '" + idx_or_id + "'");
        }
        var opt = null;
        
        if ( typeof(substr_or_idx) == "number" ) {
            if ( substr_or_idx < s.options.length ) {
                opt = s.options[substr_or_idx];
            }
        } else {        
            for(var i=0; i<s.options.length; i++) {
                if ( this._contains(s.options[i].innerText, substr_or_idx) ) {
                    opt = s.options[i];
                    break;
                }
            }
        }

        if ( opt != null ) {        
            opt.selected = true;
            s.fireEvent("onchange"); 
            this.checkSubmit();
        } else {
            throw new Error(1106, "Can't find SELECT option '" + substr_or_idx + "'" );
        }
    };

    /**
     * Set the state of the idx-th CHECKBOX object to checked which must be a boolean value.
     */
    this.setCheckBox = function setCheckBox(idx, checked) {
        var b = this.findCheckBox(idx);
        if ( b == null ) {
            throw new Error(1107, "Can't " + idx + "-th check box");
        }
        if ( b.checked != checked ) {
            this.clickObj(b);
        }
        return this;
    };

    /** Toggle the state of the idx-th radio box. */
    this.checkRadioButton = function checkRadioButton(idx) {
        var b = this.findRadioButton(idx);
        if ( b == null ) {
            throw new Error(1108, "Can't " + idx + "-th radio button");
        }
        this.clickObj(b);
        return this;
    };

    /** Set the value of an text input object.  */
    this.setField = function setField(idx_or_id, value) {
        var e = this.findField(idx_or_id);
        if ( e != null ) {
            e.value = value;
            return this;
        } else {
            throw new Error(1103, "Can't find field with index or id " + idx_or_id);
        }
    };

    //================================================================
    // General purpose functions and methods
    //================================================================

    /**
     * Check whether a string contains another string.
     */
    this._contains = function _contains(str, pattern) {
        if ( (pattern == null) || (pattern=="") || ( str.indexOf(pattern)>=0) ) {
            return true;
        } else 
            return false;
    };

    /**
     * Finds a TR object with specific text.
     */
    this.findRowByText = function findRowByText(txt) {
        return this.findParent(this.findByText(txt), "TR");
    };
    
    //================================================================
    // DHtml specifical assert functions
    //================================================================
    /**
     * Assert that a SELECT object has a OPTION object with given substring.
     * Optional message argument will be appended to the exception message 
     * in case of failure.
     */
    this.assertSelectHasOption = function assertSelectHasOption(idx_or_id, optPattern, msg) {
        var s = this.findSelect(idx_or_id);
        for( var i=0; i<s.options.length; i++) {
            if ( this._contains(s.options[i].innerText, optPattern) ) 
                return;
        }

        var txt = "assertSelectHasOption failed: no option containing '" + optPattern +"'";
        if ( msg != undefined) txt += ": " + msg;
        throw new IeError(1011, txt);
    };
    
    /**
     * Assert that the response page's text contains given string.
     * The optional error message will be appended to the exception message
     * in case of failure.
     */
    this.assertPageHasText = function assertPageHasText(str, msg) {
        var pageText = this.doc.documentElement.innerText;
        if ( pageText.indexOf(str) < 0 ) {
            var txt = "assertPageHasText failed: the pages doesn't contain '" + str + "'";
            if ( msg != undefined ) txt += ": " + msg;
            throw new IeError(1009, txt);
        }
    };
    
    /**
     * Assert that an element with given tag name and index contains
     * given text.
     */
    this.assertTagHasText = function assertTagHasText(tag, idx, txt) {
        var e = this.findByTag(tag, idx);
        if ( e == null ) {
            throw new IeError(1014, "Can't find " + idx + "-th element with tag " + tag );
        } else {
            if ( e.innerText.indexOf(txt) < 0 ) {
                throw new IeError(1014, 
                    idx + "-th element with tag " + tag + " doesn't contain + \"" + txt + "\"" );
            }
        }
    };
    
    /**
     * Assert that an element's innerText contains given text.
     * The optional error message will be appended to the exception 
     * message in case of failure.
     */
    this.assertTextContains = function(e, str, msg) {
        var innerText = e.innerText;
        if ( (innerText == null) || ( innerText.indexOf(str)<0 ) ) {
            var txt = "assertTextCoontains failed: element's innerText doesn't contain '" + str + "'";
            if ( msg != undefined ) txt += ": " + msg;
            throw new IeError(1013, txt);
        }
    };
}
