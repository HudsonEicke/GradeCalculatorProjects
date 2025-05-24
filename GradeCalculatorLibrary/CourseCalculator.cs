using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeCalculatorLibrary
{
    public class CourseCalculator : Course
    {
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

            return true;
        }

        public bool ResetScore(int categoryNum, int assignmentNum)
        {
            if (categoryNum < 0 || categoryNum >= Categories.Count())
                return false;

            return Categories[categoryNum].ResetScore(assignmentNum);
        }

        public void Calculate()
        {
            List<(int categoryNum, int difficulty)> unEnteredCategories = new List<(int categoryNum, int difficulty)>();
            double grade = 0;


            for (int i = 0; i < Categories.Count; i++)
            {
                //recalculates to get the most accurate grade
                Categories[i].RecalculateScore();

                //check if any grades can be changed in the category
                if (Categories[i].AtMaxWeight() || Categories[i].AllEntered())
                {
                    grade += Categories[i].Weight;
                    continue;
                }

                unEnteredCategories.Add((i, Categories[i].Difficulty));
            }

            if(unEnteredCategories.Count == 0)
            {
                //ADD LOGIC FOR NO POSSIBLE CHANGE
            }

            unEnteredCategories.Sort((category1, category2) => category1.difficulty.CompareTo(category2.difficulty));

            for (int i = 0; i < unEnteredCategories.Count; i++)
            {
                
            }
        }
    }
}
