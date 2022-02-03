## WordleSolver

The App [WordleSolver](/WordleSolver) helps you to solve the online game [Wordle](https://www.powerlanguage.co.uk/wordle/)  
It uses a word library from wordle.

## AutomaticWordleSolver
[AutomaticWordleSolver](/AutomaticWordleSolver) has still room for improvement current status:
```
All done. Guesscount 8803, games played 2315, average 3.80
worst word is rover with 9 tries
Scoreboard:
first guess:    1 ( 0.04%)   1x6=  6pt
second guess: 133 ( 5.75%) 133x5=665pt
third guess:  790 (34.13%) 790x4=3160pt
fourth guess: 933 (40.30%) 933x3=2799pt
fifth guess:  345 (14.90%) 345x2=690pt
sixt guess:    89 ( 3.84%)  89x1= 89pt
more:          24 ( 1.04%)  24x0=  0pt
                         Score: 7409pt
```
This app goes to a randomized list of all words from wordle and solves them.

## WordlePuzzleGenerator
The [WordlePuzzleGenerator](/WordlePuzzleGenerator) takes the input list and generates randomized version of it. For what purpose you ask? Currently none, I first thought of using different word-dictionaries. But abandoned this. For the AutomaticWordleSolver it is irrelevant if the list is randomized or not.

## WordleSolverLibrary
The [WordleSolverLibrary](/WordleSolverLibrary) holds the class Solver and Game. This project is reverenced by the others to do the heavy lifting.
