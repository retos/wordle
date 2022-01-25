namespace WordleLibrary;

public class Solver
{
    public int LengthOfWords { get; set; }
    public string NameOfBaseWordlist { get; set; }
    public string NameOfUnknownWordslist { get; set; }
    public List<Word> Dictionary { get; set; }
    public List<string> WordsToIgnore { get; set; }
    public Dictionary<Char, int> GreenCharacters { get; set; }
    public Dictionary<Char, int> YellowCharacters { get; set; }
    public List<Char> GreyCharacters { get; set; }
    public Solver()
    {
        NameOfBaseWordlist = "words_alpha.txt";
        NameOfUnknownWordslist = "unknownWords.txt";
        LengthOfWords = 5;

        //read words from textfiles
        List<string> list = ReadInput(NameOfBaseWordlist);
        WordsToIgnore = ReadInput(NameOfUnknownWordslist);
        //only use the words with the desired length
        list = list.Where(e => e.Count() == LengthOfWords).ToList();

        Dictionary = Word.Convert(list);
        GreenCharacters = new();
        YellowCharacters = new();
        GreyCharacters = new List<Char>();
    }
    List<Word> ReduceList()
    {
        //remove skipped words from wordlist
        Dictionary = Dictionary.Where(e => !WordsToIgnore.Contains(e.Value)).ToList();

        //remove blacklisted characters
        //Create a list with characters that are blacklisted, and are not on the yellow or green list
        List<Char> forbiddenCharacters = GreyCharacters.Where(g => !YellowCharacters.Any(y => y.Key == g) && !GreenCharacters.Any(y => y.Key == g)).ToList();
        List<Word> wordsWithoutBlacklisted = Dictionary.Where(w => !w.Value.Any(c => forbiddenCharacters.Contains(c))).ToList();

        //Ensure green characters are on the right spot
        List<Word> matchingGreenCharacters = new List<Word>();
        foreach (Word word in wordsWithoutBlacklisted)
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

        //ensure yellow are included, but not on the wrong spot & one spot can only be used once!
        List<Word> containYellowCharacters = new List<Word>();

        foreach (Word word in matchingGreenCharacters)
        {
            List<int> usedSpots = new List<int>();
            bool isMatch = true;
            foreach (var dict in YellowCharacters)
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
                    for (int k = 0; k < LengthOfWords; k++)
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


    public Word PickNextWord()
    {
        Dictionary = ReduceList();
        if (Dictionary.Where(w => w.LengthWithoutDuplicates == LengthOfWords).ToList().Count > 0)
        {
            return Dictionary.Where(w => w.LengthWithoutDuplicates == LengthOfWords).Skip(0).First();
        }
        else
        {
            return Dictionary.Skip(0).First();
        }
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
}
