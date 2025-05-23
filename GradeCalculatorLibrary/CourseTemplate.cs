using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeCalculatorLibrary
{
    public class CourseTemplate : Course
    {
        public CourseTemplate(string courseName)
        {
            CourseName = courseName;
        }

        public string GetTemplateFile()
        {
            if (LetterGrades == null || Categories.Count == 0)
                throw new CourseException("Invalid course: Unable to write tempalte file because a course must have a letter grade set and at least 1 category");

            StringBuilder templateStringBuilder = new StringBuilder();

            templateStringBuilder.Append(CourseName);
            templateStringBuilder.Append('\n');

            templateStringBuilder.Append(LetterGrades); //writes the letter grade set

            templateStringBuilder.Append(Categories.Count);
            templateStringBuilder.Append('\n');

            //writes all categories
            for (int i = 0; i < Categories.Count; i++)
                templateStringBuilder.Append(Categories[i]);

            return templateStringBuilder.ToString();
        }
    }
}
