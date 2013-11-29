
namespace Cruciatus
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    public static class CruciatusFactory
    {
        private const int MouseMoveSpeed = 2500;

        private const int WaitPeriod = 25;

        private const int WaitingTime = 7500;

        public static TOut WaitingValues<TOut>(
            Func<TOut> getValueFunc,
            Func<TOut, bool> compareFunc,
            int waitingTime = WaitingTime)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var value = getValueFunc();
            while (compareFunc(value))
            {
                Thread.Sleep(WaitPeriod);
                if (stopwatch.ElapsedMilliseconds > waitingTime)
                {
                    break;
                }

                value = getValueFunc();
            }

            stopwatch.Stop();
            return value;
        }
    }
}