# SmallProjects

## JS_IO (Javascript)
Is a collection of js file reading/writing IO.  
Reading functionalities: [`readStreamToString`](https://github.com/Brandhoej/SmallProjects/blob/44afe4735c2ac9715cd8a9a3c58ae7bbadc235f5/JS_IO/Read.js#L15), [`readRange`](https://github.com/Brandhoej/SmallProjects/blob/44afe4735c2ac9715cd8a9a3c58ae7bbadc235f5/JS_IO/Read.js#L25), [`readLineByLine`](https://github.com/Brandhoej/SmallProjects/blob/44afe4735c2ac9715cd8a9a3c58ae7bbadc235f5/JS_IO/Read.js#L40), [`readNthLine`](https://github.com/Brandhoej/SmallProjects/blob/44afe4735c2ac9715cd8a9a3c58ae7bbadc235f5/JS_IO/Read.js#L52).  
Pending: Read last n characters, read first n characters.  

Writing functionalities: None  
Pending: Write raw binary in sequence, binary string ("0b00100100", "0x24" -> '$' (utf8)) to characters in file.

## SoccerLeauge (C)
Reads a file with soccet match information and creates a hashtable with each team and their stats. Collision are handled by chaining and the table strictly uses void pointers ro generalise.

## SudokuSolver (Javascript)
Personal exploration of recursive sudoku solving using backtracking, bottom-up, top-down.

## Yatzy (C)
A simple Yatzy game simulation where the amount of rolls can be altered by input and the amount of sides on a die can be altered.

## Graphs (C#, WIP)
My take on graphs might not follow the perceived OOA&D. From what i have observed the main idea is that that the graph class is an abstract which multiple other forms of graphs inherits from. But i will experiment with the observation that the main properties of a graph is determined by the interconnected verticies. Therefore, I will create a graph which type is determined by the usage of the edgeset. This way I might be able to use composition over inheritance.  
A comprehensive graph library (`Null graph*`, `trivial graph*`, `non-directed graph*`, `directed graph*`, `simple graph*`, `connected graph*`, `disconnected graph*`, `regular graph*`, `complete graph*`, `wheel graph*`, `cyclic graph*`, `acyclic graph*`, `bipartite graph*`, `complete bipartite graph*`, `star grap*` and `complement graph*`). 

(*) TODO
