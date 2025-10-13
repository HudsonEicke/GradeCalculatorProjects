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
            Console.WriteLine("Grade Calculator written by Hudson Eicke");
            Console.WriteLine("!!!WARNING!!!");
            Console.WriteLine("THIS CODE MAY NOT BE PERFECT AND COULD HAVE SOME OVERSITES THAT I DID NOT ACCOUNT FOR");
            Console.WriteLine("However it has worked for every case I have given it :)\n");

            Menu();

            Console.WriteLine("Thank you for using Grade Calculator I hope I was helpful");
            Console.WriteLine("I hope you get the grade you want :)");
        }

        static void Menu()
        {
            string input = "";

            while(input != "exit")
            {
                Console.Write("Enter 1 for creating a course template file 2 for creating a letter grade template file 3 for calculating a required grades from a course template file or exit to exit the grade calculator: ");
                input = Console.ReadLine();

                switch(input)
                {
                    case "exit":
                        return;

                    case "1":
                        CreateCourseTemplate();
                        break;

                    case "2":
                        CreateLetterGradeTemplateFile();
                        break;

                    case "3":
                        CalculateGrade();
                        break;

                    default:
                        Console.WriteLine("Invalid input please try again");
                        break;
                }
            }
        }

        static void CreateLetterGradeTemplateFile()
        {
            int inputInt;
            string input;
            Console.Write("Enter the name for the new letter grade template: ");
            input = Console.ReadLine();

            LetterGradeSet letterGradeSet = DefaultLetterEnterMode();

            string letterGradeTemplateFileName = input + ".txt";

            string newCourseLocation = Path.Combine(TEMPLATEFILEPATH, LETTERTEMPLATEFILENAME, letterGradeTemplateFileName);

            while (File.Exists(newCourseLocation))
            {
                Console.WriteLine($"You already have a template for a letter grade template named {input}");
                Console.Write("Would you like to 1 rename the current letter grade template or 2 overwrite the other letter grade template?: ");
                input = Console.ReadLine();

                while (!int.TryParse(input, out inputInt) || inputInt < 1 || inputInt > 2)
                {
                    Console.WriteLine("Invalid input please try again");
                    Console.Write("Would you like to 1 rename the current letter grade template or 2 overwrite the other letter grade template?: ");
                    input = Console.ReadLine();
                }

                if (input == "1")
                {
                    Console.Write("Enter the new name of the letter grade template: ");
                    input = Console.ReadLine();
                    letterGradeTemplateFileName = input + ".txt";
                    newCourseLocation = Path.Combine(TEMPLATEFILEPATH, LETTERTEMPLATEFILENAME, letterGradeTemplateFileName);
                }
                else
                    break;
            }

            //writes the letter grade template to a file
            using (StreamWriter templateFile = new StreamWriter(newCourseLocation))
            {
                templateFile.WriteLine(letterGradeSet);
                templateFile.Close();
            }
        }

        static void CreateCourseTemplate()
        {
            string input;
            int inputInt;

            Console.Write("Enter the name of the course: ");
            input = Console.ReadLine();

            CourseTemplate course = new CourseTemplate(input);

            Console.Write("Enter 1 if you want enter grades normally or 2 if you want to use a letter grade template file?: ");
            input = Console.ReadLine();

            while (!int.TryParse(input, out inputInt) || inputInt < 1 || inputInt > 2)
            {
                Console.WriteLine("Invalid selection please try again");
                Console.Write("Enter 1 if you want enter letter grades normally or 2 if you want to use a letter grade template file?: ");
                input = Console.ReadLine();
            }

            if (inputInt == 1)
            {
                course.SetLetterGrade(DefaultLetterEnterMode());
            }
            else if (inputInt == 2)
            {
                course.SetLetterGrade(FileEnterLetterEnterMode());
            }

            Console.Write("How many categories are in the class?: ");
            input = Console.ReadLine();

            //gets how many categories are in the class
            int categoryCount = 1;

            while (!int.TryParse(input, out categoryCount) || categoryCount < 1)
            {
                if(categoryCount < 1)
                {
                    Console.WriteLine("A course must have at least 1 category");
                    categoryCount = 1;
                }
                else
                {
                    Console.WriteLine("Invalid input please try again");
                }

                Console.Write("How many categories are in the class?: ");
                input = Console.ReadLine();
            }

            //reads in all the categories
            string categoryName;
            double categoryWeight;
            int assignmentCount = 1;

            Console.WriteLine("Now you will enter the information about the categories in the class");
            Console.WriteLine("When asked about a categories weight enter it in percentage form");
            Console.WriteLine("Example: if a category has a weight of 48% enter 48");

            for (int i = 0; i < categoryCount; i++)
            {
                //gets the category name
                Console.Write("Enter the name of the category: ");
                categoryName = Console.ReadLine();

                while (categoryName.Contains('|'))
                {
                    Console.WriteLine("Category names cannot contain the | character");
                    Console.Write("Enter the name of the category: ");
                    categoryName = Console.ReadLine();
                }

                //gets the weight of the category
                Console.Write($"Enter the weight of {categoryName}: ");
                input = Console.ReadLine();

                while (!double.TryParse(input, out categoryWeight))
                {
                    Console.WriteLine("Invalid input please try again");
                    Console.Write($"Enter the weight of {categoryName}: ");
                    input = Console.ReadLine();
                }

                //gets how many assignments the category has
                Console.Write("How many assignments does the category have?: ");
                input = Console.ReadLine();

                while(!int.TryParse(input, out assignmentCount) || assignmentCount < 1)
                {
                    if(assignmentCount < 1)
                    {
                        Console.WriteLine("A category must have at least 1 assignment");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input please try again");
                    }

                    Console.Write("How many assignments does the category have?: ");
                    input = Console.ReadLine();
                }

                //gets if the category has drops
                bool hasDrops;

                Console.Write("Does the category have dropped assignments? (y/n): ");
                input = Console.ReadLine();
                input = input.ToLower();

                while(input != "y" && input != "n")
                {
                    Console.WriteLine("Invalid input please try again");
                    Console.Write("Does the category have dropped assignments? (y/n): ");
                    input = Console.ReadLine();
                    input = input.ToLower();
                }

                //gets the drop count of the category if it has any
                int dropCount = 1;

                if (input == "y")
                {
                    hasDrops = true;

                    Console.Write("How many drops does the category have?: ");
                    input = Console.ReadLine();

                    while (!int.TryParse(input, out dropCount) || dropCount < 1)
                    {
                        if (dropCount < 1)
                        {
                            Console.WriteLine("A category with drops must have at least 1 drop");
                        }
                        else
                        {
                            Console.WriteLine("Invalid input please try again");
                        }

                        Console.Write("How many drops does the category have?: ");
                        input = Console.ReadLine();
                    }
                }
                else
                    hasDrops = false;

                //gets the difficulty of the category
                int difficulty;

                Console.Write("What is the difficulty of the category? (Higher value means more difficult): ");
                input = Console.ReadLine();

                while (!int.TryParse(input, out difficulty))
                {
                    Console.WriteLine("Invalid input please try again");
                    Console.Write("What is the difficulty of the category? (Higher value means more difficult): ");
                    input = Console.ReadLine();
                }

                //creates the category
                course.AddCategory(new Category(categoryName, categoryWeight, assignmentCount, hasDrops, dropCount, difficulty));
                Console.WriteLine();
            }

            //prepares to write the course
            string courseTemplateFileName = course.CourseName + ".txt";

            string newCourseLocation = Path.Combine(TEMPLATEFILEPATH, COURSETEMPLATEFILENAME, courseTemplateFileName);

            while (File.Exists(newCourseLocation))
            {
                Console.WriteLine($"You already have a template for a course named {course.CourseName}");
                Console.Write("Would you like to 1 rename the current course or 2 overwrite the other course?: ");
                input = Console.ReadLine();

                while(!int.TryParse(input, out inputInt) || inputInt < 1 || inputInt > 2)
                {
                    Console.WriteLine("Invalid input please try again");
                    Console.Write("Would you like to 1 rename the current course or 2 overwrite the other course?: ");
                    input = Console.ReadLine();
                }

                if (input == "1")
                {
                    Console.Write("Enter the new name of the course: ");
                    input = Console.ReadLine();
                    course.SetCourseName(input);
                    courseTemplateFileName = course.CourseName + ".txt";
                    newCourseLocation = Path.Combine(TEMPLATEFILEPATH, COURSETEMPLATEFILENAME, courseTemplateFileName);
                }
                else
                    break;
            }

            //writes the course template to a file

            using (StreamWriter templateFile = new StreamWriter(newCourseLocation))
            {
                templateFile.WriteLine(course.GetTemplateFile());
                templateFile.Close();
            }
        }

        static LetterGradeSet DefaultLetterEnterMode()
        {
            string input = "";
            double inputDouble;
            List<string> letters = new List<string>();
            double[] letterScores;

            //gets all of the letters for the course
            Console.WriteLine("When you are asked to enter letters enter them in decending order of score");
            Console.WriteLine("Example: A B C...");
            while (letters.Count == 0)
            {
                while (input != "0")
                {
                    Console.Write("Enter a letter(0 to stop): ");
                    input = Console.ReadLine();

                    if (input.Contains('|'))
                    {
                        Console.WriteLine("A letter grade cannot contain the | character");
                    }
                    else if (input != "0")
                    {
                        letters.Add(input);
                    }
                }

                if (letters.Count == 0)
                {
                    Console.WriteLine("A course needs at least 1 letter in it");
                }

                input = "";
            }

            //gets the scores for those letters
            Console.WriteLine("Now you will enter the score required to get each letter");
            Console.WriteLine("Enter these values in percentage form");
            Console.WriteLine("Example: if it takes a 90% to get an A enter 90");

            letterScores = new double[letters.Count];

            for (int i = 0; i < letters.Count; i++)
            {
                Console.Write($"Enter the percent needed for {letters[i]}: ");
                input = Console.ReadLine();

                while (!double.TryParse(input, out inputDouble))
                {
                    Console.WriteLine("Invalid value entered please try again");
                    Console.Write($"Enter the percent needed for {letters[i]}: ");
                    input = Console.ReadLine();
                }

                letterScores[i] = inputDouble;
            }

            return new LetterGradeSet(letters.ToArray(), letterScores);
        }

        static LetterGradeSet FileEnterLetterEnterMode()
        {
            string[] templates = Directory.GetFiles(Path.Combine(TEMPLATEFILEPATH, LETTERTEMPLATEFILENAME));
            char[] delims = { '.', '\\' };
            string? input;
            int fileIdx;

            if (templates.Length == 0)
            {
                Console.WriteLine("You do not have any letter grade templates :(");
                Console.WriteLine($"To use a letter grade template either create a template or if you already have one place it in {Path.Combine(TEMPLATEFILEPATH, LETTERTEMPLATEFILENAME)}");
                Console.WriteLine("Starting standard letter enter mode");
                return DefaultLetterEnterMode();
            }

            while (true)
            {
                Console.WriteLine("Which letter grade tempatle would you like to use?");

                //displays course options
                for (int i = 1; i <= templates.Length; i++)
                {
                    Console.WriteLine("[" + i + "] " + templates[i - 1].Split(delims)[2]);
                }
                Console.Write($"Enter a number from 1 to {templates.Length} or enter 0 to go to default enter mode: ");
                input = Console.ReadLine();

                if (input == "0")
                {
                    Console.WriteLine("Starting standard letter enter mode");
                    return DefaultLetterEnterMode();
                }

                while (!int.TryParse(input, out fileIdx) || fileIdx < 1 || fileIdx > templates.Length)
                {

                    Console.WriteLine("Invalid template file chosen");
                    Console.Write($"Enter a number from 1 to {templates.Length} or enter 0 to go to default enter mode: ");
                    input = Console.ReadLine();

                    if (input == "0")
                    {
                        Console.WriteLine("Starting standard letter enter mode");
                        return DefaultLetterEnterMode();
                    }
                }

                LetterGradeSet letterGradeSet;

                try
                {
                    letterGradeSet = new LetterGradeSet(templates[fileIdx - 1]);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    Console.WriteLine("Uh oh looks like the template file you chose is not setup correctly :(");
                    continue;
                }

                for (int i = 0; i < letterGradeSet.Letters.Length; i++)
                {
                    Console.Write($"{letterGradeSet.Letters[i]} >= {double.Round(letterGradeSet.LetterScores[i], 2, MidpointRounding.AwayFromZero)}%");

                    if (i != letterGradeSet.Letters.Length - 1)
                        Console.Write(" | ");
                }

                Console.Write("\nThe template has the above values do you want to this tempalte? (y/n): ");
                input = Console.ReadLine();
                input = input.ToLower();

                while(input != "y" && input != "n")
                {
                    Console.WriteLine("Invalid input please try again");
                    Console.Write("The template has the above values do you want to this tempalte? (y/n): ");
                    input = Console.ReadLine();
                    input = input.ToLower();
                }

                if(input == "y")
                    return letterGradeSet;
            }
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
            Console.WriteLine("When entering grades enter them in percentage form");
            Console.WriteLine("Example: if a someone got a 30/40 they should enter 75");

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
                        if (!scoreReport.CategoryScoreReports[i].HasDrops)
                            Console.Write($"{scoreReport.CategoryScoreReports[i].CategoryName} {j + 1}: {double.Round(scoreReport.CategoryScoreReports[i].Grades[j], 2, MidpointRounding.AwayFromZero)} | ");
                        else if (scoreReport.CategoryScoreReports[i].DropIdxes.Contains(j))
                            Console.Write($"{scoreReport.CategoryScoreReports[i].CategoryName} {j + 1}: !{double.Round(scoreReport.CategoryScoreReports[i].Grades[j], 2, MidpointRounding.AwayFromZero)}! | ");
                        else
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
