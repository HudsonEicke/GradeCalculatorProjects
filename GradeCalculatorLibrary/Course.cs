namespace GradeCalculatorLibrary
{
    public abstract class Course
    {
        public string ? CourseName { get; protected set; }
        public LetterGradeSet ? LetterGrades { get; protected set; }
        public List<Category> Categories { get; private set; }

        public Course()
        {
            Categories = new List<Category>();
        }

        public void SetCourseName(string name)
        {
            CourseName = name;
        }

        public void SetLetterGrade(LetterGradeSet letterGrades)
        {
            LetterGrades = letterGrades;
        }

        public void AddCategory(Category category)
        {
            Categories.Add(category);
        }

        public void RemoveCategory(int idx)
        {
            if (idx < 0 || idx > Categories.Count)
                return;

            Categories.RemoveAt(idx);
        }
    }

    public class CourseException : Exception
    {
        public CourseException()
        {
        }

        public CourseException(string message) : base(message)
        {
        }

        public CourseException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
