namespace SteveLauncher.Utils.Converter;

public class CollectionZeroToBoolConverter : IValueConverter {
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is ICollection<object> collection)
        {
            return collection.Count > 0;
        }
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        return Binding.DoNothing;
    }
}