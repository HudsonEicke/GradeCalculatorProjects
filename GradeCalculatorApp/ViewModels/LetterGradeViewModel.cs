using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeCalculatorApp.ViewModels
{
    public partial class LetterGradeViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string? _letter;

        [ObservableProperty]
        private double? _letterScore;

        public LetterGradeViewModel(string letter, double letterScore)
        {
            _letter = letter;
            _letterScore = letterScore;
        }
    }
}
