using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeCalculatorApp.ViewModels
{
    public abstract class TabViewModelBase : ViewModelBase
    {
        public abstract string TabHeader { get; }
    }
}
