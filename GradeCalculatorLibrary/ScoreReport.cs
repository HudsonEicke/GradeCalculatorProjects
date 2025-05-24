using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeCalculatorLibrary
{
    public class ScoreReport
    {
        public string ScoreReportLetter { get; private set; }
        public CategoryScoreReport[] CategoryScoreReports { get; private set; }

        public ScoreReport(string scoreReportLetter, CategoryScoreReport[] categoryScoreReports)
        {
            ScoreReportLetter = scoreReportLetter;
            CategoryScoreReports = new CategoryScoreReport[categoryScoreReports.Length];

            for (int i = 0; i < categoryScoreReports.Length; i++)
            {
                CategoryScoreReports[i] = categoryScoreReports[i];
            }
        }
    }
}
