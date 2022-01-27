namespace WordleLibrary;

public class Solver
{
    public int LengthOfWords { get; set; }
    public string NameOfBaseWordlist { get; set; }
    public List<Word> Dictionary { get; set; }
    public Dictionary<Char, int> GreenCharacters { get; set; }
    public Dictionary<int, List<Char>> YellowCharacters { get; set; }
    public Dictionary<char, int> LetterCount { get; set; }
    public Dictionary<char, int> LetterCountMin { get; set; }
    public List<Char> GreyCharacters { get; set; }
    public Solver()
    {
        NameOfBaseWordlist = "words.txt";
        LengthOfWords = 5;

        //read words from textfiles
        List<string> list = ReadInput(NameOfBaseWordlist);
        Dictionary = Word.Convert(list);
        //Guessing that the dictionary is based on english letters, therefore testing letter with higher relative frequency first
        Dictionary = Dictionary.OrderByDescending(w => w.Wordrating).ToList();
        GreenCharacters = new();
        YellowCharacters = new();
        GreyCharacters = new List<Char>();
        LetterCount = new();
        LetterCountMin = new();
    }

    List<Word> ReduceList()
    {
        //1. remove blacklisted characters
        //Create a list with characters that are blacklisted, and are not on the yellow or green list
        List<Char> forbiddenCharacters = GreyCharacters.Where(g => !YellowCharacters.Any(y => y.Key == g) && !GreenCharacters.Any(y => y.Key == g)).ToList();
        List<Word> wordsWithoutBlacklisted = Dictionary.Where(w => !w.Value.Any(c => forbiddenCharacters.Contains(c))).ToList();

        //2. ensure known char-count is correct
        List<Word> wordsWithWrongLetterCount = new();
        foreach (var dict in LetterCount)
        {
            wordsWithWrongLetterCount.AddRange(wordsWithoutBlacklisted.Where(w => w.Value.Count(c => c == dict.Key) != dict.Value).ToList());
        }
        //3. ensure known min char-count is correct
        foreach (var dict in LetterCountMin)
        {
            wordsWithWrongLetterCount.AddRange(wordsWithoutBlacklisted.Where(w => w.Value.Count(c => c == dict.Key) < dict.Value).ToList());
        }
        List<Word> wordsWithCorrectLettercount = wordsWithoutBlacklisted.Except(wordsWithWrongLetterCount).ToList();

        //4. Ensure green characters are on the right spot
        List<Word> matchingGreenCharacters = new List<Word>();
        foreach (Word word in wordsWithCorrectLettercount)
        {
            bool isMatch = true;
            foreach (var dict in GreenCharacters)
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

        //5. ensure yellow are included, but not on the wrong spot & one spot can only be used once!
        List<Word> containYellowCharacters = new List<Word>();

        foreach (Word word in matchingGreenCharacters)
        {
            List<int> usedSpots = new List<int>();
            bool isMatch = true;
            
            foreach (var dict in YellowCharacters)
            {
                foreach (char y in dict.Value)
                {
                    if (word.Value[dict.Key] == y)
                    {
                        //Console.WriteLine($" - dropping word {word.Value} because char '{y}' was found on pos {dict.Key}");
                        isMatch = false;//forbidden char on position found -> no match
                        break;
                    }
                }
                if (!isMatch) { break; }

                List<char> allYellowLetters = YellowCharacters.SelectMany(y => y.Value).Distinct().ToList();
                string currentWord = word.Value;
                foreach (char y in allYellowLetters)
                {
                    int index = currentWord.IndexOf(y);
                    if (index >= 0)
                    {
                        //mark spot as used
                        currentWord = currentWord.Remove(index, 1).Insert(index, "_");
                    }
                    else
                    {
                        //Console.WriteLine($" - dropping word {word.Value} because char '{y}' was not found");
                        isMatch = false;//unused yellow char found -> no match
                        break;
                    }
                }

                if (!isMatch) { break; }
            }
            if (isMatch)
            {
                containYellowCharacters.Add(word);
            }
        }

        List<Word> droppedWords = Dictionary.Except(containYellowCharacters).ToList();

        return containYellowCharacters;
    }

    public void SetMatching(Word pickedWord, string matchInfo)
    {
        for (int i = 0; i < 5; i++)
        {
            //1. check if current letter is among the green or yellow ones -> Would mean we might know the number occurences
            int currentLetterCount = pickedWord.Value.Count(c => c == pickedWord.Value[i]);
            Char currentChar = pickedWord.Value[i];
            if (currentLetterCount > 1) //we only track occurences higher than 1
            {
                //Find occurences of current char
                List<int> foundIndexes = new List<int>();
                string markingsOfCurrentChar = string.Empty;
                for (int j = pickedWord.Value.IndexOf(currentChar); j > -1; j = pickedWord.Value.IndexOf(currentChar, j + 1))
                {
                    // for loop end when j=-1 (char not found)
                    foundIndexes.Add(j);
                    markingsOfCurrentChar += matchInfo[j];
                }
                if (markingsOfCurrentChar.Contains('b') && (markingsOfCurrentChar.Contains('g')|| markingsOfCurrentChar.Contains('y')))
                {
                    //since there is a grey match, we know the max number of occurences
                    //number of occurences, but only count green and yellow markings:
                    LetterCount[currentChar] = markingsOfCurrentChar.Count(c => c == 'g' || c == 'y');
                }
                else if(!markingsOfCurrentChar.Contains('b'))
                {
                    //since there are no grey matches, we know the min number of occurences
                    LetterCountMin[currentChar] = markingsOfCurrentChar.Count();
                }
            }

            //2. translate match info to the internal lists for g,y and b
            switch (matchInfo[i])
            {
                case 'g':
                    GreenCharacters[pickedWord.Value[i]] = i;
                    break;
                case 'y':
                    if (!YellowCharacters.ContainsKey(i))
                    {
                        YellowCharacters[i] = new List<char>();
                    }
                    if (!YellowCharacters[i].Contains(pickedWord.Value[i]))
                    {//not in there yet -> add
                        YellowCharacters[i].Add(pickedWord.Value[i]);
                    }
                    break;
                case 'b':
                    if (!LetterCount.ContainsKey(pickedWord.Value[i]))//current letter occures -> ignore this gray match for now
                    {
                        if (!GreyCharacters.Contains(pickedWord.Value[i]))//is already in list, do not add again
                        {//new -> add
                            GreyCharacters.Add(pickedWord.Value[i]);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public Word PickNextWord()
    {
        Dictionary = ReduceList();
        if (Dictionary.Where(w => w.LengthWithoutDuplicates == LengthOfWords).ToList().Count > 0)
        {
            //as long as possible pick a word without double occurences of letters. To eliminate letters faster
            return Dictionary.Where(w => w.LengthWithoutDuplicates == LengthOfWords).Skip(0).First();
        }
        else
        {
            return Dictionary.Skip(0).First();
        }
    }
    public static List<string> ReadInput(string filename)
    {
        StreamReader reader = new StreamReader(filename);

        List<string> inputList = new List<string>();
        while (!reader.EndOfStream)
        {
            inputList.Add(reader.ReadLine());
        }

        return inputList;
    }
}
