using CommunityToolkit.Mvvm.ComponentModel;

namespace GradeCalculatorApp.ViewModels
{
    public partial class AssignmentResultViewModel : ViewModelBase
    {
        public bool IsDropped;
        private bool _isCalculated;
        private string _assignmentName;
        public string DisplayName => $"{_assignmentName} {_assignmentNum + 1}";
        public string? Grade { get; private set; }

        public string ForegroundColor => GetColor();

        [ObservableProperty]
        private int _assignmentNum;
        public AssignmentResultViewModel(string assignmentName, int assignmentNum, double grade, bool isDropped, bool isCalculated)
        {
            _assignmentName = assignmentName;
            _assignmentNum = assignmentNum;
            Grade = grade.ToString("0.00");
            IsDropped = isDropped;
            _isCalculated = isCalculated;
        }

        private string GetColor()
        {
            if (IsDropped)
            {
                return "Red";
            }

            if (_isCalculated)
            {
                return "LightGreen";
            }

            return "Black";
        }
    }
}
