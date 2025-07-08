using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace GradeCalculatorApp.ViewModels
{
    public partial class CourseTemplateBuilderViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string? _courseName;

        //add letter grade stuff later

        public ObservableCollection<CategoryTemplateViewModel> Categories { get; } = new ObservableCollection<CategoryTemplateViewModel>();

        public CourseTemplateBuilderViewModel()
        {
            Categories.Add(new CategoryTemplateViewModel());
            Categories.Add(new CategoryTemplateViewModel());
            Categories.Add(new CategoryTemplateViewModel());
            Categories.Add(new CategoryTemplateViewModel());
            Categories[0].Name = "Test 1";
            Categories[1].Name = "Test 2";
            Categories[2].Name = "Test 3";
            Categories[3].Name = "Test 4";
        }

    }
}
