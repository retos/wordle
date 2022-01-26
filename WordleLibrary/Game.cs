using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordleLibrary
{
    public class Game
    {
        private string hiddenWord;

        public Game(string entry)
        {
            this.hiddenWord = entry;
        }

        public string Guess(Word pickedWord)
        {
            string matchingResult = "     ";
            string target = hiddenWord;

            //first mark all the exact matches green
            for (int position = 0; position < 5; position++)
            {
                Char currentChar = pickedWord.Value[position];
                if (target[position] == currentChar)
                {
                    //mark spot as used
                    target = target.Remove(position, 1).Insert(position, "_");
                    //mark letter as exact match
                    matchingResult = matchingResult.Remove(position, 1).Insert(position, "g");
                }
            }

            //second mark all the matches on other spot yellow
            for (int position = 0; position < 5; position++)
            {               
                Char currentChar = pickedWord.Value[position];
                int positionOfCurrentCharInHiddenWord = target.IndexOf(currentChar);
                if (positionOfCurrentCharInHiddenWord >= 0 && target[positionOfCurrentCharInHiddenWord] != '_' && matchingResult[position] != 'g')
                {
                    //mark spot as used
                    target = target.Remove(positionOfCurrentCharInHiddenWord, 1).Insert(positionOfCurrentCharInHiddenWord, "_");
                    //mark letter as found
                    matchingResult = matchingResult.Remove(position, 1).Insert(position, "y");
                }                
            }

            //mark remaining spots as black
            matchingResult = matchingResult.Replace(" ", "b");

            return matchingResult;
        }
    }
}
