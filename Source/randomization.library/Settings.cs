using System.Globalization;

namespace randomization.library;
public static class Settings
{
    private static NumberFormatInfo CultureNumberFormatInfo { get; } = CultureInfo.CurrentCulture.NumberFormat; // Gets a NumberFormatInfo associated with the current culture.
    public static short MissingBlockSize { get; private set; } = -999;
    public static short MissingValue { get; private set; } = -999;
    public static string NumberDecimalSeparator = CultureNumberFormatInfo.NumberDecimalSeparator;

}
