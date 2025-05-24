//maybe add something to show that a score was calculated

using GradeCalculatorLibrary;

namespace GradeCalculatorTerminal
{
    internal class Program
    {
        const string TEMPLATEFILEPATH = @"Templates";
        const string COURSETEMPLATEFILENAME = @"CourseTemplates";
        const string LETTERTEMPLATEFILENAME = @"LetterGradeTemplates";

        static void Main(string[] args)
        {
            //first time setup
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
            //end of first time setup


            CalculateGrade();
        }

        static void CreateTemplateFile()
        {

        }

        static void CalculateGrade()
        {
            string[] templates = Directory.GetFiles(@"Templates\CourseTemplates");
            char[] delims = { '.', '\\' };
            string? input;
            bool breakAndContinue = false;
            double parseVal;
            int fileIdx;
            int letterIdx;

            //if there are no template files
            if (templates.Length == 0)
            {
                Console.WriteLine("You do not have any course templates :(");
                Console.WriteLine("To calculate a grade please either create a template or if you already have one place it in Templates\\CourseTemplates");
                return;
            }

            Console.WriteLine("Which course would you like to calculate for?");

            //displays course options
            for (int i = 1; i <= templates.Length; i++)
            {
                Console.WriteLine("[" + i + "] " + templates[i - 1].Split(delims)[2]);
            }

            Console.Write($"Enter a number from 1 to {templates.Length}: ");
            input = Console.ReadLine();

            //checks if a valid index was chosen
            while (!int.TryParse(input, out fileIdx) || fileIdx < 1 || fileIdx > templates.Length)
            {
                Console.WriteLine("Invalid template file chosen");
                Console.Write($"Enter a number from 1 to {templates.Length}: ");
                input = Console.ReadLine();
            }

            CourseCalculator course;

            try
            {
                course = new CourseCalculator(templates[fileIdx - 1]);
            }
            catch (Exception exception) //bad template file case
            {
                Console.WriteLine(exception.Message);
                Console.WriteLine("Uh oh looks like the template file you chose is not setup correctly :(");
                return;
            }

            Console.WriteLine($"\nThe percentage available for {course.CourseName} is: {double.Round(course.MaxScore, 2, MidpointRounding.AwayFromZero)}%\n");

            Console.WriteLine($"Categories and weight for {course.CourseName}");

            for (int i = 0; i < course.Categories.Count(); i++)
            {
                Console.WriteLine($"{course.Categories[i].Name}: {double.Round(course.Categories[i].Weight, 2, MidpointRounding.AwayFromZero)}%");
            }

            Console.WriteLine($"\nGrading scale for {course.CourseName}");

            for (int i = 0; i < course.LetterGrades.Letters.Length; i++)
            {
                Console.Write($"{course.LetterGrades.Letters[i]} >= {double.Round(course.LetterGrades.LetterScores[i], 2, MidpointRounding.AwayFromZero)}%");

                if (i != course.LetterGrades.Letters.Length - 1)
                    Console.Write(" | ");
            }

            Console.WriteLine();

            Console.WriteLine("\nPlease fill as many of the following grades in as you would like");
            Console.WriteLine("For any grades you want the calculator to estimate on just press enter instead of filling in a value");

            //for each category
            for (int i = 0; i < course.Categories.Count(); i++)
            {
                //if there is only one assignment in the category
                if (course.Categories[i].AssignmentCount == 1)
                {
                    Console.Write($"Enter your grade for {course.Categories[i].Name}: ");
                    input = Console.ReadLine();

                    //if the user wants the grade calculator to handle this one
                    if (input.Length == 0)
                        continue;

                    //invalid input loop
                    while (!double.TryParse(input, out parseVal))
                    {
                        Console.WriteLine("Invalid input please try again");
                        Console.Write($"Enter your grade for {course.Categories[i].Name}: ");
                        input = Console.ReadLine();

                        //if the user wants the grade calculator to handle this one
                        if (input.Length == 0)
                        {
                            breakAndContinue = true;
                            break;
                        }
                    }

                    if (breakAndContinue)
                    {
                        breakAndContinue = false;
                        continue;
                    }

                    //set the score
                    course.SetScore(i, 0, parseVal);
                }
                else
                {
                    //for each assignment in the category
                    for (int j = 0; j < course.Categories[i].AssignmentCount; j++)
                    {
                        Console.Write($"Enter your grade for { course.Categories[i].Name} {j + 1}: ");
                        input = Console.ReadLine();

                        //if the user wants the grade calculator to handle this one
                        if (input.Length == 0)
                            continue;

                        while (!double.TryParse(input, out parseVal))
                        {
                            Console.WriteLine("Invalid input please try again");
                            Console.Write($"Enter your grade for {course.Categories[i].Name} {j + 1}: ");
                            input = Console.ReadLine();

                            //if the user wants the grade calculator to handle this one
                            if (input.Length == 0)
                            {
                                breakAndContinue = true;
                                break;
                            }
                        }

                        if (breakAndContinue)
                        {
                            breakAndContinue = false;
                            continue;
                        }

                        //set the score
                        course.SetScore(i, j, parseVal);
                    }
                }

            }

            Console.WriteLine($"\nYour current true grade is: {double.Round(course.TrueGrade, 2, MidpointRounding.AwayFromZero)}%");

            Console.WriteLine();
            ScoreReport[] neededScores = course.Calculate();

            while (true)
            {
                Console.WriteLine("Which letter would you like to see the calculated grade for?");

                for (int i = 0; i < course.LetterGrades.Letters.Length; i++)
                {
                    Console.WriteLine($"[{i + 1}] {course.LetterGrades.Letters[i]}");
                }

                Console.Write($"Enter a number from 1 to {course.LetterGrades.Letters.Count()} or type exit to return to the main menue: ");
                input = Console.ReadLine();

                if (input.ToLower().Equals("exit"))
                    return;

                //checks if a valid index was chosen
                while (!int.TryParse(input, out letterIdx) || letterIdx < 1 || letterIdx > course.LetterGrades.Letters.Count())
                {
                    Console.WriteLine("Invalid letter chosen");
                    Console.Write($"Enter a number from 1 to {course.LetterGrades.Letters.Count()} or type exit to return to the main menue: ");
                    input = Console.ReadLine();

                    if (input.ToLower().Equals("exit"))
                        return;
                }

                DisplayScoreReport(neededScores[letterIdx - 1]);
            }
        }

        //displays a score report
        static void DisplayScoreReport(ScoreReport scoreReport)
        {
            if (scoreReport == null)
            {
                Console.WriteLine("Unfortunately this score is unobtainable :(");
                return;
            }

            Console.WriteLine($"Score required to get a {scoreReport.ScoreReportLetter}");

            for (int i = 0; i < scoreReport.CategoryScoreReports.Count(); i++)
            {
                if(scoreReport.CategoryScoreReports[i].Grades.Length == 1)
                {
                    Console.Write($"{scoreReport.CategoryScoreReports[i].CategoryName}: {double.Round(scoreReport.CategoryScoreReports[i].Grades[0], 2, MidpointRounding.AwayFromZero)}");
                }
                else
                {
                    for (int j = 0; j < scoreReport.CategoryScoreReports[i].Grades.Length; j++)
                    {
                        Console.Write($"{scoreReport.CategoryScoreReports[i].CategoryName} {j + 1}: {double.Round(scoreReport.CategoryScoreReports[i].Grades[j], 2, MidpointRounding.AwayFromZero)} | ");
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine("Press enter to return to the letter selection menue");
            Console.ReadLine();
        }
    }
}
