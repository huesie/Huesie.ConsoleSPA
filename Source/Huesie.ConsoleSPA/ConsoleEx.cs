using System;
using System.Linq;
using System.Threading.Tasks;

namespace Huesie.ConsoleSPA
{
    /// <summary>
    /// An enhancement to <seealso cref="Console"/> with basic enhancement support.
    /// </summary>
    public static class ConsoleEx
    {
        private static object _lockOutput = new object();

        /// <summary>
        /// Gets the adjusted width of the window.
        /// </summary>
        /// <value>
        /// The width of the window.
        /// </value>
        public static int WindowWidth { get { return Console.WindowWidth - 1; } }

        /// <summary>
        /// Applies a highlight to the specified value.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <param name="highlight">if set to <c>true</c> [highlight].</param>
        /// <returns></returns>
        public static string Highlight(string value, bool highlight = true)
        {
            return (highlight ? "*" : "") + value + (highlight ? "*" : "");
        }

        /// <summary>
        /// Prompts the user for allowed input keys.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <param name="allowedKeysToUpper">The allowed keys to upper.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">allowedKeysToUpper</exception>
        public static string PromptEnhForInputKeysUpper(string prompt, string allowedKeysToUpper)
        {
            lock (_lockOutput)
            {
                WriteEnh(prompt + ((!string.IsNullOrWhiteSpace(prompt) && prompt[prompt.Length - 1] != ' ') ? " " : ""));

                if (allowedKeysToUpper == null || allowedKeysToUpper.Length == 0) throw new ArgumentNullException(nameof(allowedKeysToUpper));

                string key = '\0'.ToString();

                while (allowedKeysToUpper.ToUpperInvariant().Contains(key) == false)
                {
                    key = Console.ReadKey().KeyChar.ToString().ToUpperInvariant();
                }

                return key;
            }
        }

        /// <summary>
        /// Locks the console and pauses until the user presses any key.
        /// </summary>
        public static void PausePressAnyKey()
        {
            lock (_lockOutput)
            {
                while (Console.KeyAvailable)
                {
                    ReadKeyWithTimeout(TimeSpan.Zero, intercept: true);
                }

                Console.WriteLine("\nPress any key . . .");

                Console.ReadKey(intercept: true);
            }
        }

        /// <summary>
        /// Reads the key with timeout.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <param name="intercept">if set to <c>true</c> [intercept].</param>
        /// <returns></returns>
        public static ConsoleKeyInfo? ReadKeyWithTimeout(TimeSpan timeout, bool intercept = true)
        {
            var tTimeout = Task.Delay(timeout);

            while (!tTimeout.IsCompleted)
            {
                Task.Delay(10).GetAwaiter().GetResult();
                if (!Console.KeyAvailable) continue;
                return Console.ReadKey(intercept);
            }

            return null;
        }

        /// <summary>
        /// Wraps the action in output lock.
        /// </summary>
        /// <param name="action">The action.</param>
        public static void WrapInOutputLock(Action action)
        {
            lock (_lockOutput)
            {
                action?.Invoke();
            }
        }

        /// <summary>
        /// Writes the blanking line.
        /// </summary>
        public static void WriteBlankingLine()
        {
            lock (_lockOutput)
            {
                Console.Write(new string(' ', Console.WindowWidth));
            }
        }

        /// <summary>
        /// Writes with optional enhancements, e.g. This is *enhanced*; this is not.
        /// </summary>
        /// <param name="enhancedString">The enhanced string.</param>
        public static void WriteEnh(string enhancedString)
        {
            if (enhancedString.Any(c => char.IsControl(c)))
            {
                throw new ArgumentException("Enhanced string must not contain control characters.", nameof(enhancedString));
            }

            int visibleLength = 0;

            bool enhanceOn = false;

            lock (_lockOutput)
            {
                foreach (string str in enhancedString.Split('*'))
                {
                    Console.ForegroundColor = enhanceOn ? ConsoleColor.Yellow : ConsoleColor.Gray;

                    Console.Write(str);

                    visibleLength += str.Length;

                    enhanceOn = !enhanceOn; // toggle enhancement
                }
            }
        }

        /// <summary>
        /// Writes the line with optional enhancements, e.g. This is *enhanced*; this is not.
        /// </summary>
        /// <param name="enhancedString">The enhanced string.</param>
        public static void WriteLineEnh(string enhancedString = "")
        {
            lock (_lockOutput)
            {
                WriteEnh(enhancedString);

                Console.Write(new string(' ', WindowWidth - Console.CursorLeft));

                Console.WriteLine();
            }
        }

        public static void WriteLineInColour(string colouredString = "", ConsoleColor colour = ConsoleColor.Gray)
        {
            lock (_lockOutput)
            {
                var previousColour = Console.ForegroundColor;

                Console.ForegroundColor = colour;

                Console.WriteLine(colouredString);
            }
        }

        /// <summary>
        /// Writes the line with an underline that matches its length
        /// </summary>
        /// <param name="enhancedString">The enhanced string.</param>
        /// <param name="underline">The underline character, default '-'</param>
        public static void WriteLineUnderline(string enhancedString, char underline = '-')
        {
            lock (_lockOutput)
            {
                WriteEnh(enhancedString);

                int pos = Console.CursorLeft;

                Console.WriteLine($"\n{new string(underline, pos)}");
            }
        }
    }
}
