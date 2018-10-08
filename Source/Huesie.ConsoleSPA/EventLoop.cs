using System;
using System.Threading;
using System.Threading.Tasks;
using Huesie.ConsoleSPA.Extensions;

namespace Huesie.ConsoleSPA
{
    public static class EventLoop
    {
        private static CancellationTokenSource _cts = new CancellationTokenSource();
        public static CancellationToken CancellationToken { get; } = _cts.Token;

        public static TimeSpan RefreshInterval { get; set; } = TimeSpan.FromSeconds(1);
        public static TimeSpan ShortDelayDuration { get; set; } = TimeSpan.FromMilliseconds(500);

        private static object _lockCriticalOutput = new object();

        private static volatile bool _running = true;

        public static void Run()
        {
            try
            {
                Console.Clear();

                while (_running)
                {
                    ConsoleEx.WrapInOutputLock(() => {
                        Console.CursorVisible = false;
                        Console.SetCursorPosition(0, 0);
                        ConsoleEx.WriteLineEnh(SetTitle.Invoke().FixedWidthEnh(ConsoleEx.WindowWidth - 12) + "    " + DateTime.Now.ToStringHH24mmss());
                        ConsoleEx.WriteLineEnh(SetKeysHelp.Invoke().FixedWidthEnh(ConsoleEx.WindowWidth - 8 /*16*/) + "  " + "- *Q*U|T" /*$" * R*estart - *Q*U|T"*/);

                        OnRefresh?.Invoke();
                        Console.CursorVisible = true;
                    });

                    var keyInfo = ConsoleEx.ReadKeyWithTimeout(RefreshInterval);
                    if (keyInfo == null) continue;

                    if (keyInfo.Value.KeyChar != 'Q' && keyInfo.Value.KeyChar != '|')
                    {
                        OnKey?.Invoke(keyInfo.Value.KeyChar.ToString());
                    }

                    HandleStandardKey(keyInfo);
                }
            }
            finally
            {
                _cts.Cancel();
            }
        }

        private static void HandleStandardKey(ConsoleKeyInfo? keyInfo)
        {
            string key = keyInfo.Value.KeyChar.ToString();

            switch (key)
            {
                case " ": // Refresh
                    Console.Clear();
                    break;

                case "|":
                    ConsoleEx.WriteLineEnh("*Cancelling..*");
                    _cts.Cancel();
                    ShortDelay();
                    Console.Clear();
                    break;

                case "^": // Restart (forget all state)
                    if (ConsoleEx.PromptEnhForInputKeysUpper("Are you sure? Y/N ", "yn") == "Y")
                    {
                        ConsoleEx.WriteLineEnh("*Restarting..* (if supported)"); ShortDelay();
                        OnRestart?.Invoke(); ShortDelay();
                        Console.Clear();
                    }
                    else
                    {
                        ShortDelay(); Console.Clear();
                    }
                    break;

                case "q": // Quit (lower)
                    if (ConsoleEx.PromptEnhForInputKeysUpper("Are you sure? Y/N ", "yn") == "Y")
                    {
                        ConsoleEx.WriteLineEnh("\n*Quitting..*"); ShortDelay();
                        OnQuit?.Invoke(); ShortDelay();
                        _running = false;
                    }
                    else
                    {
                        ShortDelay(); Console.Clear();
                    }
                    break;
                case "Q": // Quit (upper)
                    ConsoleEx.WriteLineEnh("*Quitting..*"); ShortDelay();
                    OnQuit?.Invoke(); ShortDelay();
                    _running = false;
                    break;

                default:
                    break;
            }
        }

        private static void ShortDelay()
        {
            Task.Delay(ShortDelayDuration).GetAwaiter().GetResult();
        }

        public static Func<string> SetTitle { get; set; }
        public static Func<string> SetKeysHelp { get; set; }

        public static Action<string> OnKey { get; set; }
        public static Action OnRefresh { get; set; }
        public static Action OnQuit { get; set; }
        public static Action OnRestart { get; set; }
    }
}
