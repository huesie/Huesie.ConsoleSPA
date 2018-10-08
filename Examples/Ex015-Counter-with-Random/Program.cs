using System;
using Ex015_Counter_with_Random.Extensions;
using Huesie.ConsoleSPA;

namespace Ex015_Counter_with_Random
{
    class Program
    {
        static long _counter = 100;

        static bool _randomSteps = false;
        static int _step = 1;

        static void Main(string[] args)
        {
            // Setup event loop
            EventLoop.SetTitle = () => $"Counter example with Random ({(_randomSteps ? $"*ON*: Random step of *{_step}*" : "off")})";
            EventLoop.SetKeysHelp = () => "KEYS: *R*andom toggle - *S*et step";

            // OnRefresh - runs every ~1sec
            EventLoop.OnRefresh = () => {
                ConsoleEx.WriteBlankingLine();

                _counter += (!_randomSteps) ? 0 : (RandomEx.GetThreadRandom().Next(2) * 2 - 1) * _step; // +/- step
                ConsoleEx.WriteLineEnh($"Counter: *{_counter}*");

                ConsoleEx.WriteBlankingLine();
            };

            // Key presses
            EventLoop.OnKey = (key) => {
                switch (key.ToUpperInvariant()) {
                    case "R": RandomStepsToggle(); break;
                    case "S": SetStepValue(); break;
                }
            };

            EventLoop.Run();
        }

        static void RandomStepsToggle()
        {
            ConsoleEx.WriteLineEnh("Toggling random steps mode..");
            EventLoop.ShortDelay();
            _randomSteps = !_randomSteps;
            Console.Clear();
        }

        static void SetStepValue()
        {
            ConsoleEx.WriteEnh("(non-int or blank will cancel) *Set value of each random step*: ");
            var stringValue = Console.ReadLine();
            int.TryParse(!string.IsNullOrWhiteSpace(stringValue) ? stringValue : _counter.ToString(), out _step);
            Console.Clear();
        }
    }
}
