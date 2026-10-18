namespace WarOrgan.Model.DataSet;

public static class DataSetValidation
{
    private static readonly char[] _allowedDataSetIdCharacters = [' ', '-', '_', '(', ')', '[', ']'];

    public static bool IsValidDataSetId(string? id)
    {
        const int maxLength = 30;
        const int minLength = 3;

        if (string.IsNullOrWhiteSpace(id) || id.Length < minLength || id.Length > maxLength)
            return false;

        if (id.Equals("CON", StringComparison.OrdinalIgnoreCase)
            || id.Equals("PRN", StringComparison.OrdinalIgnoreCase)
            || id.Equals("AUX", StringComparison.OrdinalIgnoreCase)
            || id.Equals("NUL", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var isValid = id.All(character =>
            char.IsAsciiLetter(character)
            || char.IsAsciiDigit(character)
            || _allowedDataSetIdCharacters.Contains(character));

        return isValid;
    }
}
