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
    using System.Windows.Automation;

    using WindowsInput;

    using Cruciatus.Core;
    using Cruciatus.Elements;
    using Cruciatus.Exceptions;
    using Cruciatus.Settings;

    using NLog;
    using NLog.Config;
    using NLog.Targets;

    #endregion

    public static class CruciatusFactory
    {
        private static KeyboardSimulatorExt _keyboardSimulatorExt;

        private static SendKeysExt _sendKeysExt;

        private static Mouse _mouse;

        private static Screenshoter _screenshoter;

        static CruciatusFactory()
        {
            LoggerInit();
            InputSimulatorsInit();
            ScreenshotersInit();
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
                return new CruciatusElement(null, AutomationElement.RootElement, null);
            }
        }

        public static IKeyboard Keyboard
        {
            get
            {
                return GetCurrentKeyboard();
            }
        }

        public static Mouse Mouse
        {
            get
            {
                return _mouse;
            }
        }

        public static IScreenshoter Screenshoter
        {
            get
            {
                return _screenshoter;
            }
        }

        private static void InputSimulatorsInit()
        {
            var inputSimulator = new InputSimulator();
            _keyboardSimulatorExt = new KeyboardSimulatorExt(inputSimulator.Keyboard, Logger);
            _mouse = new Mouse(inputSimulator.Mouse);

            _sendKeysExt = new SendKeysExt(Logger);
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

        private static void ScreenshotersInit()
        {
            _screenshoter = new Screenshoter();
        }

        private static IKeyboard GetCurrentKeyboard()
        {
            switch (Settings.KeyboardSimulatorType)
            {
                case KeyboardSimulatorType.BasedOnInputSimulatorLib:
                    return _keyboardSimulatorExt;
                case KeyboardSimulatorType.BasedOnWindowsFormsSendKeysClass:
                    return _sendKeysExt;
            }

            throw new CruciatusException("Unknown KeyboardSimulatorType");
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
