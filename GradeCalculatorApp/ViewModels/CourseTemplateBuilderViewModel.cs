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
            if (!Directory.Exists(@"Templates"))
            {
                Directory.CreateDirectory(@"Templates");
            }

            if (!Directory.Exists(@"Templates\CourseTemplates"))
            {
                Directory.CreateDirectory(@"Templates\CourseTemplates");
            }
        }

        [RelayCommand]
        private void AddCategory()
        {
            CategoryTemplateViewModel category = new CategoryTemplateViewModel();

            category.SaveTemplateButtonChange += SaveTemplateButtonChange;

            Categories.Add(category);
            CreateTemplateCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        private void RemoveCategory(CategoryTemplateViewModel category)
        {
            Categories.Remove(category);
            category.SaveTemplateButtonChange -= SaveTemplateButtonChange;
            CreateTemplateCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        private void AddLetterGrade()
        {
            LetterGradeViewModel letter = new LetterGradeViewModel();
            letter.SaveTemplateButtonChange += SaveTemplateButtonChange;
            LetterGrades.Add(letter);
            CreateTemplateCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        private void RemoveLetterGrade()
        {
            if (LetterGrades.Count > 0)
            {
                LetterGrades[LetterGrades.Count - 1].SaveTemplateButtonChange -= SaveTemplateButtonChange;
                LetterGrades.RemoveAt(LetterGrades.Count - 1);
            }

            CreateTemplateCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        private void Clear()
        {
            CourseName = null;
            LetterGrades.Clear();
            Categories.Clear();
        }

        //add safety check
        [RelayCommand(CanExecute = nameof(ValidateEntireCourse))]
        private void CreateTemplate()
        {
            CourseTemplate courseTemplate = new CourseTemplate(CourseName);

            string[] letters = new string[LetterGrades.Count];
            double[] letterScores = new double[LetterGrades.Count];

            for (int i = 0; i < LetterGrades.Count; i++)
            {
                letters[i] = LetterGrades[i].Letter;
                letterScores[i] = LetterGrades[i].Grade != null ? (double)LetterGrades[i].Grade : 0f;
            }

            courseTemplate.SetLetterGrade(new LetterGradeSet(letters, letterScores));

            foreach (CategoryTemplateViewModel category in Categories)
                courseTemplate.AddCategory(category.GetCategory());

            string newCourseLocation = Path.Combine(TEMPLATEFILEPATH, COURSETEMPLATEFILENAME, CourseName + ".txt");

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
            if (string.IsNullOrEmpty(CourseName))
                return false;

            if (CourseName.IndexOfAny(CourseTemplateBuilderViewModel.INVALIDNAMECHARS) >= 0)
                return false;

            if (LetterGrades.Count == 0)
                return false;

            if(Categories.Count == 0)
                return false;

            foreach(LetterGradeViewModel letterGrade in LetterGrades)
            {
                if(!letterGrade.IsValidLetterGrade())
                    return false;
            }

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
            if (string.IsNullOrWhiteSpace(courseName))
            {
                return new("Invalid course name please enter a course name that contains at least one character that is not white space.");
            }

            if (courseName.IndexOfAny(CourseTemplateBuilderViewModel.INVALIDNAMECHARS) >= 0)
                return new("The course name cannot contain: \\ / : * ? \" < > | do to how files are stored.");

            return ValidationResult.Success;
        }

        partial void OnCourseNameChanged(string? oldValue, string? newValue)
        {
            ValidateProperty(newValue, nameof(CourseName));
            CreateTemplateCommand.NotifyCanExecuteChanged();
        }
    }
}
