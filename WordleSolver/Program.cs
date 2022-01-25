// See https://aka.ms/new-console-template for more information
#region some prepping
//https://www.powerlanguage.co.uk/wordle/
//https://github.com/dwyl/english-words

string nameOfBaseWordlist = "words_alpha.txt";
string nameOfUnknownWordslist = "unknownWords.txt";
int lengthOfWords = 5;


List<string> list = ReadInput(nameOfBaseWordlist);
list = list.Where(e => e.Count() == lengthOfWords).ToList();
List<string> unkown = ReadInput(nameOfUnknownWordslist);


//Console.WriteLine($"Total count of words with length of {lengthOfWords} {list.Count()}");
List<Word> words = Word.Convert(list);
Dictionary<Char, int> greenCharacters = new();
Dictionary<Char, int> yellowCharacters = new();
List<Char> greyCharacters = new List<Char>();

//Console.WriteLine($"Total count of words with length of {lengthOfWords} and no duplicates {words.Where(w => w.LengthWithoutDuplicates == lengthOfWords).Count()}");

#endregion

Console.WriteLine("wordle solver");
Console.WriteLine($"Mark matching g:green, y:yellow, b:black");
Console.WriteLine($"Press s to skip this word and suggest a new one");

for (int j = 0; j < 10; j++)
{
    //suggest word to try
    Word picked = PickNextWord();
    Console.WriteLine($"Suggestion: {picked.Value}");
    bool skipThisWord = false;

    for (int i = 0; i < lengthOfWords; i++)
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
                greenCharacters[picked.Value[i]] = i;
                break;
            case 'y':
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.Write(picked.Value[i]);
                yellowCharacters[picked.Value[i]] = i;
                break;
            case 'b':
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write(picked.Value[i]);
                greyCharacters.Add(picked.Value[i]);
                break;
            case 's': //skip
                currentTop --;
                skipThisWord = true;
                unkown.Add(picked.Value);
                break;
                //write word to 'unknownWords.txt'
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write(picked.Value[i]);
                greyCharacters.Add(picked.Value[i]);
                break;
            default:
                break;
        }
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(currentLeft, currentTop);
    }
}







//todo allow blacklist if word is not in dictionary of wordle:


//1. rate the words
//2. pick one
//3. mark letters
//green: The letter is in the word and in the correct spot.
//yellow: The letter is in the word but in the wrong spot.
//black: The letter U is not in the word in any spot.
//https://www.geeksforgeeks.org/c-sharp-how-to-change-background-color-of-text-in-console/



List<Word> ReduceList()
{
    //remove skipped words from wordlist
    words = words.Where(e => !unkown.Contains(e.Value)).ToList();

    //remove blacklisted characters
    List<Word> wordsWithoutBlacklisted = words.Where(w => !w.Value.Any(c => greyCharacters.Contains(c))).ToList();

    //Ensure green characters are on the right spot
    List<Word> matchingGreenCharacters = new List<Word>();
    foreach (Word word in wordsWithoutBlacklisted)
    {
        bool isMatch = true;
        foreach (var dict in greenCharacters)
        {
            if (word.Value[dict.Value] != dict.Key)
            {
                isMatch = false;
            }
            if (!isMatch)
            {
                break;
            }
        }
        if (isMatch)
        {
            matchingGreenCharacters.Add(word);
        }
    }

    //ensure yellow are included, but not on the wrong spot & one spot can only be used once!
    List<Word> containYellowCharacters = new List<Word>();

    foreach (Word word in matchingGreenCharacters)
    {
        List<int> usedSpots = new List<int>();
        bool isMatch = true;
        foreach (var dict in yellowCharacters)
        {
            if (!word.Value.Contains(dict.Key))
            {
                isMatch = false;//yellowCharacter was not in word
            }
            else if (word.Value[dict.Value] == dict.Key)
            {
                isMatch = false;//yellowCharacter was on forbidden spot
            }
            else
            {
                bool fulfillsCharacter = false;
                for (int k = 0; k < lengthOfWords; k++)
                {
                    if (!usedSpots.Contains(k) && word.Value[k] == dict.Key) //spot unused so far && spot matches yellow letter
                    {
                        usedSpots.Add(k);//mark spot as used
                        fulfillsCharacter = true;
                        break;
                    }
                }
                if (!fulfillsCharacter)
                {
                    isMatch = false;
                }
            }

            if (!isMatch)
            {
                break;
            }
        }
        if (isMatch)
        {
            containYellowCharacters.Add(word);
        }
    }
    return containYellowCharacters;
}

List<string> ReadInput(string filename)
{
    StreamReader reader = new StreamReader(filename);

    List<string> inputList = new List<string>();
    while (!reader.EndOfStream)
    {
        inputList.Add(reader.ReadLine());
    }

    return inputList;
}
Word PickNextWord()
{
    words = ReduceList();
    if (words.Where(w => w.LengthWithoutDuplicates == lengthOfWords).ToList().Count > 0)
    {
        return words.Where(w => w.LengthWithoutDuplicates == lengthOfWords).Skip(0).First();
    }
    else
    {
        return words.Skip(0).First();
    }
}