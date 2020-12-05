namespace Scriber.Util
{
    public static class StringUtility
    {
        public static string Trim(this string value, out int advance)
        {
            return TrimStart(value, out advance).TrimEnd();
        }

        public static string TrimStart(this string value, out int advance)
        {
            var trimmed = value.TrimStart();
            advance = value.Length - trimmed.Length;
            return trimmed;
        }

        public static string Pluralify(this int count, string single, string plural)
        {
            return count == 1 ? single : plural;
        }
    }
}
