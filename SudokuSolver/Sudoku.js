/**
 * Credit: Professor Thorsten Altenkirch, Sean Riley and Brady Haran
 * https://www.youtube.com/watch?v=G_UYXzGuqvM&t=511s
 * 
 * Future of this project is a personal exploration of recursive problem solving using backtracking, bottom-up, top-down
 */

'use strict';

let squareDimension = 3;
let squareSize = squareDimension * squareDimension;

let sudoku1 =
[
    [5,3,0,0,7,0,0,0,0],
    [6,0,0,1,9,5,0,0,0],
    [0,9,8,0,0,0,0,6,0],
    [8,0,0,0,6,0,0,0,3],
    [4,0,0,8,0,3,0,0,1],
    [7,0,0,0,2,0,0,0,6],
    [0,6,0,0,0,0,2,8,0],
    [0,0,0,4,1,9,0,0,5],
    [0,0,0,0,8,0,0,7,9]
];

let sudoku1Solved =
[
    [5,3,4,6,7,8,9,1,2],
    [6,7,2,1,9,5,3,4,8],
    [1,9,8,3,4,2,5,6,7],
    [8,5,9,7,6,1,4,2,3],
    [4,2,6,8,5,3,7,9,1],
    [7,1,3,9,2,4,8,5,6],
    [9,6,1,5,3,7,2,8,4],
    [2,8,7,4,1,9,6,3,5],
    [3,4,5,2,8,6,1,7,9]
];

let sudoku2 =
[
    [5,3,0,0,7,0,0,0,0],
    [6,0,0,1,9,5,0,0,0],
    [0,9,8,0,0,0,0,6,0],
    [8,0,0,0,6,0,0,0,3],
    [4,0,0,8,0,3,0,0,1],
    [7,0,0,0,2,0,0,0,6],
    [0,6,0,0,0,0,2,8,0],
    [0,0,0,4,1,9,0,0,5],
    [0,0,0,0,8,0,0,0,0]
];

let sudoku3 =
[
    [0,0,0,0,7,0,0,0,0],
    [6,0,0,1,9,5,0,0,0],
    [0,9,8,0,0,0,0,6,0],
    [8,0,0,0,0,0,0,0,3],
    [4,0,0,0,0,0,0,0,1],
    [7,0,0,0,2,0,0,0,6],
    [0,6,0,0,0,0,2,8,0],
    [0,0,0,4,1,9,0,0,5],
    [0,0,0,0,8,0,0,0,0]
];

let sudoku = sudoku3;

solve();
<
console.log("-------------------------------\n| 1  2  3 | 1  2  3 | 1  2  3 |\n| 1  2  3 | 1  2  3 | 1  2  3 |\n| 1  2  3 | 1  2  3 | 1  2  3 |\n-------------------------------\n" + 
                                             "| 1  2  3 | 1  2  3 | 1  2  3 |\n| 1  2  3 | 1  2  3 | 1  2  3 |\n| 1  2  3 | 1  2  3 | 1  2  3 |\n-------------------------------\n" + 
                                             "| 1  2  3 | 1  2  3 | 1  2  3 |\n| 1  2  3 | 1  2  3 | 1  2  3 |\n| 1  2  3 | 1  2  3 | 1  2  3 |\n-------------------------------");

function canPlace(x, y, n) {
    return canBePlacedRowColumn(x, y, n) && canBePlacedSquare(x, y, n);
}

function canBePlacedRowColumn(x, y, n) {
    for(let ri = 0; ri < squareSize; ri++) {
        if(sudoku[ri][x] === n || sudoku[y][ri] === n) {
            return false;
        }
    }
    return true;
}

function canBePlacedSquare(x, y, n) {
    // Get the square indices
    let sqrBXI = squareDimension * Math.floor(x / squareDimension);
    let sqrBYI = squareDimension * Math.floor(y / squareDimension);

    for(let rX = 0; rX < squareDimension; rX++) {
        for(let rY = 0; rY < squareDimension; rY++) {
            if(sudoku[sqrBYI + rY][sqrBXI + rX] === n) {
                return false;
            }
        }
    }
    return true;
}

function solve() {
    let solutions = [];
    _solve();

    function _solve() {
        for(let y = 0; y < squareSize; y++) {
            for(let x = 0; x < squareSize; x++) {
                if(sudoku[y][x] === 0) {
                    for(let n = 1; n <= squareSize; n++) {
                        if(canPlace(x, y, n)) {
                            sudoku[y][x] = n;
                            _solve();
                            sudoku[y][x] = 0;
                        }
                    }
                    return;
                }
            }
        }

        let cpy = [];
        for(let i = 0; i < squareSize; i++) {
            // Cpy each sub array by slicing it from 0 to end
            cpy[i] = sudoku[i].slice(0);
        }

        // Add a copy of the solution to solutions
        solutions.push(cpy);
    }

    console.log(`Found ${solutions.length} solutions`);
    console.log(solutions);
}