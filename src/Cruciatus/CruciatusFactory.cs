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
    using System.Linq;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows.Automation;

    using Cruciatus.Elements;
    using Cruciatus.Settings;

    using NLog;
    using NLog.Config;
    using NLog.Targets;

    #endregion

    public static class CruciatusFactory
    {
        static CruciatusFactory()
        {
            LoggerInit();
        }

        public static Logger Logger
        {
            get
            {
                return LogManager.GetLogger("cruciatus");
            }
        }

        public static CruciatusSettings Settings
        {
            get
            {
                return CruciatusSettings.Instance;
            }
        }

        public static CruciatusElement Root
        {
            get
            {
                return new CruciatusElement(null, RootAe, null);
            }
        }

        internal static AutomationElement RootAe
        {
            get
            {
                return AutomationElement.RootElement;
            }
        }

        private static void LoggerInit()
        {
            // Step 1. Create configuration object 
            var config = new LoggingConfiguration();

            // Step 2. Create targets and add them to the configuration 
            var consoleTarget = new ConsoleTarget();
            config.AddTarget("console", consoleTarget);

            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);

            const string layout = @"[${date:format=HH\:mm\:ss}] [${level}] ${message} "
                                  + "${onexception:${exception:format=tostring,stacktrace}${newline}${stacktrace}}";

            // Step 3. Set target properties 
            consoleTarget.Layout = layout;
            fileTarget.FileName = "Cruciatus.log";
            fileTarget.Layout = layout;

            // Step 4. Define rules
            var rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
            config.LoggingRules.Add(rule1);

            var rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule2);

            // Step 5. Activate the configuration
            LogManager.Configuration = config;
        }

        internal static AutomationElement Find(AutomationElement parent, string automationId, TreeScope scope)
        {
            var element = parent;
            var uids = automationId.Split('/');
            for (var i = 0; i < uids.Count(); ++i)
            {
                var condition = new PropertyCondition(AutomationElement.AutomationIdProperty, uids[i]);
                var currentParent = element;
                element = WaitingValues(() => currentParent.FindFirst(scope, condition), value => value == null,
                                        Settings.SearchTimeout);
            }

            return element;
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
