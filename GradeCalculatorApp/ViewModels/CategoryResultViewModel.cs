using System;
using System.Linq;
using System.Text;
using GradeCalculatorLibrary;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GradeCalculatorApp.ViewModels
{
    public partial class CategoryResultViewModel : ViewModelBase
    {
        private CategoryScoreReport _scoreReport;
        public ObservableCollection<AssignmentResultViewModel> Assignments { get; } = new ObservableCollection<AssignmentResultViewModel>();

        [ObservableProperty]
        private string _name;

        public CategoryResultViewModel(CategoryScoreReport scoreReport)
        {
            _scoreReport = scoreReport;
            BuildCategoryResult();
        }

        private void BuildCategoryResult()
        {
            StringBuilder nameBuilder = new StringBuilder();

            nameBuilder.Append(_scoreReport.CategoryName);

            if (_scoreReport.HasDrops)
            {
                nameBuilder.Append($" ({_scoreReport.DropIdxes.Count} drop");

                if (_scoreReport.DropIdxes.Count > 1)
                {
                    nameBuilder.Append('s');
                }

                nameBuilder.Append(')');
            }

            Name = nameBuilder.ToString();

            if (_scoreReport.HasDrops)
            {
                for (int i = 0; i < _scoreReport.Grades.Count(); i++)
                {
                    Assignments.Add(new AssignmentResultViewModel(_scoreReport.CategoryName, i, _scoreReport.Grades[i], _scoreReport.DropIdxes.Contains(i), _scoreReport.CalculatedIdxs.Contains(i)));
                }
            }
            else
            {
                for (int i = 0; i < _scoreReport.Grades.Count(); i++)
                {
                    Assignments.Add(new AssignmentResultViewModel(_scoreReport.CategoryName, i, _scoreReport.Grades[i], false, _scoreReport.CalculatedIdxs.Contains(i)));
                }
            }
        }
    }
}
