mergeInto(LibraryManager.library, {

    // This is the JS function Unity will call
    GetUmbracoPlayerNameJS: function () {
        var playerName = "";
        
        // 1. Look for the variable Umbraco left for us
        if (typeof window.UmbracoPlayerName !== 'undefined') {
            playerName = window.UmbracoPlayerName;
        }

        // 2. Unity WebGL requires strings to be converted to memory buffers
        var bufferSize = lengthBytesUTF8(playerName) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(playerName, buffer, bufferSize);
        
        // 3. Hand the buffer back to Unity C#
        return buffer;
    }

});