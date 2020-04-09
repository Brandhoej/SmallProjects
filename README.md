# SmallProjects

## JS_IO
Is a collection of js file reading/writing IO.  
Reading functionalities: [`readStreamToString`](https://github.com/Brandhoej/SmallProjects/blob/44afe4735c2ac9715cd8a9a3c58ae7bbadc235f5/JS_IO/Read.js#L15), [`readRange`](https://github.com/Brandhoej/SmallProjects/blob/44afe4735c2ac9715cd8a9a3c58ae7bbadc235f5/JS_IO/Read.js#L25), [`readLineByLine`](https://github.com/Brandhoej/SmallProjects/blob/44afe4735c2ac9715cd8a9a3c58ae7bbadc235f5/JS_IO/Read.js#L40), [`readNthLine`](https://github.com/Brandhoej/SmallProjects/blob/44afe4735c2ac9715cd8a9a3c58ae7bbadc235f5/JS_IO/Read.js#L52).  
Pending: Read last n characters, read first n characters.  

Writing functionalities: None  
Pending: Write raw binary in sequence, binary string ("0b00100100", "0x24" -> '$' (utf8)) to characters in file.

## SoccerLeauge
Reads a file with soccet match information and creates a hashtable with each team and their stats. Collision are handled by chaining and the table strictly uses void pointers ro generalise.

## SudokuSolver
Personal exploration of recursive sudoku solving using backtracking, bottom-up, top-down.

## Yatzy
A simepl Yatzy game simulation where the aount of rolls can be altered by input and the amount of sides on a die can be altered.
