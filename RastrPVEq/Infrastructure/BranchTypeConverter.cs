using System;
using System.Globalization;
using System.Windows.Data;
using RastrPVEq.Models.PowerSystem;

namespace RastrPVEq.Infrastructure
{
    /// <summary>
    /// BranchTypeConverter class
    /// </summary>
    [ValueConversion(typeof(BranchType), typeof(string))]
    internal class BranchTypeConverter : IValueConverter
    {
        /// <summary>
        /// Convert
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="culture">Culture</param>
        /// <returns></returns>
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                BranchType.Line => "Л",
                BranchType.Transformer => "ТР",
                _ => "?"
            };
        }

        /// <summary>
        /// Convert back
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="culture">Culture</param>
        /// <returns></returns>
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value.ToString();

            return stringValue switch
            {
                "Л" => BranchType.Line,
                "ТР" => BranchType.Transformer,
                _ => BranchType.Line
            };
        }
    }
}
