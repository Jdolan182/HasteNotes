using System;
using System.Globalization;
using Avalonia.Data.Converters;
using System.Collections;

namespace HasteNotes.Converters
{
    public class NullOrCountGreaterThanZeroConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is IEnumerable collection)
                return collection.GetEnumerator().MoveNext();
            return false;
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}