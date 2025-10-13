using System;
using Avalonia;
using GradeCalculatorLibrary;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Controls.ApplicationLifetimes;

namespace GradeCalculatorApp.ViewModels
{
    public partial class CourseGradeCalculatorViewModel : ViewModelBase
    {
        private CourseCalculator? _course;

        [ObservableProperty]
        private string? _courseName;

        [ObservableProperty]
        private double? _trueGrade = 0;

        public CourseGradeCalculatorInputViewModel CourseGradeCalculatorInputViewModel { get; private set; } = new CourseGradeCalculatorInputViewModel();
        public ObservableCollection<TabViewModelBase> Tabs { get; } = new();


        //public ObservableCollection<CategoryViewModel> Categories { get; } = new ObservableCollection<CategoryViewModel>();

        [ObservableProperty]
        private string? _errorMessage;

        [ObservableProperty]
        private bool _validCourseLoaded = false;

        public CourseGradeCalculatorViewModel()
        {
            CourseGradeCalculatorInputViewModel.GradeChanged += UpdateTrueGrade;
        }

        [RelayCommand]
        private async Task OpenCourse()
        {
            string? coursePath;
            try
            {
                var file = await DoOpenFilePickerAsync();

                if (file == null)
                {
                    coursePath = null;
                }
                else
                {
                    coursePath = file.Path.LocalPath;
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return;
            }

            LoadCourse(coursePath);
        }

        [RelayCommand]
        private void CalculateGrade()
        {
            ScoreReport[] reports = _course.Calculate();

            for (int i = 0; i < reports.Length; i++)
            {
                ((LetterGradeResultViewModel)Tabs[i + 1]).GradeCalculated(reports[i]);
            }
        }

        public void LoadCourse(string? path)
        {
            ValidCourseLoaded = false;
            _course = null;
            CourseName = null;
            CourseGradeCalculatorInputViewModel.Clear();
            TrueGrade = 0;
            Tabs.Clear();


            if (string.IsNullOrEmpty(path))
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

            CourseName = _course.CourseName;
            Tabs.Add(CourseGradeCalculatorInputViewModel);

            BuildLetterGrades();

            BuildCategories();

            ValidCourseLoaded = true;
        }

        private void BuildLetterGrades()
        {
            for (int i = 0; i < _course.LetterGrades.Letters.Length; i++)
            {
                Tabs.Add(new LetterGradeResultViewModel(_course.LetterGrades.Letters[i], _course.LetterGrades.LetterScores[i]));
            }
        }

        private void BuildCategories()
        {
            CourseGradeCalculatorInputViewModel.BuildCategories(_course);
        }

        private async Task<IStorageFile?> DoOpenFilePickerAsync()
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
                desktop.MainWindow?.StorageProvider is not { } provider)
                throw new NullReferenceException("Missing StorageProvider instance.");

            var startFolder = await provider.TryGetFolderFromPathAsync(@"Templates\CourseTemplates");

            var files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Open Course File",
                AllowMultiple = false,
                SuggestedStartLocation = startFolder
            });

            return files?.Count >= 1 ? files[0] : null;
        }


        public void UpdateTrueGrade(object? sender, EventArgs e)
        {
            _course.UpdateTrueGrade();
            TrueGrade = _course.TrueGrade;
        }
    }
}
