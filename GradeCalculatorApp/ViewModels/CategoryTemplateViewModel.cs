using CommunityToolkit.Mvvm.ComponentModel;
using GradeCalculatorLibrary;
using System;
using System.ComponentModel.DataAnnotations;


namespace GradeCalculatorApp.ViewModels
{
    public partial class CategoryTemplateViewModel : ObservableValidator
    {
        [ObservableProperty]
        [CustomValidation(typeof(CategoryTemplateViewModel), nameof(ValidateCategoryName))]
        private string? _name;

        [ObservableProperty]
        [CustomValidation(typeof(CategoryTemplateViewModel), nameof(ValidateWeight))]
        private double? _weight;

        [ObservableProperty]
        [CustomValidation(typeof(CategoryTemplateViewModel), nameof(ValidateAssignmentCount))]
        private int? _assignmentCount;

        [ObservableProperty]
        private bool _hasDrops;

        [ObservableProperty]
        [CustomValidation(typeof(CategoryTemplateViewModel), nameof(ValidateDropCount))]
        private int? _dropCount;

        [ObservableProperty]
        [CustomValidation(typeof(CategoryTemplateViewModel), nameof(ValidateDifficulty))]
        private int? _difficulty;

        public event EventHandler? SaveTemplateButtonChange;

        partial void OnHasDropsChanged(bool oldValue, bool newValue)
        {
            if (!newValue)
                DropCount = null;

            SaveTemplateButtonChange?.Invoke(this, EventArgs.Empty);
        }

        public Category GetCategory()
        {
            return new Category(Name, (double)Weight, (int)AssignmentCount, HasDrops, DropCount == null ? 0 : (int)DropCount , (int)Difficulty);
        }

        public bool IsValidCategory()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }

            if (Name.Contains('|'))
            {
                return false;
            }

            if (Weight == null)
            {
                return false;
            }

            if (Weight <= 0)
            {
                return false;
            }

            if (AssignmentCount == null)
            {
                return false;
            }

            if (AssignmentCount <= 0)
            {
                return false;
            }

            if (HasDrops)
            {
                if (DropCount == null)
                {
                    return false;
                }

                if (DropCount <= 0)
                {
                    return false;
                }
            }

            if (Difficulty == null)
            {
                return false;
            }

            return true;
        }

        //Category name logic
        partial void OnNameChanged(string? oldValue, string? newValue)
        {
            ValidateProperty(newValue, nameof(Name));
            SaveTemplateButtonChange?.Invoke(this, EventArgs.Empty);
        }

        public static ValidationResult ValidateCategoryName(string categoryName, ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return new("Invalid category name please enter a category name that contains at least one character that is not white space.");
            }

            if(categoryName.Contains('|'))
            {
                return new("Invalid category name the name can not contain the | character.");
            }

            return ValidationResult.Success;
        }

        //Weight logic
        partial void OnWeightChanged(double? oldValue, double? newValue)
        {
            ValidateProperty(newValue, nameof(Weight));
            SaveTemplateButtonChange?.Invoke(this, EventArgs.Empty);
        }

        public static ValidationResult ValidateWeight(double? weight, ValidationContext context)
        {
            if (weight == null)
            {
                return new("Weight must be a positive number");
            }

            if (weight <= 0)
            {
                return new("Weight must be a positive number");
            }

            return ValidationResult.Success;
        }

        //Assignment count logic
        partial void OnAssignmentCountChanged(int? oldValue, int? newValue)
        {
            ValidateProperty(newValue, nameof(AssignmentCount));
            SaveTemplateButtonChange?.Invoke(this, EventArgs.Empty);
        }

        public static ValidationResult ValidateAssignmentCount(int? assignmentCount, ValidationContext context)
        {
            if (assignmentCount == null)
            {
                return new("Assignment Count must be a positive number");
            }

            if (assignmentCount <= 0)
            {
                return new("Assignment Count must be a positive number");
            }

            return ValidationResult.Success;
        }

        //Drop count logic
        partial void OnDropCountChanged(int? oldValue, int? newValue)
        {
            ValidateProperty(newValue, nameof(DropCount));
            SaveTemplateButtonChange?.Invoke(this, EventArgs.Empty);
        }

        public static ValidationResult ValidateDropCount(int? dropCount, ValidationContext context)
        {
            CategoryTemplateViewModel instance = (CategoryTemplateViewModel)context.ObjectInstance;

            if (!instance.HasDrops)
                return ValidationResult.Success;

            if (dropCount == null)
            {
                return new("Drop Count must be a positive number");
            }

            if (dropCount <= 0)
            {
                return new("Drop Count must be a positive number");
            }

            return ValidationResult.Success;
        }

        //Difficulty logic
        partial void OnDifficultyChanged(int? oldValue, int? newValue)
        {
            ValidateProperty(newValue, nameof(Difficulty));
            SaveTemplateButtonChange?.Invoke(this, EventArgs.Empty);
        }

        public static ValidationResult ValidateDifficulty(int? difficulty, ValidationContext context)
        {
            if (difficulty == null)
            {
                return new("Difficulty must be a non decimal number");
            }

            return ValidationResult.Success;
        }
    }
}
