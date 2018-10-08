using System;

namespace Huesie.ConsoleSPA.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Writes a fixed width string by allowing for enhancement characters.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="width">The width.</param>
        /// <param name="elipsis">The elipsis.</param>
        /// <param name="enhChar">The enh character.</param>
        /// <returns></returns>
        public static string FixedWidthEnh(this string s, int width = 64, string elipsis = "..", char enhChar = '*')
        {
            string str = s.TrimEnd();
            int visibleLength = str.Replace(enhChar.ToString(), string.Empty).Length;

            if (visibleLength <= width)
            {
                return str.PadRight(width + (str.Length - visibleLength));
            }
            else
            {
                return str.TruncEnh(width - elipsis.Length) + elipsis;
            }
        }

        /// <summary>
        /// Truncates a string, allowing for enhancement characters.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="length">The length.</param>
        /// <param name="enhChar">The enh character.</param>
        /// <returns></returns>
        public static string TruncEnh(this string s, int length, char enhChar = '*')
        {
            int actualPos = 0, visiblePos = 0;

            foreach (char ch in s)
            {
                if (ch != enhChar)
                {
                    visiblePos++;
                    if (visiblePos > length)
                    {
                        break; // found length required
                    }
                }
                actualPos++;
            }

            return s.Substring(0, actualPos);
        }
    }
}
