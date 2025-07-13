using CommunityToolkit.Mvvm.ComponentModel;
using GradeCalculatorLibrary;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;


namespace GradeCalculatorApp.ViewModels
{
    public partial class LetterGradeViewModel : ObservableValidator
    {
        [ObservableProperty]
        [CustomValidation(typeof(LetterGradeViewModel), nameof(ValidateLetter))]
        private string? _letter;

        [ObservableProperty]
        [CustomValidation(typeof(LetterGradeViewModel), nameof(ValidateGrade))]
        private double? _grade;

        public event EventHandler? SaveTemplateButtonChange;

        public bool IsValidLetterGrade()
        {
            if (string.IsNullOrWhiteSpace(Letter))
            {
                return false;
            }

            if (Letter.Contains('|'))
            {
                return false;
            }

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
            ValidateProperty(newValue, nameof(Letter));
            SaveTemplateButtonChange?.Invoke(this, EventArgs.Empty);
        }

        public static ValidationResult ValidateLetter(string letter, ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(letter))
            {
                return new("Letters must contain at least 1 character that is not a |.");
            }

            if (letter.Contains('|'))
            {
                return new("Letters cannot contain the | character");
            }

            return ValidationResult.Success;
        }

        //Grade logic
        partial void OnGradeChanged(double? oldValue, double? newValue)
        {
            ValidateProperty(newValue, nameof(Grade));
            SaveTemplateButtonChange?.Invoke(this, EventArgs.Empty);
        }

        public static ValidationResult ValidateGrade(double? grade, ValidationContext context)
        {
            if (grade == null)
            {
                return new("Grade must be a non negative number");
            }

            if (grade < 0)
            {
                return new("Grade must be a non negative number");
            }

            return ValidationResult.Success;
        }
    }
}
