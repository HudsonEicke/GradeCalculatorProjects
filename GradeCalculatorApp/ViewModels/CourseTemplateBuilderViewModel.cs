using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GradeCalculatorLibrary;

namespace GradeCalculatorApp.ViewModels
{
    public partial class CourseTemplateBuilderViewModel : ObservableValidator
    {
        [ObservableProperty]
        [CustomValidation(typeof(CourseTemplateBuilderViewModel), nameof(ValidateCourseName))]
        private string? _courseName;

        public ObservableCollection<LetterGradeViewModel> LetterGrades { get; } = new ObservableCollection<LetterGradeViewModel>();

        public ObservableCollection<CategoryTemplateViewModel> Categories { get; } = new ObservableCollection<CategoryTemplateViewModel>();

        const string TEMPLATEFILEPATH = @"Templates";
        const string COURSETEMPLATEFILENAME = @"CourseTemplates";
        private static char[] INVALIDNAMECHARS = ['\\', '/', ':', '*', '?', '\"', '<', '>', '|' ];

        public CourseTemplateBuilderViewModel()
        {
            //Creates the templates file if doesn't exist
            if (!Directory.Exists(@"Templates"))
            {
                Directory.CreateDirectory(@"Templates");
            }

            //Creates the course templates file if doesn't exist
            if (!Directory.Exists(@"Templates\CourseTemplates"))
            {
                Directory.CreateDirectory(@"Templates\CourseTemplates");
            }
        }

        [RelayCommand]
        private void AddCategory()
        {
            CategoryTemplateViewModel category = new CategoryTemplateViewModel();

            //subscribes to the categories event so the category can notify the course if a change has been made
            category.SaveTemplateButtonChange += SaveTemplateButtonChange;

            Categories.Add(category);

            //tells the save template button a change occured
            CreateTemplateCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        private void RemoveCategory(CategoryTemplateViewModel category)
        {
            Categories.Remove(category);

            //usubscribes from the event since it is no longer needed
            category.SaveTemplateButtonChange -= SaveTemplateButtonChange;

            //tells the save template button a change occured
            CreateTemplateCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        private void AddLetterGrade()
        {
            LetterGradeViewModel letter = new LetterGradeViewModel();

            //subscribes to the letter grades event so the letter grade can notify the course if a change has been made
            letter.SaveTemplateButtonChange += SaveTemplateButtonChange;
            LetterGrades.Add(letter);

            //tells the save template button a change occured
            CreateTemplateCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        private void RemoveLetterGrade()
        {
            //checks if there is a letter grade to remove
            if (LetterGrades.Count > 0)
            {
                //usubscribes from the event since it is no longer needed
                LetterGrades[LetterGrades.Count - 1].SaveTemplateButtonChange -= SaveTemplateButtonChange;

                LetterGrades.RemoveAt(LetterGrades.Count - 1);

                //tells the save template button a change occured
                CreateTemplateCommand.NotifyCanExecuteChanged();
            }
        }

        //resets the template builder to have no data
        [RelayCommand]
        private void Clear()
        {
            CourseName = null;

            //usubscribes from all of the event since they are no longer needed
            foreach (LetterGradeViewModel letter in LetterGrades)
            {
                letter.SaveTemplateButtonChange -= SaveTemplateButtonChange;
            }
            LetterGrades.Clear();

            foreach(CategoryTemplateViewModel category in Categories)
            {
                category.SaveTemplateButtonChange -= SaveTemplateButtonChange;
            }
            Categories.Clear();
        }

        //relay command for saving the course template
        [RelayCommand(CanExecute = nameof(ValidateEntireCourse))]
        private void CreateTemplate()
        {
            //initializes teh course template
            CourseTemplate courseTemplate = new CourseTemplate(CourseName);

            //converts the letter grade view models to data usable by the CourseTemplate
            string[] letters = new string[LetterGrades.Count];
            double[] letterScores = new double[LetterGrades.Count];

            for (int i = 0; i < LetterGrades.Count; i++)
            {
                letters[i] = LetterGrades[i].Letter;

                //turnary safty check for if the grade is set to null but this should never happen
                letterScores[i] = LetterGrades[i].Grade != null ? (double)LetterGrades[i].Grade : 0f;
            }

            //applies the letter grades to the CourseTemplate
            courseTemplate.SetLetterGrade(new LetterGradeSet(letters, letterScores));

            //copies all of the categories over to the CourseTemplate
            foreach (CategoryTemplateViewModel category in Categories)
                courseTemplate.AddCategory(category.GetCategory());

            //builds the course file path
            string newCourseLocation = Path.Combine(TEMPLATEFILEPATH, COURSETEMPLATEFILENAME, CourseName + ".txt");

            //writes the CourseTemplate to a file
            using (StreamWriter templateFile = new StreamWriter(newCourseLocation))
            {
                templateFile.WriteLine(courseTemplate.GetTemplateFile());
                templateFile.Close();
            }
        }

        //used for data validation

        //returns true for no errors
        public bool ValidateEntireCourse()
        {
            //makes sure the name is valid
            if (string.IsNullOrEmpty(CourseName))
                return false;

            if (CourseName.IndexOfAny(CourseTemplateBuilderViewModel.INVALIDNAMECHARS) >= 0)
                return false;

            //makes sure there is at least 1 letter grade
            if (LetterGrades.Count == 0)
                return false;

            //makes sure there is at least 1 category
            if (Categories.Count == 0)
                return false;

            //validates all of the letter grades
            foreach(LetterGradeViewModel letterGrade in LetterGrades)
            {
                if(!letterGrade.IsValidLetterGrade())
                    return false;
            }

            //validates all of the categories
            foreach (CategoryTemplateViewModel category in Categories)
            {
                if(!category.IsValidCategory())
                    return false;
            }

            return true;
        }

        //used for notifying the course template about changes in categories or letter grades
        private void SaveTemplateButtonChange(object? sender, EventArgs e)
        {
            CreateTemplateCommand.NotifyCanExecuteChanged();
        }

        public static ValidationResult ValidateCourseName(string courseName, ValidationContext context)
        {
            //prevents an empty string as a name
            if (string.IsNullOrWhiteSpace(courseName))
            {
                return new("Invalid course name please enter a course name that contains at least one character that is not white space.");
            }

            //prevents the user from entering characters that are invalid for file names
            if (courseName.IndexOfAny(CourseTemplateBuilderViewModel.INVALIDNAMECHARS) >= 0)
                return new("The course name cannot contain: \\ / : * ? \" < > | do to how files are stored.");

            return ValidationResult.Success;
        }

        partial void OnCourseNameChanged(string? oldValue, string? newValue)
        {
            //tells the code to validate the course name
            ValidateProperty(newValue, nameof(CourseName));

            //tells the save template button a change occured
            CreateTemplateCommand.NotifyCanExecuteChanged();
        }
    }
}
