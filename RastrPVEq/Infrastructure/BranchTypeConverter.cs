using System;
using System.Globalization;
using System.Windows.Data;
using RastrPVEq.Models.RastrWin3;

namespace RastrPVEq.Infrastructure
{
    [ValueConversion(typeof(BranchType), typeof(string))]
    class BranchTypeConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BranchType.Line)
            {
                return "Л";
            }
            else if (value is BranchType.Transformer)
            {
                return "ТР";
            }
            else
            {
                return "?";
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value.ToString();
            if (stringValue == "Л")
            {
                return BranchType.Line;
            }
            else if (stringValue == "ТР")
            {
                return BranchType.Transformer;
            }
            else
            {
                return BranchType.Line;
            }
        }
    }
}
