using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace CustomControlSample.Converter;

public class IsSmallerOrEqualConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count != 2)
        {
            throw new ArgumentException("Expected exactly two numbers");
        }
        var firstNumber = values[0] as int?;
        var secondNumber = values[1] as int?;

        return firstNumber <= secondNumber;
    }
}