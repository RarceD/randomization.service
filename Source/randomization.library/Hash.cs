namespace randomization.library;
static internal class Hash
{
    /// <summary>
    /// Sorts the characters of a string.
    /// </summary>
    /// <param name="input">The string to sort.</param>
    /// <returns></returns>
    public static string SortString(string input)
    {
        char[] characters = input.ToArray();
        Array.Sort(characters);

        return new string(characters);
    }

    /// <summary>
    /// Calculates a stable hash code of a string.
    /// </summary>
    /// <param name="hashableString">The string from which to calculate a stable hash code.</param>
    /// <returns>A stable hash code of a string.</returns>
    public static long GetStableHashCode(string hashableString)
    {
        //This function is a copy of the 64bit GetHashCode() that will
        //remain stable for future .NET versions.
        //Credits to Scott Chamberlain in https://stackoverflow.com/questions/36845430/persistent-hashcode-for-strings
        unchecked
        {
            long hash1 = 5381;
            long hash2 = hash1;

            for (int i = 0; i < hashableString.Length && hashableString[i] != '\0'; i += 2)
            {
                hash1 = ((hash1 << 5) + hash1) ^ hashableString[i];

                if (i == hashableString.Length - 1 || hashableString[i + 1] == '\0')
                    break;

                hash2 = ((hash2 << 5) + hash2) ^ hashableString[i + 1];
            }

            return hash1 + (hash2 * 1566083941);
        }
    }
}
