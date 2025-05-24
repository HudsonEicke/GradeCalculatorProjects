//TODO: Implement drops
//TODO: Implement all or nothing

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeCalculatorLibrary
{
    public class Category
    {
        //public fields
        public string Name { get; private set; }
        public double Weight { get; private set; }
        public int AssignmentCount { get; private set; }
        public double[] ? Grades { get; private set; }
        public bool HasDrops { get; private set; }
        public int DropCount { get; private set; }
        public int Difficulty { get; private set; }

        //private fields
        private bool[] ? _scoreSet;
        private int _enteredScoresCount = 0;
        private List<int> ? _dropIdxes;
        private double _obtainedScore = 0;
        private double _weightPerAssignment;
        private int _lastRecalculateAssignmentCount = 0;

        //used for the case that we are loading a course template file category templates have the following structure
        //AssignmentName|AssignmentWeight|AssignmentCount|HasDrop|if true DropCount|Difficulty
        public Category(string ? categoryTemplate)
        {
            string[] categoryTokens = categoryTemplate.Split('|');

            if (categoryTokens.Length != 5 && categoryTokens.Length != 6)
                throw new CategoryException("Invalid category: A category must contain 5 values or 6 if the category has drops");

            if(categoryTokens.Length == 5)
            {
                //parses the category name part of the template
                Name = categoryTokens[0];

                //parses the category weight part of the template
                double tempWeight;

                if (!double.TryParse(categoryTokens[1], out tempWeight))
                    throw new CategoryException("Invalid category: Invalid value in the weight field");

                Weight = double.Round(tempWeight, 2);

                //parses the assignment count part of the template
                int tempAssignmentCount;

                if (!int.TryParse(categoryTokens[2], out tempAssignmentCount))
                    throw new CategoryException("Invalid category: Invalid value in the assignment count field");

                AssignmentCount = tempAssignmentCount;

                //parses the has drops part of the template
                bool tempHasDrops;

                if (!bool.TryParse(categoryTokens[3], out tempHasDrops))
                    throw new CategoryException("Invalid category: Invalid value in the has drops field");

                if (tempHasDrops)
                    throw new CategoryException("Invalid category: Missing field for either drop count of difficulty");

                HasDrops = tempHasDrops;
                DropCount = 0;

                //parses the difficulty part of the template
                int tempDifficulty;

                if (!int.TryParse(categoryTokens[4], out tempDifficulty))
                    throw new CategoryException("Invalid category: Invalid value in the difficulty field");

                Difficulty = tempDifficulty;
            }
            else
            {
                //parses the category name part of the template
                Name = categoryTokens[0];

                //parses the category weight part of the template
                double tempWeight;

                if (!double.TryParse(categoryTokens[1], out tempWeight))
                    throw new CategoryException("Invalid category: Invalid value in the weight field");

                Weight = double.Round(tempWeight, 2);

                //parses the assignment count part of the template
                int tempAssignmentCount;

                if (!int.TryParse(categoryTokens[2], out tempAssignmentCount))
                    throw new CategoryException("Invalid category: Invalid value in the assignment count field");

                AssignmentCount = tempAssignmentCount;

                //parses the has drops part of the template
                bool tempHasDrops;

                if (!bool.TryParse(categoryTokens[3], out tempHasDrops))
                    throw new CategoryException("Invalid category: Invalid value in the has drops field");

                if (!tempHasDrops)
                    throw new CategoryException("Invalid category: Extra field is in the template");

                HasDrops = tempHasDrops;

                //parses the drop count part of the template
                int tempDropCount;

                if (!int.TryParse(categoryTokens[4], out tempDropCount))
                    throw new CategoryException("Invalid category: Invalid value in the drop count field");

                if(tempDropCount >= AssignmentCount)
                    throw new CategoryException("Invalid category: Cannont have a drop count greater than or equal to the assignment count");

                DropCount = tempDropCount;

                //parses the difficulty part of the template
                int tempDifficulty;

                if (!int.TryParse(categoryTokens[5], out tempDifficulty))
                    throw new CategoryException("Invalid category: Invalid value in the difficulty field");

                Difficulty = tempDifficulty;
            }

            Grades = new double[AssignmentCount];
            _scoreSet = new bool[AssignmentCount];

            //WHEN DROP LOGIC IS ADDED MAYBE REMOVE THIS CODE
            if (HasDrops)
                _dropIdxes = new List<int>();

            if(HasDrops)
                _weightPerAssignment = Weight / (AssignmentCount - DropCount);
            else
                _weightPerAssignment = Weight / AssignmentCount;
        }

        //used for category creation
        public Category(string name, double weight, int assignmentCount, bool hasDrops, int dropCount, int difficulty)
        {
            if (name.Contains('|'))
                throw new CategoryException("Invalid category: Category name may not contain the | character");

            Name = name;
            Weight = weight;
            AssignmentCount = assignmentCount;
            HasDrops = hasDrops;
            DropCount = dropCount;
            Difficulty = difficulty;
        }

        public bool SetScore(int assignmentNum, double score)
        {
            //if a template is used like an actual category
            if (Grades == null || _scoreSet == null)
                return false;

            //if invalid assignment num
            if (assignmentNum < 0 || assignmentNum >= AssignmentCount)
                return false;

            if(HasDrops)
            {
                //WHEN DROP LOGIC IS ADDED ADD THE CODE HERE
            }
            else
            {
                //if the score has not been set
                if(!_scoreSet[assignmentNum])
                {
                    //set and add score
                    Grades[assignmentNum] = score;
                    _scoreSet[assignmentNum] = true;
                    _obtainedScore += CalculateIdxScore(assignmentNum);
                    _enteredScoresCount++;
                }
                else //score has been set before
                {
                    //remove old score
                    _obtainedScore -= CalculateIdxScore(assignmentNum);

                    //add new score
                    Grades[assignmentNum] = score;
                    _obtainedScore += CalculateIdxScore(assignmentNum);
                }
            }

            FinalizeScoreChange();

            return true;
        }

        public bool ResetScore(int assignmentNum)
        {
            //if a template is used like an actual category
            if (Grades == null || _scoreSet == null)
                return false;

            //if invalid assignment num
            if (assignmentNum < 0 || assignmentNum >= AssignmentCount)
                return false;

            //if the score has not been set yet
            if (!_scoreSet[assignmentNum])
                return true;

            if(!HasDrops)
            {
                //remove the old score
                _obtainedScore -= CalculateIdxScore(assignmentNum);
            }

            Grades[assignmentNum] = 0;
            _scoreSet[assignmentNum] = false;
            _enteredScoresCount--;

            if(HasDrops)
            {
                //WHEN DROP LOGIC IS ADDED ADD THE CODE HERE
            }

            FinalizeScoreChange();

            return true;
        }

        //adds the amount to increase and updates the overall score
        public bool UpdateScore(int assignmentNum, double amountToIncrease)
        {
            //if a template is used like an actual category
            if (Grades == null || _scoreSet == null)
                return false;

            //if invalid assignment num
            if (assignmentNum < 0 || assignmentNum >= AssignmentCount)
                return false;

            if (HasDrops)
            {
                //WHEN DROP LOGIC IS ADDED ADD THE CODE HERE
            }
            else
            {
                //if the score has already been set
                if (_scoreSet[assignmentNum])
                {
                    //remove old score
                    _obtainedScore -= CalculateIdxScore(assignmentNum);

                    //add new score
                    Grades[assignmentNum] += amountToIncrease;
                    _obtainedScore += CalculateIdxScore(assignmentNum);

                }
                else
                {
                    //set and add score
                    Grades[assignmentNum] = amountToIncrease;
                    _scoreSet[assignmentNum] = true;
                    _obtainedScore += CalculateIdxScore(assignmentNum);
                    _enteredScoresCount++;
                }
            }

            FinalizeScoreChange();

            return true;
        }

        public void RecalculateScore()
        {
            //if a template is used like an actual category
            if (_scoreSet == null)
                return;

            if(HasDrops)
            {
                //WHEN DROP LOGIC IS ADDED ADD THE CODE HERE
            }
            else
            {
                _obtainedScore = 0;

                for(int i = 0; i < AssignmentCount; i++)
                {
                    //if score has not been set yet
                    if (!_scoreSet[i])
                        continue;

                    _obtainedScore += CalculateIdxScore(i);
                }
            }

            FinalizeScoreChange();
        }

        //calculates the score of the given index
        private double CalculateIdxScore(int assignmentNum)
        {
            //if a template is used like an actual category
            if (Grades == null)
                return 0;

            return (Grades[assignmentNum] / 100f) * _weightPerAssignment;
        }

        public void ConvertToRealCategory()
        {
            //is already a real category
            if (Grades != null)
                return;

            Grades = new double[AssignmentCount];
            _scoreSet = new bool[AssignmentCount];

            //WHEN DROP LOGIC IS ADDED MAYBE REMOVE THIS CODE
            if (HasDrops)
                _dropIdxes = new List<int>();

            if (HasDrops)
                _weightPerAssignment = Weight / (AssignmentCount - DropCount);
            else
                _weightPerAssignment = Weight / AssignmentCount;
        }

        //any logic that should take place after a score has been changed
        private void FinalizeScoreChange()
        {

        }

        //checks if the max score has been obtained
        public bool AtMaxWeight()
        {
            return _obtainedScore == Weight;
        }

        //checks if all grades have been filled
        public bool AllEntered()
        {
            return AssignmentCount == _enteredScoresCount;
        }

        public List<int> ? GetUnenteredIdxes()
        {
            if (_scoreSet == null)
                return null;

            List<int> unenteredIdx = new List<int>();

            for(int i = 0; i < AssignmentCount; i++)
            {
                if (!_scoreSet[i])
                    unenteredIdx.Add(i);
            }

            return unenteredIdx;
        }

        //used for course template file building
        public override string ToString()
        {
            StringBuilder categoryString = new StringBuilder();

            categoryString.Append(Name);
            categoryString.Append('|');
            categoryString.Append(Weight);
            categoryString.Append('|');
            categoryString.Append(AssignmentCount);
            categoryString.Append('|');
            categoryString.Append(HasDrops);
            categoryString.Append('|');

            if (HasDrops)
            {
                categoryString.Append(DropCount);
                categoryString.Append('|');
            }

            categoryString.Append(Difficulty);
            categoryString.Append('\n');

            return categoryString.ToString();
        }
    }

    public class CategoryTemplateException : Exception
    {
        public CategoryTemplateException()
        {
        }

        public CategoryTemplateException(string message) : base(message)
        {
        }

        public CategoryTemplateException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class CategoryException : Exception
    {
        public CategoryException()
        {
        }

        public CategoryException(string message) : base(message)
        {
        }

        public CategoryException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
