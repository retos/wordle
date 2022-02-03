Console.WriteLine("wordle solver - use this app to solve the online game wordle");
Console.WriteLine($"Mark matching g:green, y:yellow, b:black");
Solver solver = new Solver();

for (int j = 0; j < 10; j++)
{
    //suggest word to try
    Word picked = solver.PickNextWord();

    Console.WriteLine($"Suggestion: {picked.Value}");
    bool skipThisWord = false;

    for (int i = 0; i < 5; i++)
    {
        if (skipThisWord) { break;}
        //jump to word above
        int currentTop = Console.CursorTop;
        int currentLeft = Console.CursorLeft;
        Console.SetCursorPosition(currentLeft + 12 + i, currentTop - 1);
        ConsoleKeyInfo input = Console.ReadKey();
        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
        Console.ForegroundColor = ConsoleColor.Black;

        switch (input.KeyChar)
        {
            case 'g':
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write(picked.Value[i]);
                solver.GreenCharacters[picked.Value[i]] = i;
                break;
            case 'y':
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.Write(picked.Value[i]);
                if (!solver.YellowCharacters.ContainsKey(i))
                {
                    solver.YellowCharacters[i] = new List<char>();
                }
                solver.YellowCharacters[i].Add(picked.Value[i]);
                break;
            case 'b':
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write(picked.Value[i]);
                if (!solver.LetterCount.ContainsKey(picked.Value[i]))//current letter occures -> ignore this gray match for now
                {
                    if (!solver.GreyCharacters.Contains(picked.Value[i]))//is already in list, do not add again
                    {//new -> add
                        solver.GreyCharacters.Add(picked.Value[i]);
                    }
                }
                break;
            default:
                break;
        }
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(currentLeft, currentTop);
    }
}