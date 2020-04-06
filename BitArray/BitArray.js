
`use strict`;

/*
 * A BitArray is an encapsulation of a Uint8Array where each bit can be manipulated
 * 
 * Functionalities:
 * Expected binary operations: OR (Union), AND (Intersection), XOR, NOT (Complement)
 * Unary operations: OR, AND, XOR, NOT, Inveret([b1,b2,b3]->[b3,b2,b1])
 * Binary operation: OR(Union), And(Intersection), NOT(Complement), "a[i] AND !b[i]" difference
 * 
 * Array functions:
 * Shift(Insert first), Unshift(Remove first), Push(Insert last), Pull(Remove last), Find(Find first 1/0 bit)
 * get(bitIndex), get(byteIndex, bitIndex)
 * set(bitIndex), set(byteIndex, bitIndex)
 * 
 * startPointer: Number (In the first byte)
 * endPointer:   Number (In the last byte)
 * Length: Number (The amount of bits in the array)
 * 
 * ctr: Initial length
 * 
 * Publics:  lengthBits:Number, lengthBytes:Number(The total length of the internal byte array)
 * Privates: _firstBit:Number, _lastBit:Number, _bytes:Uint8Array
 */

class BitArray {
    constructor(length) {
        this._bytes = new Uint8Array(Math.ceil(this._endPointer / 8));
    }

    get(bitIndex) {
        return this._get(Math.floor(bitIndex / 8), Math.floor(bitIndex % 8));
    }

    _get(byteIndex, bitIndex) {
        let mask = 1 << bitIndex;
        // Can be optimaized by returning the bit value because doing this the equality can be removed. But returning T/F might be more suitable.
        return (this._bytes[byteIndex] & mask) !== 0;
    }

    set(bitIndex, value) {
        this._set(Math.floor(bitIndex / 8), Math.floor(bitIndex % 8), value);
    }

    _set(byteIndex, bitIndex, value) {
        let mask = 1 << bitIndex;
        if(value !== 0) {
            this._bytes[byteIndex] |= mask;
        } else {
            this._bytes[byteIndex] &= ~mask;
        }
    }
}

let bits = new BitArray(100);
for(let i = 0; i < 10; i++) {
    console.log(`i=${i} is ${bits.get(i)}`);
    bits.set(i, 1);
    console.log(`i=${i} is ${bits.get(i)}`);
}
console.log(bits.get(10));