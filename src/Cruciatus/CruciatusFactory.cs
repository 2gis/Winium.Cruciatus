// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CruciatusFactory.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет фабрику Cruciatus.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus
{
    #region using

    using System;
    using System.Diagnostics;
    using System.Threading;

    using Cruciatus.Settings;

    #endregion

    public static class CruciatusFactory
    {
        public static CruciatusSettings Settings
        {
            get
            {
                return CruciatusSettings.Instance;
            }
        }

        internal static TOut WaitingValues<TOut>(
            Func<TOut> getValueFunc, 
            Func<TOut, bool> compareFunc)
        {
            return WaitingValues(getValueFunc, compareFunc, Settings.WaitForGetValueTimeout);
        }

        internal static TOut WaitingValues<TOut>(
            Func<TOut> getValueFunc, 
            Func<TOut, bool> compareFunc, 
            int waitingTime)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var value = getValueFunc();
            while (compareFunc(value))
            {
                Thread.Sleep(Settings.WaitingPeriod);
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
