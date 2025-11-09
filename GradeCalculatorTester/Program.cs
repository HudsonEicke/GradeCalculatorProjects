/*
    SHOULD NOT BE USED TO TEST ANYMORE AS THE FILE IS VERY OUTDATED
*/

using GradeCalculatorLibrary;

namespace GradeCalculatorTester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (!Directory.Exists(@"Templates"))
            {
                Directory.CreateDirectory(@"Templates");
            }

            if (!Directory.Exists(@"Templates\LetterGradeTemplates"))
            {
                Directory.CreateDirectory(@"Templates\LetterGradeTemplates");
            }

            if (!Directory.Exists(@"Templates\CourseTemplates"))
            {
                Directory.CreateDirectory(@"Templates\CourseTemplates");
            }

            string courseName = "COP3402";

            CourseTemplate test = new CourseTemplate(courseName);
            test.SetLetterGrade(new LetterGradeSet("A|B+|B|C+|C|D|F", "0.9|0.87|0.8|0.77|0.7|0.6|0"));
            test.AddCategory(new Category("Homework|0.2|24|True|4|1"));
            test.AddCategory(new Category("Exercises|0.08|2|False|2"));
            test.AddCategory(new Category("Projects|0.48|6|False|3"));
            test.AddCategory(new Category("Midterm|0.08|1|False|4"));
            test.AddCategory(new Category("Attendance|0.04|1|False|1"));
            test.AddCategory(new Category("Bonus Project|0.03|1|False|5"));
            test.AddCategory(new Category("Bonus Attendance|0.03|1|False|1"));
            test.AddCategory(new Category("Final|0.12|1|False|5"));
            
            Console.WriteLine(test.GetTemplateFile());

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(@"Templates\CourseTemplates\", "COP3402.txt")))
            {
                outputFile.WriteLine(test.GetTemplateFile());
                outputFile.Close();
            }
            //Console.WriteLine("LetterGradeSet unit test cases");
            //LetterGradeSetUnitTests();
        }

        public static void LetterGradeSetUnitTests()
        {
            char[] delims = { '.', '\\' };
            string folderName = @"Templates\LetterGradeTemplates";
            LetterGradeSet letterGrades;
            int templateFileCaseCount = 5;

            //proccessed array test cases
            Console.WriteLine("Proccessed array test cases\n");

            Console.Write("Test case 1: ");
            try
            {
                string[] proccessedLetters = { "A", "B", "C", "D", "F" };
                double[] proccessedScores = { 40, 30, 20, 10 };

                letterGrades = new LetterGradeSet(proccessedLetters, proccessedScores);

                Console.WriteLine("Fail");
            }
            catch (Exception)
            {
                Console.WriteLine("Pass");
            }

            Console.Write("Test case 2: ");
            try
            {
                string[] proccessedLetters = { "A", "B", "C", "D", "F" };
                double[] proccessedScores = { 40, 30, 20, 10, 0};

                letterGrades = new LetterGradeSet(proccessedLetters, proccessedScores);

                Console.WriteLine("Pass");
            }
            catch (Exception)
            {
                Console.WriteLine("Fail");
            }



            //2 strings test cases
            Console.WriteLine("\n2 strings test cases\n");

            Console.Write("Test case 1: ");
            try
            {
                string letters = "A|B|C|D|F";
                string scores = "40|30|20|10";

                letterGrades = new LetterGradeSet(letters, scores);

                Console.WriteLine("Fail");
            }
            catch (Exception)
            {
                Console.WriteLine("Pass");
            }

            Console.Write("Test case 2: ");
            try
            {
                string letters = "A|B|C|D|F";
                string scores = "40|30|20|10|A";

                letterGrades = new LetterGradeSet(letters, scores);

                Console.WriteLine("Fail");
            }
            catch (Exception)
            {
                Console.WriteLine("Pass");
            }

            Console.Write("Test case 3: ");
            try
            {
                string letters = "A|B|C|D|F";
                string scores = "40|30|20|10|0";

                letterGrades = new LetterGradeSet(letters, scores);

                Console.WriteLine("Pass");
            }
            catch (Exception)
            {
                Console.WriteLine("Fail");
            }



            //template file test cases
            Console.WriteLine("\nTemplate file test cases\n");

            Console.Write("Test case 1: ");
            try
            {
                letterGrades = new LetterGradeSet(folderName + "BogusFileName.txt");

                Console.WriteLine("Fail");
            }
            catch (Exception)
            {
                Console.WriteLine("Pass");
            }

            for (int i = 2; i < templateFileCaseCount; i++)
            {
                Console.Write($"Test case {i}: ");
                try
                {
                    letterGrades = new LetterGradeSet(folderName + $"\\LetterGradeFileTestCase{i}.txt");

                    Console.WriteLine("Fail");
                }
                catch (Exception)
                {
                    Console.WriteLine("Pass");
                }
            }

            Console.Write("Test case 5: ");
            try
            {
                letterGrades = new LetterGradeSet(folderName + "\\LetterGradeFileTestCase5.txt");

                Console.WriteLine("Pass");
            }
            catch (Exception)
            {
                Console.WriteLine("Fail");
            }
        }
    }
}
