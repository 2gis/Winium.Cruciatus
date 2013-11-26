
namespace Cruciatus
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    public static class CruciatusFactory
    {
        private const int MouseMoveSpeed = 2500;

        private const int WaitPeriod = 100;

        private const int WaitingTime = 5000;

        public static TOut WaitingValues<T, TOut>(
            T variable,
            Func<T, TOut> getValueFunc,
            Func<TOut, bool> compareFunc,
            int waitingTime = WaitingTime)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var value = getValueFunc(variable);
            while (compareFunc(value))
            {
                Thread.Sleep(WaitPeriod);
                if (stopwatch.ElapsedMilliseconds > waitingTime)
                {
                    break;
                }

                value = getValueFunc(variable);
            }

            stopwatch.Stop();
            return value;
        }
    }
}