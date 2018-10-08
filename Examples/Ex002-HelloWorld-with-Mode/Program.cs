using System;
using Huesie.ConsoleSPA;
using Ex002_HelloWorld_with_Mode.Enums;

namespace Ex002_HelloWorld_with_Mode
{
    class Program
    {
        static DisplayMode _mode = DisplayMode.Plain;

        static void Main(string[] args)
        {
            // Setup event loop
            EventLoop.SetTitle = () => $"Ex002: HelloWorld with Mode: *{_mode}*";
            EventLoop.SetKeysHelp = () => "KEYS: *T*oggle mode";

            // OnRefresh - runs every ~1sec
            EventLoop.OnRefresh = () => {
                string text = ConsoleEx.Highlight("hello, world", _mode == DisplayMode.Highlighted);
                ConsoleEx.WriteLineEnh($"\n{text} - printed at {DateTime.Now}");
            };

            // Key presses
            EventLoop.OnKey = (key) => {
                switch (key.ToUpperInvariant()) {
                    case "T":
                        ConsoleEx.WriteLineEnh("Toggling mode.."); EventLoop.ShortDelay();
                        _mode = 1 - _mode;
                        Console.Clear();
                        break;
                }
            };

            EventLoop.Run();
        }
    }
}
