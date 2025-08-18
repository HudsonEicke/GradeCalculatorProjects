using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using GradeCalculatorLibrary;

//maybe add a refresh button

namespace GradeCalculatorApp.ViewModels
{
    public partial class CategoryViewModel : ViewModelBase
    {
        private Category _category;
        public ObservableCollection<AssignmentViewModel> Assignments { get; } = new ObservableCollection<AssignmentViewModel>();

        [ObservableProperty]
        private double _grade;

        public double MaxWeight { get => _category.Weight; }

        [ObservableProperty]
        private string _name;

        //used for displaying assignments with nums next to them ex: hw1 hw2 hw3 ...
        public string AssignmentName {  get => _category.Name; }

        public CategoryViewModel(Category category)
        {
            _category = category;

            for (int i = 0; i < category.AssignmentCount; i++)
            {
                Assignments.Add(new AssignmentViewModel(i));
                Assignments[i].GradeChanged += AssignmentGradeChange;
            }

            StringBuilder nameBuilder = new StringBuilder();

            nameBuilder.Append(category.Name);

            if (category.HasDrops)
            {
                nameBuilder.Append($" ({category.DropCount} drop");

                if (category.DropCount > 1)
                {
                    nameBuilder.Append('s');
                }

                nameBuilder.Append(')');
            }

            Name = nameBuilder.ToString();
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
