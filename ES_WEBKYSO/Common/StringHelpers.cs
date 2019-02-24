using System.Collections.Generic;
using System.Linq;

namespace ES_WEBKYSO.Common
{
    public static class StringHelpers
    {
        public static string CameCaseToRawString(string cameCase)
        {
            var newStr = FirstLetterUperString(cameCase);
            var letters = newStr.ToCharArray().ToList().Select(x => x.ToString()).ToList();
            var newLetters = new List<string>();
            foreach (var letter in letters)
            {
                if (letter == letter.ToUpper())
                {
                    newLetters.Add(" ");
                }
                newLetters.Add(letter);
            }
            return string.Join(string.Empty, newLetters.ToArray());
        }

        public static string FirstLetterUperString(string raw)
        {
            raw = raw.Trim();
            var letters = raw.ToCharArray();
            letters[0] = char.ToUpper(letters[0]);
            return new string(letters);
        }
    }
}