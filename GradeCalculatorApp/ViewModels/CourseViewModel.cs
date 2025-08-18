using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GradeCalculatorLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeCalculatorApp.ViewModels
{
    public partial class CourseViewModel : ViewModelBase
    {
        private CourseCalculator? _course;

        [ObservableProperty]
        private string? _courseName;

        [ObservableProperty]
        private double? _trueGrade;

        public ObservableCollection<LetterGradeViewModel> LetterGrades { get; } = new ObservableCollection<LetterGradeViewModel>();
        public ObservableCollection<CategoryViewModel> Categories { get; } = new ObservableCollection<CategoryViewModel>();

        [ObservableProperty]
        private string? _errorMessage;

        public void LoadCourse(string? path)
        {
            _course = null;
            LetterGrades.Clear();
            Categories.Clear();
            TrueGrade = 0;

            if(string.IsNullOrEmpty(path))
            {
                ErrorMessage = "No class selected";
                return;
            }

            try
            {
                _course = new CourseCalculator(path);
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
                return;
            }

            BuildLetterGrades();

            BuildCategories();
        }

        private void BuildLetterGrades()
        {
            for (int i = 0; i < _course.LetterGrades.Letters.Length; i++)
            {
                LetterGrades.Add(new LetterGradeViewModel(_course.LetterGrades.Letters[i], _course.LetterGrades.LetterScores[i]));
            }
        }

        private void BuildCategories()
        {
            foreach (Category category in _course.Categories)
            {
                Categories.Add(new CategoryViewModel(category));
            }
        }
    }
}
