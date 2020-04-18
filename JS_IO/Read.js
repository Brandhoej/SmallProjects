const fs = require('fs');

const defaultEncoding = 'utf8';

/**
 * Reads a stream and converts it to string using the passed encoding. This is promised based.
 * 
 * @Example
 * readStreamToString(path).then((str) => console.log(str));
 * 
 * @param {Stream} stream                     use createReadStream for creation of a ReadStream which extends Stream.Readable
 * @param {String} [encoding=defaultEncoding] encoding the encoding used to convert to string format
 * 
 * @return {promise} a promise which resolves with the complete string representation of what the read stream read
 */
function readStreamToString(stream, encoding = defaultEncoding) {
    if(stream == undefined) {
        throw ReferenceError("stream is undefined");
    }

    if(encoding == undefined) {
        throw ReferenceError("encoding is undefined");
    }

    // The chunks of data which have been read. Acts like a buffer always in order.
    let chunks = [];
    return new Promise((resolve, reject) => {
        // The buffer is typeof string or Buffer
        stream.on('data',  (buffer) => chunks.push(buffer));
        stream.on('error', (error)  => reject(error));
        stream.on('end',   ()       => resolve(Buffer.concat(chunks).toString(encoding)));
    });
}

/**
 * Reads a specified range of a file. This is promised based.
 * 
 * @Example
 * // Reads the complete file like readStreamToString
 * readRange(path).then((str) => console.log(str));
 * // Reads the first character of a file
 * readRange(path, 0, 0).then((str) => console.log(str));
 * // Reads from character 0 to 100 which is 101 charcaters in total
 * readRange(path, 0, 100).then((str) => console.log(str));
 * 
 * @param {String} path                       path the file path
 * @param {Number} [start=0]                  start the start position of the cursor inclusive starting from char index 0
 * @param {Number} [end=Infinity]             end  the end position of the the cursor exclusive can be the same as start but not less
 * @param {String} [encoding=defaultEncoding] encoding the encoding used to convert to string format
 * 
 * @returns {object} a promise which resolves with the string representation spanning from start to end in the file at path
 */
function readRange(path, start = 0, end = Infinity, encoding = 'utf8') {
    if(path == undefined) {
        throw ReferenceError("path is undefined");
    }

    if(start == undefined) {
        throw ReferenceError("start is undefined");
    }

    if(end == undefined) {
        throw ReferenceError("end is undefined");
    }

    if(encoding == undefined) {
        throw ReferenceError("encoding is undefined");
    }

    // Checks if the bounds are valid
    if(!Number.isInteger(start) || start < 0 ||
       !Number.isInteger(end) || start <= end) {
        throw RangeError(`The range [${start}; ${end}] is invalid`);
    }

    // The options used to create the read stream
    const options = {
        start: start,
        end:   end
    };

    let stream = fs.createReadStream(path, options);
    return readStreamToString(stream, encoding);
}

/**
 * Uses the fs.access to check mode access to file. Wrapper for both a/sync
 * 
 * Modes:
 * 
 * fs.constants.F_OK :: Flag indicating that the file is visible to the calling process. This is useful for determining if a file exists, but says nothing about rwx permissions. Default if no mode is specified.
 * 
 * fs.constants.R_OK :: Flag indicating that the file can be read by the calling process.
 * 
 * fs.constants.W_OK :: Flag indicating that the file can be written by the calling process.
 * 
 * fs.constants.X_OK :: Flag indicating that the file can be executed by the calling process. This has no effect on Windows (will behave like fs.constants.F_OK).
 * 
 * @example
 * // Check if readable sync
 * if(checkPath(path, fs.constants.R_OK)) {
 *     // Is readable
 * }
 * 
 * // Check if readable async
 * checkPath(path, fs.constants.R_OK, false, (err) => {
 *     if(err == null) {
 *         // Is readable
 *     }
 * });
 * 
 * @param {String} path            the file path
 * @param {Number} mode            fs.constant value for the mode
 * @param {Boolean} sync           if it should be done in sync
 * @param {Function} function(err) for async callback
 * 
 * @returns {boolean} only returns anything if it is in syn mode. Returns mode check state, true for mode access.
 */
function checkPath(path, mode = fs.constants.F_OK, sync = true, callback = undefined) {
    if(path == undefined) {
        throw ReferenceError("path is undefined");
    }

    if(mode == undefined) {
        throw ReferenceError("mode is undefined");
    }

    if(sync == undefined) {
        throw ReferenceError("sync is undefined");
    }

    if(sync === false && callback == undefined) {
        throw ReferenceError("callback is undefined and it has to be defined when doing it async");
    }

    if(!Number.isInteger(mode)) {
        throw RangeError("Mode has to be an integer");
    }

    if(sync === false) {
        fs.access(path, mode, callback);
    } else {
        try {
            // Void function which does not return access for the mode valid/denied
            fs.accessSync(path, mode);
            return true;
        } catch(arr) {
            // Throws error if access for the mode was rejected
            return false;
        }
    }
}

exports.readStreamToString = readStreamToString;
exports.readRange = readRange;
exports.checkPath = checkPath;