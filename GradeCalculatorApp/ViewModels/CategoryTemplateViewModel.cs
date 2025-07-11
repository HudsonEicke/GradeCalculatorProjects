using CommunityToolkit.Mvvm.ComponentModel;
using GradeCalculatorLibrary;


namespace GradeCalculatorApp.ViewModels
{
    public partial class CategoryTemplateViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private double? _weight;

        [ObservableProperty]
        private int? _assignmentCount;

        [ObservableProperty]
        private bool _hasDrops;

        [ObservableProperty]
        private int? _dropCount;

        [ObservableProperty]
        private int? _difficulty;

        partial void OnHasDropsChanged(bool oldValue, bool newValue)
        {
            if (!newValue)
                DropCount = null;
        }
    }
}
