using GradeCalculatorLibrary;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;


namespace GradeCalculatorApp.ViewModels
{
    public partial class LetterGradeResultViewModel : TabViewModelBase
    {
        private string? _letter;

        private double _letterScore;
        public override string TabHeader => $"{_letter}: {_letterScore.ToString("0.##")}%";

        [ObservableProperty]
        private bool _calculated = false;

        public ObservableCollection<CategoryResultViewModel> Categories { get; } = new ObservableCollection<CategoryResultViewModel>();

        public LetterGradeResultViewModel(string letter, double letterScore)
        {
            _letter = letter;
            _letterScore = letterScore;
        }

        public void GradeCalculated(ScoreReport scoreReport)
        {
            Calculated = true;

            Categories.Clear();
            foreach (CategoryScoreReport categoryScoreReport in scoreReport.CategoryScoreReports)
            {
                Categories.Add(new CategoryResultViewModel(categoryScoreReport));
            }
        }
    }
}
