mergeInto(LibraryManager.library, {

  GetUmbracoPlayerNameJS: function () {
    var playerName = "";
    
    try {
        // Look at the PARENT window (the Umbraco page) for the variable.
        if (typeof window.parent.UmbracoPlayerName !== 'undefined' && window.parent.UmbracoPlayerName !== null) {
            playerName = window.parent.UmbracoPlayerName;
        }
    } catch (e) {
        // This prevents the game from crashing if the browser blocks the iframe from reading the parent page
        console.warn("[Unity Bridge] Could not access parent window variables: " + e.message);
    }

    // Convert the string (whether it is a real name or completely empty) to a format Unity C# can read
    var bufferSize = lengthBytesUTF8(playerName) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(playerName, buffer, bufferSize);
    
    return buffer;
  }

});