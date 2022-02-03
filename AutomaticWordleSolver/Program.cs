using WordleLibrary;

Console.WriteLine("Automatic Wordle-Solver!");

string gameInput = "puzzle_0.txt";

List<string> puzzleInput = Solver.ReadInput(gameInput);
int totalGuesses = 0;
int gamesPlayed = 0;
int maxGuess = 0;
string maxGuessWord = string.Empty;
bool debugMode = false;
List<Word> solvedInX = new List<Word>();
List<Word> solvedIn1 = new List<Word>();
List<Word> solvedIn2 = new List<Word>();
List<Word> solvedIn3 = new List<Word>();
List<Word> solvedIn4 = new List<Word>();
List<Word> solvedIn5 = new List<Word>();
List<Word> solvedIn6 = new List<Word>();


foreach (string entry in puzzleInput)
{
    int guessCounter = 0;
    Game game = new Game(entry);
    Solver solver = new Solver();
    string matchInfo = string.Empty;
    Word pickedWord;

    do
    {
        pickedWord = solver.PickNextWord();
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

    switch (guessCounter)
    {
        case 1:
            solvedIn1.Add(pickedWord);
            break;
        case 2:
            solvedIn2.Add(pickedWord);
            break;
        case 3:
            solvedIn3.Add(pickedWord);
            break;
        case 4:
            solvedIn4.Add(pickedWord);
            break;
        case 5:
            solvedIn5.Add(pickedWord);
            break;
        case 6:
            solvedIn6.Add(pickedWord);
            break;
        default:
            solvedInX.Add(pickedWord);
            break;
    }

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
Console.WriteLine($"Scoreboard:");
Console.WriteLine($"first guess:  {solvedIn1.Count().ToString().PadLeft(3)} ({((float)solvedIn1.Count() / gamesPlayed * 100).ToString("0.00").PadLeft(5)}%) {solvedIn1.Count().ToString().PadLeft(3)}x6={(solvedIn1.Count()*6).ToString().PadLeft(3)}pt");
Console.WriteLine($"second guess: {solvedIn2.Count().ToString().PadLeft(3)} ({((float)solvedIn2.Count() / gamesPlayed * 100).ToString("0.00").PadLeft(5)}%) {solvedIn2.Count().ToString().PadLeft(3)}x5={(solvedIn2.Count()*5).ToString().PadLeft(3)}pt");
Console.WriteLine($"third guess:  {solvedIn3.Count().ToString().PadLeft(3)} ({((float)solvedIn3.Count() / gamesPlayed * 100).ToString("0.00").PadLeft(5)}%) {solvedIn3.Count().ToString().PadLeft(3)}x4={(solvedIn3.Count()*4).ToString().PadLeft(3)}pt");
Console.WriteLine($"fourth guess: {solvedIn4.Count().ToString().PadLeft(3)} ({((float)solvedIn4.Count() / gamesPlayed * 100).ToString("0.00").PadLeft(5)}%) {solvedIn4.Count().ToString().PadLeft(3)}x3={(solvedIn4.Count()*3).ToString().PadLeft(3)}pt");
Console.WriteLine($"fifth guess:  {solvedIn5.Count().ToString().PadLeft(3)} ({((float)solvedIn5.Count() / gamesPlayed * 100).ToString("0.00").PadLeft(5)}%) {solvedIn5.Count().ToString().PadLeft(3)}x2={(solvedIn5.Count()*2).ToString().PadLeft(3)}pt");
Console.WriteLine($"sixt guess:   {solvedIn6.Count().ToString().PadLeft(3)} ({((float)solvedIn6.Count() / gamesPlayed * 100).ToString("0.00").PadLeft(5)}%) {solvedIn6.Count().ToString().PadLeft(3)}x1={(solvedIn6.Count()*1).ToString().PadLeft(3)}pt");
Console.WriteLine($"more:         {solvedInX.Count().ToString().PadLeft(3)} ({((float)solvedInX.Count() / gamesPlayed * 100).ToString("0.00").PadLeft(5)}%) {solvedInX.Count().ToString().PadLeft(3)}x0=  0pt");

Console.WriteLine($"                       Score: = {(solvedIn1.Count()*6) + (solvedIn2.Count() * 5) + (solvedIn3.Count() * 4) + (solvedIn4.Count() * 3) + (solvedIn5.Count() * 2) + (solvedIn6.Count() * 1)}pt");


Console.ReadKey();