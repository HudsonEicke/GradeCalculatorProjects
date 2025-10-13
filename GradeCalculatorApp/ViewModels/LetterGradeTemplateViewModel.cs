using System;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace GradeCalculatorApp.ViewModels
{
    public partial class LetterGradeTemplateViewModel : ObservableValidator
    {
        [ObservableProperty]
        [CustomValidation(typeof(LetterGradeTemplateViewModel), nameof(ValidateLetter))]
        private string? _letter;

        [ObservableProperty]
        [CustomValidation(typeof(LetterGradeTemplateViewModel), nameof(ValidateGrade))]
        private double? _grade;

        public event EventHandler? SaveTemplateButtonChange;

        //returns true if the category is valid and false otherwise
        public bool IsValidLetterGrade()
        {
            //validates the letter
            if (string.IsNullOrWhiteSpace(Letter))
            {
                return false;
            }

            if (Letter.Contains('|'))
            {
                return false;
            }

            //validates the grade
            if (Grade == null)
            {
                return false;
            }

            if (Grade < 0)
            {
                return false;
            }

            return true;
        }

        //Letter logic
        partial void OnLetterChanged(string? oldValue, string? newValue)
        {
            //tells the code to validate the letter
            ValidateProperty(newValue, nameof(Letter));

            //tells the save template button a change occured
            SaveTemplateButtonChange?.Invoke(this, EventArgs.Empty);
        }

        public static ValidationResult ValidateLetter(string letter, ValidationContext context)
        {
            //prevents an empty letter
            if (string.IsNullOrWhiteSpace(letter))
            {
                return new("Letters must contain at least 1 character that is not a |.");
            }

            //prevents errors in the template file generation
            if (letter.Contains('|'))
            {
                return new("Letters cannot contain the | character");
            }

            return ValidationResult.Success;
        }

        //Grade logic
        partial void OnGradeChanged(double? oldValue, double? newValue)
        {
            //tells the code to validate the grade
            ValidateProperty(newValue, nameof(Grade));


            //tells the save template button a change occured
            SaveTemplateButtonChange?.Invoke(this, EventArgs.Empty);
        }

        public static ValidationResult ValidateGrade(double? grade, ValidationContext context)
        {
            //prevents an empty grade
            if (grade == null)
            {
                return new("Grade must be a non negative number");
            }

            //prevents a negative grade
            if (grade < 0)
            {
                return new("Grade must be a non negative number");
            }

            return ValidationResult.Success;
        }
    }
}
