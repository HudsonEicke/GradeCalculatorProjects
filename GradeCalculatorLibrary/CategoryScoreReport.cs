using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeCalculatorLibrary
{
    public class CategoryScoreReport
    {
        public string ? CategoryName { get; private set; }
        public double[] ? Grades { get; private set; }
        public bool HasDrops { get; private set; }
        public List<int> ? DropIdxes { get; private set; }

        public CategoryScoreReport(string categoryName, double[] grades, bool hasDrops, List<int> dropIdxes)
        {
            if(hasDrops)
                Setup(categoryName, grades, hasDrops, dropIdxes);
            else
                Setup(categoryName, grades);
        }

        public CategoryScoreReport(Category category)
        {
            if (!category.HasDrops)
                Setup(category.Name, category.Grades);
        }

        private void Setup(string categoryName, double[] ? grades)
        {
            if (grades == null)
                return;

            CategoryName = categoryName;
            Grades = new double[grades.Length];

            for (int i = 0; i < Grades.Length; i++)
            {
                Grades[i] = grades[i];
            }

            HasDrops = false;
        }

        private void Setup(string categoryName, double[] grades, bool hasDrops, List<int> dropIdxes)
        {

        }
    }
}
