
namespace SteveLauncher.Utils.Converter;

public class AuthStateConverter : IValueConverter {
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is Enum enumValue)
        {
            return enumValue.ToString(); 
        }
        return null; 

    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is string stringValue && targetType.IsEnum)
        {
            if (Enum.TryParse(targetType, stringValue, true, out var result))
            {
                return result; 
            }
        }
        return null; 
    }
}