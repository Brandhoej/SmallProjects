/* 
 * Functions:
 * write raw binary in sequence. Maybe in the size of a byte. Could take a string as input.
 * 
 * Create easy a-/sync using options: 'as+/rs+'
 * TODO: Validate options
 */

const defaultEncoding = 'utf8';

/**
 * Wrapper function for using fs to writeFile
 * Use options for the desired writing to file action
 * 
 * Does not throw an error if unsuccessful. The error is passed in the callback.
 * Does not check if a read stream is open for the same file.
 * Does not check if the file is not a directory before trying to open the stream
 * 
 * @param {String} path                   The path where the new file should be created.
 * @param {Array} data                    The data to write to file using options. If something other than a Buffer or Uint8Array is provided, the value is coerced to a string. Default can be a string.
 * @param {String} options                Either the encoding for the file, or an object optionally specifying the encoding, file mode, and flag. If encoding is not supplied, the default of 'utf8' is used. If mode is not supplied, the default of 0o666 is used. If mode is a string, it is parsed as an octal integer. If flag is not supplied, the default of 'w' is used.
 * @param {Function} [callback=undefined] The callback of the function which takes an error as paramter. If success then error was null.
 */
function writeFile(path, data, options, callback) {
    if(path == undefined) {
        throw ReferenceError("path is undefined");
    }

    if(array == undefined) {
        throw ReferenceError("array is undefined");
    }

    fs.writeFile(path, data, options, (err) => {
        if(callback != undefined) {
            callback(err);
        }
    });
}

/**
 * Function for writing a file containing a given array.
 * use the options for the desired action to happen in the file.
 * 
 * Throws an error in the function if unsuccessful.
 * 
 * @param {String} path                   The path where the new file should be created.
 * @param {Array} array                   The array that should be included in the file
 * @param {String} options                The options for writing.
 * @param {Function} [callback=undefined] The callback of the function which takes an error as paramter. If succes then error was null.
 */
function writeArray(path, array, options = 'w', callback = undefined) {
    if(path == undefined) {
        throw ReferenceError("path is undefined");
    }

    if(array == undefined) {
        throw ReferenceError("array is undefined");
    }

    let stringData = JSON.stringify(array).split('],').join('],\n');
    defaultEncoding(path, stringData, options, callback);
}