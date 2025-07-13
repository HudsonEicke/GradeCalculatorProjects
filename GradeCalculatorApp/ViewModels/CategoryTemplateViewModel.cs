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
            //resets drop count if the category doesn't have drops
            if (!newValue)
                DropCount = null;

            //tells the save template button a change occured
            SaveTemplateButtonChange?.Invoke(this, EventArgs.Empty);
        }

        //converts the category view model to a category
        public Category GetCategory()
        {
            return new Category(Name, (double)Weight, (int)AssignmentCount, HasDrops, DropCount == null ? 0 : (int)DropCount , (int)Difficulty);
        }

        //validates if the category is a valid category
        public bool IsValidCategory()
        {
            //validates the name
            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }

            if (Name.Contains('|'))
            {
                return false;
            }

            //validates the weight
            if (Weight == null)
            {
                return false;
            }

            if (Weight <= 0)
            {
                return false;
            }

            //validates the assignment count
            if (AssignmentCount == null)
            {
                return false;
            }

            if (AssignmentCount <= 0)
            {
                return false;
            }

            //check for drops if there are drops validates drop count
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

            //validates the difficulty
            if (Difficulty == null)
            {
                return false;
            }

            return true;
        }

        //Category name logic
        partial void OnNameChanged(string? oldValue, string? newValue)
        {
            //tells the code to validate the name
            ValidateProperty(newValue, nameof(Name));

            //tells the save template button a change occured
            SaveTemplateButtonChange?.Invoke(this, EventArgs.Empty);
        }

        public static ValidationResult ValidateCategoryName(string categoryName, ValidationContext context)
        {
            //prevents an empty category name
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return new("Invalid category name please enter a category name that contains at least one character that is not white space.");
            }

            //prevents errors in the template file generation
            if (categoryName.Contains('|'))
            {
                return new("Invalid category name the name can not contain the | character.");
            }

            return ValidationResult.Success;
        }

        //Weight logic
        partial void OnWeightChanged(double? oldValue, double? newValue)
        {
            //tells the code to validate the weight
            ValidateProperty(newValue, nameof(Weight));

            //tells the save template button a change occured
            SaveTemplateButtonChange?.Invoke(this, EventArgs.Empty);
        }

        public static ValidationResult ValidateWeight(double? weight, ValidationContext context)
        {
            //prevents empty weight
            if (weight == null)
            {
                return new("Weight must be a non negative number");
            }

            //prevents negative weight
            if (weight < 0)
            {
                return new("Weight must be a non negative number");
            }

            return ValidationResult.Success;
        }

        //Assignment count logic
        partial void OnAssignmentCountChanged(int? oldValue, int? newValue)
        {
            //tells the code to validate the assignment count
            ValidateProperty(newValue, nameof(AssignmentCount));

            //tells the save template button a change occured
            SaveTemplateButtonChange?.Invoke(this, EventArgs.Empty);
        }

        public static ValidationResult ValidateAssignmentCount(int? assignmentCount, ValidationContext context)
        {
            //prevents an empty assignment count
            if (assignmentCount == null)
            {
                return new("Assignment Count must be a positive number");
            }

            //prevents a category with 0 assignments
            if (assignmentCount <= 0)
            {
                return new("Assignment Count must be a positive number");
            }

            return ValidationResult.Success;
        }

        //Drop count logic
        partial void OnDropCountChanged(int? oldValue, int? newValue)
        {
            //tells the code to validate the drop count
            ValidateProperty(newValue, nameof(DropCount));

            //tells the save template button a change occured
            SaveTemplateButtonChange?.Invoke(this, EventArgs.Empty);
        }

        public static ValidationResult ValidateDropCount(int? dropCount, ValidationContext context)
        {
            //gets the category calling the validation so we can access has drops
            CategoryTemplateViewModel category = (CategoryTemplateViewModel)context.ObjectInstance;

            //if the category doesn't have drops no need to validate
            if (!category.HasDrops)
                return ValidationResult.Success;

            //prevents empty drop count
            if (dropCount == null)
            {
                return new("Drop Count must be a positive number");
            }

            //prevents category that has drops having no or negative drops
            if (dropCount <= 0)
            {
                return new("Drop Count must be a positive number");
            }

            return ValidationResult.Success;
        }

        //Difficulty logic
        partial void OnDifficultyChanged(int? oldValue, int? newValue)
        {
            //tells the code to validate difficulty
            ValidateProperty(newValue, nameof(Difficulty));

            //tells the save template button a change occured
            SaveTemplateButtonChange?.Invoke(this, EventArgs.Empty);
        }

        public static ValidationResult ValidateDifficulty(int? difficulty, ValidationContext context)
        {
            //prevents empty difficulty
            if (difficulty == null)
            {
                return new("Difficulty must be a non decimal number");
            }

            return ValidationResult.Success;
        }
    }
}
