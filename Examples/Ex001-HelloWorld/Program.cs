using System;
using Huesie.ConsoleSPA;

namespace Ex001_HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            // OnRefresh - runs every ~1sec
            EventLoop.OnRefresh = () => {
                ConsoleEx.WriteLineEnh($"*hello, world* - printed at *{DateTime.Now}*");
            };

            EventLoop.Run();
        }
    }
}
