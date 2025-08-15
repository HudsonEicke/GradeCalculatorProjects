using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeCalculatorApp.ViewModels
{
    public partial class AssignmentViewModel : ObservableValidator
    {
        [ObservableProperty]
        private int _assignmentNum;

        [ObservableProperty]
        [CustomValidation(typeof(AssignmentViewModel), nameof(ValidateStringGrade))]
        private string? _stringGrade;

        public double? Grade { get; private set; }

        public event EventHandler<int>? GradeChanged;

        public AssignmentViewModel(int assignmentNum)
        {
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
