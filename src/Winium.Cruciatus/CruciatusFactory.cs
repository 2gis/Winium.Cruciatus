namespace Winium.Cruciatus
{
    #region using

    using System.Windows.Automation;

    using NLog;
    using NLog.Config;
    using NLog.Targets;

    using WindowsInput;

    using Winium.Cruciatus.Core;
    using Winium.Cruciatus.Elements;
    using Winium.Cruciatus.Exceptions;
    using Winium.Cruciatus.Settings;

    #endregion

    /// <summary>
    /// Класс доступа к инфраструктуре Cruciatus.
    /// </summary>
    public static class CruciatusFactory
    {
        #region Static Fields

        private static KeyboardSimulatorExt keyboardSimulatorExt;

        private static MouseSimulatorExt mouseSimulatorExt;

        private static Screenshoter screenshoter;

        private static SendKeysExt sendKeysExt;

        #endregion

        #region Constructors and Destructors

        static CruciatusFactory()
        {
            LoggerInit();
            InputSimulatorsInit();
            ScreenshotersInit();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Возвращает элемент находящийся в фокусе.
        /// </summary>
        public static CruciatusElement FocusedElement
        {
            get
            {
                return new CruciatusElement(null, AutomationElement.FocusedElement, null);
            }
        }

        /// <summary>
        /// Возвращает текущий симулятор клавиатуры.
        /// </summary>
        public static IKeyboard Keyboard
        {
            get
            {
                return GetSpecificKeyboard(Settings.KeyboardSimulatorType);
            }
        }

        /// <summary>
        /// Возвращает объект, используемый для ведения логов.
        /// </summary>
        public static Logger Logger
        {
            get
            {
                return LogManager.GetLogger("cruciatus");
            }
        }

        /// <summary>
        /// Возвращает симулятор мыши.
        /// </summary>
        public static MouseSimulatorExt Mouse
        {
            get
            {
                return mouseSimulatorExt;
            }
        }

        /// <summary>
        /// Возвращает корневой элемент - Рабочий стол.
        /// </summary>
        public static CruciatusElement Root
        {
            get
            {
                return new CruciatusElement(null, AutomationElement.RootElement, null);
            }
        }

        /// <summary>
        /// Возвращает объект, используемый для снятия скриншотов.
        /// </summary>
        public static IScreenshoter Screenshoter
        {
            get
            {
                return screenshoter;
            }
        }

        /// <summary>
        /// Возвращает объект по управлению настройками Crucaitus.
        /// </summary>
        public static CruciatusSettings Settings
        {
            get
            {
                return CruciatusSettings.Instance;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Возвращает специфичный симулятор клавиатуры по заданному типу.
        /// </summary>
        /// <param name="keyboardSimulatorType">
        /// Тип симулятора клавиатуры.
        /// </param>
        public static IKeyboard GetSpecificKeyboard(KeyboardSimulatorType keyboardSimulatorType)
        {
            switch (keyboardSimulatorType)
            {
                case KeyboardSimulatorType.BasedOnInputSimulatorLib:
                    return keyboardSimulatorExt;
                case KeyboardSimulatorType.BasedOnWindowsFormsSendKeysClass:
                    return sendKeysExt;
            }

            throw new CruciatusException("Unknown KeyboardSimulatorType");
        }

        #endregion

        #region Methods

        private static void InputSimulatorsInit()
        {
            var inputSimulator = new InputSimulator();
            keyboardSimulatorExt = new KeyboardSimulatorExt(inputSimulator.Keyboard, Logger);
            mouseSimulatorExt = new MouseSimulatorExt(inputSimulator.Mouse);

            sendKeysExt = new SendKeysExt(Logger);
        }

        private static void LoggerInit()
        {
            // Step 0. Not override if there is some configuration
            if (LogManager.Configuration != null)
            {
                return;
            }

            // Step 1. Create configuration object 
            var config = new LoggingConfiguration();

            // Step 2. Create targets and add them to the configuration 
            var consoleTarget = new ConsoleTarget();
            config.AddTarget("console", consoleTarget);

            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);

            const string Layout =
                @"[${date:format=HH\:mm\:ss}] [${level}] ${message} "
                + "${onexception:${exception:format=tostring,stacktrace}${newline}${stacktrace}}";

            // Step 3. Set target properties 
            consoleTarget.Layout = Layout;
            fileTarget.FileName = "Cruciatus.log";
            fileTarget.Layout = Layout;

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
            screenshoter = new Screenshoter();
        }

        #endregion
    }
}
