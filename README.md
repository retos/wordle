# wordle solver

The App [WordleSolver](/WordleSolver) helps you to solve the online game [Wordle](https://www.powerlanguage.co.uk/wordle/)

It uses a word library from wordle.

[AutomaticWordleSolver](/AutomaticWordleSolver) has still room for improvement current status:
```
Automatic Wordle-Solver!
All done. Guesscount 9105, games played 2315, average 3
worst word is eater with 9 ties
```
This app goes to a randomized list of all words from wordle and solves them.

The [WordlePuzzleGenerator](/WordlePuzzleGenerator) takes the input list and generates randomized version of it.

The [WordleSolverLibrary](/WordleSolverLibrary) holds the class Solver and Game. This project is reverenced by the others to do the heavy lifting.