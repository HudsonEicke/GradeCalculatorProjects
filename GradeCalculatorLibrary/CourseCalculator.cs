using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeCalculatorLibrary
{
    public class CourseCalculator : Course
    {
        public double MaxScore { get; private set; }
        public double TrueGrade { get; private set; }
        private const double _PRECISION = 0.01;


        //builds a course from a template
        public CourseCalculator(string filePath)
        {
            string ? currentLine;
            string? currentLine2;
            int categoryCount;

            try
            {
                //opens the template file
                StreamReader templateFileReader = new StreamReader(filePath);

                //reads the name of the course
                currentLine = templateFileReader.ReadLine();
                CourseName = currentLine;

                currentLine = templateFileReader.ReadLine();
                currentLine2 = templateFileReader.ReadLine();

                LetterGrades = new LetterGradeSet(currentLine, currentLine2);

                currentLine = templateFileReader.ReadLine();

                if (!int.TryParse(currentLine, out categoryCount))
                {
                    throw new CourseException("Invalid course template file: Invalid value in the category count field");
                }

                for (int i = 0; i < categoryCount; i++)
                {
                    currentLine = templateFileReader.ReadLine();
                    Categories.Add(new Category(currentLine));
                    MaxScore += Categories[i].Weight;
                }
            }
            catch (FileNotFoundException)
            {
                //invalid path case
                throw new CourseException("Invalid course template file: Invalid template file path");
            }
        }

        public bool SetScore(int categoryNum, int assignmentNum, double score)
        {
            if(categoryNum < 0 || categoryNum >= Categories.Count())
                return false;

            if(!Categories[categoryNum].SetScore(assignmentNum, score))
                return false;

            UpdateTrueGrade();

            return true;
        }

        public bool ResetScore(int categoryNum, int assignmentNum)
        {
            if (categoryNum < 0 || categoryNum >= Categories.Count())
                return false;

            UpdateTrueGrade();

            return Categories[categoryNum].ResetScore(assignmentNum);
        }

        public ScoreReport[] Calculate()
        {
            return CalculateNoDrops();
        }

        private void UpdateTrueGrade()
        {
            TrueGrade = 0;

            for (int i = 0; i < Categories.Count; i++)
            {
                TrueGrade += Categories[i].ObtainedScore;
            }
        }

        private ScoreReport[] CalculateNoDrops()
        {
            List<(int categoryNum, int difficulty)> unEnteredCategories = new List<(int categoryNum, int difficulty)>();
            ScoreReport[] scoreReports = new ScoreReport[LetterGrades.Letters.Length];
            CategoryScoreReport[] categoryScoreReports = new CategoryScoreReport[Categories.Count()];
            double grade = 0;
            int lettersObtained = 0;


            for (int i = 0; i < Categories.Count; i++)
            {
                //recalculates to get the most accurate grade
                Categories[i].RecalculateScore();
                categoryScoreReports[i] = new CategoryScoreReport(Categories[i]);

                //check if any grades can be changed in the category
                if (Categories[i].AllEntered())
                {
                    grade += Categories[i].ObtainedScore;
                    continue;
                }

                unEnteredCategories.Add((i, Categories[i].Difficulty));
            }

            //finds all letter grades already obtained
            for (int i = LetterGrades.Letters.Count() - 1; i >= 0; i--)
            {
                if (grade >= LetterGrades.LetterScores[i])
                {
                    scoreReports[i] = new ScoreReport(LetterGrades.Letters[i], categoryScoreReports);
                    lettersObtained++;
                }
                else
                    break;
            }

            //if all already obtained
            if (lettersObtained == LetterGrades.Letters.Count())
                return scoreReports;

            //if not all letters are obtainable
            if (unEnteredCategories.Count == 0)
            {
                for(int i = LetterGrades.Letters.Length - lettersObtained - 1; i >= 0; i--)
                {
                    scoreReports[i] = null;
                }

                return scoreReports;
            }
            //sorts the difficulties in ascending order
            unEnteredCategories.Sort((category1, category2) => category1.difficulty.CompareTo(category2.difficulty));

            //calculates the grades needed for the remaining letters
            for (int i = 0; i < unEnteredCategories.Count; i++)
            {
                //get the idx of the category
                int idx = unEnteredCategories[i].categoryNum;

                double currVal = _PRECISION;

                //check if we are not at the last category
                if (i != unEnteredCategories.Count - 1)
                {
                    //gets the indexes that have not been filled yet
                    List<int> ? missingGrades = Categories[idx].GetUnenteredIdxes();

                    //runs for each value from 0 to 100 with an increment of PRECISION
                    while (currVal <= 100)
                    {
                        //for each unentered grade
                        for (int j = 0; j < missingGrades.Count(); j++)
                        {
                            //increase the index by PRECISION
                            Categories[idx].UpdateScore(missingGrades[j], _PRECISION);

                            //see if a new letter is available
                            if (IsNewLetterAvailable(grade, lettersObtained, idx))
                            {
                                //generate an updated category score report
                                categoryScoreReports[idx] = new CategoryScoreReport(Categories[idx]);

                                int amtToAdd = 0;

                                //gets all the new letters
                                for (int k = LetterGrades.Letters.Count() - lettersObtained - 1; k >= 0; k--)
                                {
                                    if (grade + Categories[idx].ObtainedScore >= LetterGrades.LetterScores[k])
                                    {
                                        scoreReports[k] = new ScoreReport(LetterGrades.Letters[k], categoryScoreReports);
                                        amtToAdd++;
                                    }
                                    else
                                        break;
                                }

                                lettersObtained += amtToAdd;
                                
                                //if all obtained
                                if (lettersObtained == LetterGrades.Letters.Count())
                                    return scoreReports;
                            }
                        }

                        currVal += _PRECISION;
                    }

                    for (int j = 0; j < missingGrades.Count(); j++)
                    {
                        Categories[idx].SetScore(missingGrades[j], 100);
                    }

                    Categories[idx].RecalculateScore();

                    //generate an updated category score report
                    categoryScoreReports[idx] = new CategoryScoreReport(Categories[idx]);
                }
                else
                {
                    //gets the indexes that have not been filled yet
                    List<int>? missingGrades = Categories[idx].GetUnenteredIdxes();

                    //runs until all letters are obtained
                    while (lettersObtained != LetterGrades.Letters.Count())
                    {
                        //for each unentered grade
                        for (int j = 0; j < missingGrades.Count(); j++)
                        {
                            //increase the index by PRECISION
                            Categories[idx].UpdateScore(missingGrades[j], _PRECISION);

                            //see if a new letter is available
                            if (IsNewLetterAvailable(grade, lettersObtained, idx))
                            {
                                //generate an updated category score report
                                categoryScoreReports[idx] = new CategoryScoreReport(Categories[idx]);

                                int amtToAdd = 0;

                                //gets all the new letters
                                for (int k = LetterGrades.Letters.Count() - lettersObtained - 1; k >= 0; k--)
                                {
                                    if (grade + Categories[idx].ObtainedScore >= LetterGrades.LetterScores[k])
                                    {
                                        scoreReports[k] = new ScoreReport(LetterGrades.Letters[k], categoryScoreReports);
                                        amtToAdd++;
                                    }
                                    else
                                        break;
                                }

                                lettersObtained += amtToAdd;

                                //if all obtained
                                if (lettersObtained == LetterGrades.Letters.Count())
                                    return scoreReports;
                            }
                        }
                    }
                }

                //increase the current grade properly
                grade += Categories[idx].ObtainedScore;
            }

            return scoreReports;
        }

        //checks if a new letter grade is available
        private bool IsNewLetterAvailable(double currGrade, int lettersObtained, int categoryIdx)
        {
            if(LetterGrades == null)
                return false;

            return currGrade + Categories[categoryIdx].ObtainedScore >= LetterGrades.LetterScores[LetterGrades.LetterScores.Length - lettersObtained - 1];
        }
    }
}
