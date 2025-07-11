using CommunityToolkit.Mvvm.ComponentModel;
using GradeCalculatorLibrary;


namespace GradeCalculatorApp.ViewModels
{
    public partial class LetterGradeViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string? _letter;

        [ObservableProperty]
        private double? _grade;
    }
}
