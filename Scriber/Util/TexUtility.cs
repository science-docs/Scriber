namespace Scriber.Util
{
    public static class TexUtility
    {
        /// <summary>
        /// See: https://en.wikibooks.org/wiki/LaTeX/Special_Characters#Escaped_codes
        /// </summary>
        /// <param name="c"></param>
        /// <param name="diacritic"></param>
        /// <returns></returns>
        public static char CompositeCharacter(char c, char diacritic)
        {
            var unicodeChar = diacritic switch
            {
                '`' => '\u0300',
                '\'' => '\u0301',
                '^' => '\u0302',
                '"' => '\u0308',
                'H' => '\u030B',
                '~' => '\u0303',
                'c' => '\u0327',
                'k' => '\u0328',
                '=' => '\u0304',
                'b' => '\u0331',
                '.' => '\u0307',
                'd' => '\u0323',
                'r' => '\u030A',
                'u' => '\u0306',
                'v' => '\u030C',
                _ => ' ' // just return the input character
            };

            string value = c.ToString();
            value += unicodeChar;
            value = value.Normalize();
            return value[0];
        }
    }
}
