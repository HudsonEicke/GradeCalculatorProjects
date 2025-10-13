namespace GradeCalculatorLibrary
{
    public class CategoryScoreReport
    {
        public string ? CategoryName { get; private set; }
        public double[] ? Grades { get; private set; }
        public bool HasDrops { get; private set; }
        public List<int> CalculatedIdxs { get; private set; }
        public List<int> ? DropIdxes { get; private set; }

        public CategoryScoreReport(string categoryName, double[] grades, bool hasDrops, List<int> dropIdxes)
        {
            if(hasDrops)
                Setup(categoryName, grades, hasDrops, dropIdxes, new List<int>());
            else
                Setup(categoryName, grades);
        }

        public CategoryScoreReport(Category category, List<int> unenteredIdxs)
        {
            CalculatedIdxs = unenteredIdxs;

            if (!category.HasDrops)
                Setup(category.Name, category.Grades);
            else
                Setup(category.Name, category.Grades, category.HasDrops, category.GetDropIndexes(), unenteredIdxs);
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

        private void Setup(string categoryName, double[] grades, bool hasDrops, List<int> dropIdxes, List<int> unenteredIdxs)
        {

            if (grades == null)
                return;

            CategoryName = categoryName;
            Grades = new double[grades.Length];
            DropIdxes = dropIdxes;
            HasDrops = true;

            for (int i = 0; i < Grades.Length; i++)
            {
                if (dropIdxes.Contains(i) && unenteredIdxs.Contains(i))
                    Grades[i] = 0;
                else
                    Grades[i] = grades[i];
            }

        }
    }
}
