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

        public int Wordrating(Dictionary<char, int> letterfrequency) 
        {
            int rating = 0;

            foreach (char c in word)
            {
                rating += letterfrequency[c];
            }
            return rating;            
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