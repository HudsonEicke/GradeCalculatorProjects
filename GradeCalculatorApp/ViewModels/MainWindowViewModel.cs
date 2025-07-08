namespace GradeCalculatorApp.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public CourseTemplateBuilderViewModel CourseTemplateBuilderViewModel { get; } = new CourseTemplateBuilderViewModel();
    }
}
