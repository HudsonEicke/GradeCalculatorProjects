using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using GradeCalculatorLibrary;

namespace GradeCalculatorApp.ViewModels
{
    public partial class CategoryViewModel : ViewModelBase
    {
        private Category _category;
        public ObservableCollection<AssignmentViewModel> Assignments { get; } = new ObservableCollection<AssignmentViewModel>();

        [ObservableProperty]
        private double _grade;

        public CategoryViewModel(Category category)
        {
            _category = category;

            for (int i = 0; i < category.AssignmentCount; i++)
            {
                Assignments.Add(new AssignmentViewModel(i));
                Assignments[i].GradeChanged += AssignmentGradeChange;
            }
        }

        private void AssignmentGradeChange(object? sender, int assignmentNum)
        {
            if(Assignments[assignmentNum].Grade == null)
            {
                _category.ResetScore(assignmentNum);
            }
            else
            {
                _category.SetScore(assignmentNum, (double)Assignments[assignmentNum].Grade);
            }

            Grade = _category.ObtainedScore;
        }
    }
}
