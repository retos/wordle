using WordleLibrary;

Console.WriteLine("Automatic Wordle-Solver!");

string gameInput = "puzzle_0.txt";

List<string> puzzleInput = Solver.ReadInput(gameInput);
int totalGuesses = 0;
int gamesPlayed = 0;
int maxGuess = 0;
string maxGuessWord = string.Empty;
bool debugMode = false;

foreach (string entry in puzzleInput)
{
    int guessCounter = 0;
    Game game = new Game(entry);
    Solver solver = new Solver();
    string matchInfo = string.Empty;

    do
    {
        Word pickedWord = solver.PickNextWord();
        guessCounter++;
        matchInfo = game.Guess(pickedWord);

        if (debugMode)
        {            
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
        }

        solver.SetMatching(pickedWord, matchInfo);
    } while (matchInfo != "ggggg");

    gamesPlayed++;
    totalGuesses += guessCounter;

    Console.WriteLine($"Word {entry}, guesses {guessCounter}, games played {gamesPlayed}, total guesses {totalGuesses}, average {((float)totalGuesses / gamesPlayed).ToString("0.00")}");
    if (guessCounter > maxGuess)
    {
        maxGuess = guessCounter;
        maxGuessWord = entry;
    }
}

Console.WriteLine($"All done. Guesscount {totalGuesses}, games played {gamesPlayed}, average {((float)totalGuesses / gamesPlayed).ToString("0.00")}");
Console.WriteLine($"worst word is {maxGuessWord} with {maxGuess} tries");
Console.ReadKey();