using System.Globalization;

namespace IncrementalSheep;

public static class Helpers
{
    public static string FormatAsMoney(this double number)
        => number.ToString("N2", CultureInfo.InvariantCulture);
}