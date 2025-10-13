using System;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GradeCalculatorApp.ViewModels
{
    public partial class AssignmentViewModel : ObservableValidator
    {
        private string _assignmentName;

        [ObservableProperty]
        private int _assignmentNum;

        [ObservableProperty]
        [CustomValidation(typeof(AssignmentViewModel), nameof(ValidateStringGrade))]
        private string? _stringGrade;

        public string DisplayName => $"{_assignmentName} {_assignmentNum + 1}";
        public double? Grade { get; private set; }

        public event EventHandler<int>? GradeChanged;

        public AssignmentViewModel(string assignmentName, int assignmentNum)
        {
            _assignmentName = assignmentName;
            _assignmentNum = assignmentNum;
        }

        partial void OnStringGradeChanged(string? oldValue, string? newValue)
        {
            ValidateProperty(newValue, nameof(StringGrade));

            if(!HasErrors)
            {
                if(string.IsNullOrEmpty(newValue))
                {
                    Grade = null;
                }
                else
                {
                    Grade = double.Parse(newValue);
                }

                GradeChanged?.Invoke(this, AssignmentNum);
            }
        }

        public static ValidationResult ValidateStringGrade(string stringGrade, ValidationContext context)
        {
            if(!string.IsNullOrEmpty(stringGrade) && !double.TryParse(stringGrade, out double grade))
            {
                return new("Invalid value");
            }

            return ValidationResult.Success;
        }
    }
}
