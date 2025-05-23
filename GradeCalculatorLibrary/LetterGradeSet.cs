using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GradeCalculatorLibrary
{
    public class LetterGradeSet
    {
        public string[] Letters { get; private set; }
        public double[] LetterScores { get; private set; }

        //already proccessed arrays 
        public LetterGradeSet(string[] letters, double[] letterScores)
        {
            Letters = letters;
            LetterScores = letterScores;

            //if there is not an equal amount of scores and letters
            if (LetterScores.Length != Letters.Length)
                throw new LetterGradeException("Invalid letter grade strings: The letter grade strings must have an equal amount of letters and letter scores");
        }

        //using 2 strings
        public LetterGradeSet(string letters, string letterScores)
        {
            Letters = letters.Split('|');

            //prepares the letter scores to be converted
            string[] letterScoresStringSplit = letterScores.Split('|');

            //if there is not an equal amount of scores and letters
            if (letterScoresStringSplit.Length != Letters.Length)
                throw new LetterGradeException("Invalid letter grade strings: The letter grade strings must have an equal amount of letters and letter scores");

            LetterScores = new double[letterScoresStringSplit.Length];

            //converts all values from string form to double form
            for (int i = 0; i < Letters.Length; i++)
            {
                //if invalid value in the letter score string
                if (!double.TryParse(letterScoresStringSplit[i], out LetterScores[i]))
                    throw new LetterGradeException("Invalid letter score string: The letter scores may only be numbers");

                //rounds number to 2 decimal places
                LetterScores[i] = double.Round(LetterScores[i], 2);
            }
        }

        //using a letter grade template file
        public LetterGradeSet(string letterGradeTemplateFilePath)
        {
            string ? lettersString;
            string ? letterScoresString;

            try
            {
                //opens the template file
                StreamReader templateFileReader = new StreamReader(letterGradeTemplateFilePath);

                //read the first 2 lines of the file
                lettersString = templateFileReader.ReadLine();
                letterScoresString = templateFileReader.ReadLine();
            }
            catch (Exception)
            {
                //invalid path case
                throw new LetterGradeTemplateException("Invalid letter grade template file: Invalid template file path");
            }

            //if the file does not contain at least 2 lines
            if (lettersString == null || letterScoresString == null)
                throw new LetterGradeTemplateException("Invalid letter grade template file: The letter grade template file must contain at least 2 lines");

            //converts the first line of teh file to letters
            Letters = lettersString.Split('|');

            //prepares the second line to be converted
            string[] letterScoresStringSplit = letterScoresString.Split('|');
            
            //if there is not an equal amount of scores and letters
            if(letterScoresStringSplit.Length != Letters.Length)
                throw new LetterGradeTemplateException("Invalid letter grade template file: The letter grade template file must have an equal amount of letters and letter scores");

            LetterScores = new double[letterScoresStringSplit.Length];

            //converts all values from string form to double form
            for (int i = 0; i < Letters.Length; i++)
            {
                //if invalid value in the letter grade file
                if(!double.TryParse(letterScoresStringSplit[i], out LetterScores[i]))
                    throw new LetterGradeTemplateException("Invalid letter grade template file: The letter scores may only be numbers");

                //rounds number to 2 decimal places
                LetterScores[i] = double.Round(LetterScores[i], 2);
            }
        }

        //checks if all letters are obtained
        public bool ObtainedAll(double currentScore)
        {
            if(currentScore >= LetterScores[0])
                return true;

            return false;
        }

        //counts how many letter scores have been obtained at the given score
        public int ObtainedCount(double currentScore)
        {
            for (int i = 0; i < LetterScores.Length; i++)
            {
                //finds the best letter we can get
                if (LetterScores[i] >= currentScore)
                    return LetterScores.Length - i;
            }

            return 0;
        }

        //returns a list of all letters obtained
        public List<string> GetObtainedLetters(double currentScore)
        {
            List<string> obtainedLetters = new List<string>();
            
            for (int i = 0; i < LetterScores.Length; i++)
            {
                //finds the best letter we can get
                if(currentScore >= LetterScores[i])
                {
                    //add all other letters after it to bypass unneeded comparisons
                    for (int j = i; j < Letters.Length; j++)
                        obtainedLetters.Add(Letters[j]);

                    break;
                }
            }

            return obtainedLetters;
        }

        //used for template file building
        public override string ToString()
        {
            StringBuilder letterGradeSetString = new StringBuilder();

            for (int i = 0; i < Letters.Length; i++)
            {
                letterGradeSetString.Append(Letters[i]);
                letterGradeSetString.Append('|');
            }

            letterGradeSetString.Remove(letterGradeSetString.Length - 1, 1);
            letterGradeSetString.Append('\n');

            for (int i = 0; i < LetterScores.Length; i++)
            {
                letterGradeSetString.Append(LetterScores[i]);
                letterGradeSetString.Append('|');
            }

            letterGradeSetString.Remove(letterGradeSetString.Length - 1, 1);
            letterGradeSetString.Append('\n');

            return letterGradeSetString.ToString();
        }
    }

    public class LetterGradeTemplateException : Exception
    {
        public LetterGradeTemplateException()
        {
        }

        public LetterGradeTemplateException(string message) : base(message)
        {
        }

        public LetterGradeTemplateException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class LetterGradeException : Exception
    {
        public LetterGradeException()
        {
        }

        public LetterGradeException(string message) : base(message)
        {
        }

        public LetterGradeException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
