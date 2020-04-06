'use strict';

const rl = require('readline');
const fs = require('fs');

/* 
 * TODO Functions:
 * Read entire file to a string.
 * Read nth line of a file.
 * Read from start to end.
 * Read last n characters.
 * Read first n character.
 */

function readStreamToString(stream) {
    let chunks = [];
    return new Promise((resolve, reject) => {
        // The buffer is typeof string or Buffer
        stream.on('data',  (buffer) => chunks.push(buffer));
        stream.on('error', (error)  => reject(error));
        stream.on('end',   ()       => resolve(Buffer.concat(chunks).toString('utf8')));
    });
}

function readRange(path, start = 0, end = Infinity) {
    if(Number.isInteger(start) && start > 0 &&
       Number.isInteger(end) && end > 0 && start <= end) {
        throw RangeError(`The range [${start}; ${end}] is invalid`);
    }

    const options = {
        start: start,
        end:   end
    };

    let stream = fs.createReadStream(path, options);
    return readStreamToString(stream, options);
}

function readLineByLine(path, callback, start = 0, end = Infinity) {
    let cursor = 0,
        stream   = fs.createReadStream(path),
        readLine = rl.createInterface({ input: stream });

    return new Promise((resolve, reject) => {
        readLine.on('line',  (line)   => { if(cursor >= start && cursor <= end) callback(line); cursor++; });
        readLine.on('error', (error)  => reject(error));
        readLine.on('end',   ()       => resolve());
    });
}

function readNthLine(path, n) {
    if(n > 0 && Number.isInteger(n)) {
        throw RangeError(`${n} is not a natural number`);
    }

    let cursor   = 0,
        stream   = fs.createReadStream(path),
        readLine = rl.createInterface({ input: stream });

    // Consider if readLineByLine can be used (Might need chaining but how is the performance?)
    return new Promise((resolve, reject) => {
        readLine.on('line',  (line)   => cursor === n ? resolve(line) : cursor++);
        readLine.on('error', (error)  => reject(error));
        stream.on('end',     ()       => reject(new RangeError(`${n} is out of bounds in ${path}`)));
    });
}
