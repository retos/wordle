using WordleLibrary;

Console.WriteLine("Automatic Wordle-Solver!");

string gameInput = "puzzle.txt";

//https://en.wikipedia.org/wiki/Letter_frequency#:~:text=top%20eight%20characters.-,Relative%20frequencies%20of%20letters%20in%20the%20English%20language,%2C%20Q%2C%20X%2C%20Z.

List<string> puzzleInput = Solver.ReadInput(gameInput);
int totalGuesses = 0;
int gamesPlayed = 0;
int maxGuess = 0;
string maxGuessWord = string.Empty;

foreach (string entry in puzzleInput)
{
    int guessCounter = 0;
    Console.WriteLine($"Starting game for {entry}");
    Game game = new Game(entry);
    Solver solver = new Solver();
    string matchInfo = string.Empty;

    do
    {
        Word pickedWord = solver.PickNextWord();
        guessCounter++;
        matchInfo = game.Guess(pickedWord);
        
        //commment out for debug...
        Console.Write($"Guess {guessCounter} is word ");
        Console.ForegroundColor = ConsoleColor.Black;
        for (int i = 0; i < 5; i++)
        {
            switch (matchInfo[i])
            {
                case 'b':
                    Console.BackgroundColor = ConsoleColor.Gray;
                    break;
                case 'g':
                    Console.BackgroundColor = ConsoleColor.Green;
                    break;
                case 'y':
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    break;
                default:
                    break;
            }
            Console.Write(pickedWord.Value[i]);
        }

        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
        

        solver.SetMatching(pickedWord, matchInfo);
    } while (matchInfo != "ggggg");

    gamesPlayed++;
    totalGuesses += guessCounter;

    //Console.WriteLine($"Word {entry}, guesses {guessCounter}, games played {gamesPlayed}, total guesses {totalGuesses}, average {totalGuesses/gamesPlayed}");
    if (guessCounter > maxGuess)
    {
        maxGuess = guessCounter;
        maxGuessWord = entry;
    }
}

Console.WriteLine($"All done. Guesscount {totalGuesses}, games played {gamesPlayed}, average {totalGuesses / gamesPlayed}");
Console.WriteLine($"worst word is {maxGuessWord} with {maxGuess} tries");
Console.ReadKey();