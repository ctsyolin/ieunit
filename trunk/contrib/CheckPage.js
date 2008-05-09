/**
 * Description: CheckPage class implements methods to valid help 
 * check web pages, e.g. dead links, etc.
 * Usage: See example CheckLinks.jst
 *
 */
function CheckPage() {
    this.checkLinks = function(url) {
       this.linksChecked = 0;
       this.linksFailed = 0;

       this.log("Checking URL: " + url);
       this.openWindow(url);

       this.checkDocLinks();
       var frameNumber = this.doc.frames.length;
       for(var i=0; i<frameNumber; i++) {
           try {
               this.setFrame(i);
               this.checkDocLinks();
           } catch (ex) { }
        }
        this.closeWindow();

        this.log("Links checked: " + this.linksChecked);
        this.log("Links faield: " + this.linksFailed);
        this.log("");
    };

    this.checkDocLinks = function() {
        var aLink = new Array(); // the href list
        var aList = this.doc.all.tags("A");

        for(var idx=0; idx<aList.length; idx++) {
            aLink[idx]=aList[idx].href;
        }

        this.log("Checking " + aLink.length + " links...");

        for(var idx=0; idx<aLink.length; idx++) {
            var href = aLink[idx];

            try {
                if ( (href != null) && (href!="") ) {
                    if ( href.match(/^mailto:/i) ) continue;

                    _.log("Checking link: " + href);
                    _.navigateTo(href);
                    this.linksChecked++;
                    if ( this.linksChecked%10 == 0 ) {
			             _.log("Links checked: " + this.linksChecked);
		            }
                }
            } catch (ex) {
                this.linksFailed++;
                this.log("Failed to visit: " + href + ": " + ex.description);
            }
        }
    };
}
