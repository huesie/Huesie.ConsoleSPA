using System;
using System.Threading;

namespace Ex015_Counter_with_Random.Extensions
{
    static class RandomEx
    {
        private static int seed = Environment.TickCount;

        private static ThreadLocal<Random> randomWrapper = new ThreadLocal<Random>(() =>
            new Random(Interlocked.Increment(ref seed))
        );

        /// <summary>
        /// Gets a random instance for thread.
        /// </summary>
        /// <returns>Random from <c>ThreadLocal</c></returns>
        public static Random GetThreadRandom()
        {
            return randomWrapper.Value;
        }
    }
}
