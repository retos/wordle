namespace WordleLibrary
{
    public class Word
    {
        private string word;
        private int lengthWithoutDuplicates;

        public Word(string entry)
        {
            this.word = entry;
            this.lengthWithoutDuplicates = string.Join("", new HashSet<char>(entry)).Length;
        }
        public int LengthWithoutDuplicates
        {
            get
            {
                return lengthWithoutDuplicates;
            }
        }

        public string Value
        {
            get
            {
                return word;
            }
            set
            {
                word = value.Trim();
            }
        }

        public object Wordrating 
        {
            get
            {
                int rating = 0;
                //https://en.wikipedia.org/wiki/Letter_frequency#:~:text=top%20eight%20characters.-,Relative%20frequencies%20of%20letters%20in%20the%20English%20language,%2C%20Q%2C%20X%2C%20Z.
                Dictionary<char, int> letterfrequency = new Dictionary<char, int>();
                letterfrequency.Add('a', 82);
                letterfrequency.Add('b', 15);
                letterfrequency.Add('c', 28);
                letterfrequency.Add('d', 43);
                letterfrequency.Add('e', 130);
                letterfrequency.Add('f', 22);
                letterfrequency.Add('g', 20);
                letterfrequency.Add('h', 61);
                letterfrequency.Add('i', 70);
                letterfrequency.Add('j', 2);
                letterfrequency.Add('k', 8);
                letterfrequency.Add('l', 40);
                letterfrequency.Add('m', 25);
                letterfrequency.Add('n', 67);
                letterfrequency.Add('o', 75);
                letterfrequency.Add('p', 19);
                letterfrequency.Add('q', 1);
                letterfrequency.Add('r', 60);
                letterfrequency.Add('s', 63);
                letterfrequency.Add('t', 91);
                letterfrequency.Add('u', 28);
                letterfrequency.Add('v', 10);
                letterfrequency.Add('w', 24);
                letterfrequency.Add('x', 2);
                letterfrequency.Add('y', 20);
                letterfrequency.Add('z', 1);

                foreach (char c in word)
                {
                    rating += letterfrequency[c];
                }
                return rating;
            }
        }

        internal static List<Word> Convert(List<string> list)
        {
            List<Word> result = new List<Word>();
            foreach (string entry in list)
            {
                result.Add(new Word(entry));
            }
            return result;
        }
    }
}