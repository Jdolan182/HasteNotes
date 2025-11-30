using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace HasteNotes.Converters
{
    public class NullToEmptyCollectionConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value ?? new object[0];

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}