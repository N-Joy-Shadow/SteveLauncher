using UraniumUI.Extensions;

namespace SteveLauncher.Utils.Converter;

public class NullToBoolConverter : IValueConverter{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is null)
            return false;
        else
            return true;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        return Binding.DoNothing;
    }
}