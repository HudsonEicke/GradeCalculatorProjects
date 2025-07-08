using CommunityToolkit.Mvvm.ComponentModel;
using GradeCalculatorLibrary;


namespace GradeCalculatorApp.ViewModels
{
    public partial class CategoryTemplateViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private string? _weight;

        [ObservableProperty]
        private string? _assignmentCount;

        [ObservableProperty]
        private bool _hasDrops;

        [ObservableProperty]
        private string? _dropCount;

        [ObservableProperty]
        private string? _difficulty;
    }
}
