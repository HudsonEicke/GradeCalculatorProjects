using GradeCalculatorLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeCalculatorApp.ViewModels
{
    public partial class CourseGradeCalculatorInputViewModel : TabViewModelBase
    {
        public ObservableCollection<CategoryViewModel> Categories { get; } = new ObservableCollection<CategoryViewModel>();
        public override string TabHeader => "Input";

        public void Clear()
        {
            Categories.Clear();
        }

        public void BuildCategories(CourseCalculator course)
        {
            foreach (Category category in course.Categories)
            {
                Categories.Add(new CategoryViewModel(category));
            }
        }
    }
}
