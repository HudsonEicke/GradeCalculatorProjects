using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GradeCalculatorApp.Converters;

namespace GradeCalculatorApp.Views;

public partial class CourseTemplateBuilderView : UserControl
{
    public static readonly StringToDoubleConverter DoubleConverter = new StringToDoubleConverter();
    public static readonly StringToIntegerConverter IntegerConverter = new StringToIntegerConverter();

    public CourseTemplateBuilderView()
    {
        InitializeComponent();

        Resources["DoubleConverter"] = DoubleConverter;
        Resources["IntegerConverter"] = IntegerConverter;
    }
}