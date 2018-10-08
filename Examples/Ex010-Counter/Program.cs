using Huesie.ConsoleSPA;
using System;

namespace Ex010_Counter
{
    class Program
    {
        static long _counter;

        static void Main(string[] args)
        {
            // Setup event loop
            EventLoop.SetTitle = () => "Counter example";
            EventLoop.SetKeysHelp = () => "KEYS: *I*ncrement - *D*ecrement - *S*et value";

            // OnRefresh - runs every ~1sec
            EventLoop.OnRefresh = () => {
                ConsoleEx.WriteBlankingLine();
                ConsoleEx.WriteLineEnh($"Counter: *{_counter}*");
                ConsoleEx.WriteBlankingLine();
            };

            // Key presses
            EventLoop.OnKey = (key) => {
                switch (key.ToUpperInvariant())
                {
                    case "I": Increment(); break;
                    case "D": Decrement(); break;
                    case "S": SetValue(); break;
                }
            };

            EventLoop.Run();
        }

        static void Increment()
        {
            ConsoleEx.WriteLineEnh("Incrementing..");
            EventLoop.ShortDelay();
            Console.Clear();
            _counter++;
        }

        static void Decrement()
        {
            ConsoleEx.WriteLineEnh("Decrementing..");
            EventLoop.ShortDelay();
            Console.Clear();
            _counter--;

        }

        static void SetValue()
        {
            ConsoleEx.WriteEnh("(non-int or blank will cancel) *Set counter value*: ");
            var stringCounter = Console.ReadLine();
            long.TryParse(!string.IsNullOrWhiteSpace(stringCounter) ? stringCounter : _counter.ToString(), out _counter);
            Console.Clear();
        }
    }
}
