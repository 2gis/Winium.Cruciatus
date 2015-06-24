namespace Winium.Cruciatus.Core
{
    #region using

    using System;
    using System.Threading;
    using System.Windows.Forms;

    using NLog;

    using WindowsInput.Native;

    #endregion

    /// <summary>
    /// Симулятор клавиатуры. Обёртка над System.Windows.Forms.SendKeys .
    /// </summary>
    public class SendKeysExt : IKeyboard
    {
        #region Constants

        /// <summary>
        /// Кнопка Alt.
        /// </summary>
        public const char Alt = '%';

        /// <summary>
        /// Кнопка Backspace.
        /// </summary>
        public const string Backspace = "{BACKSPACE}";

        /// <summary>
        /// Кнопка Ctrl.
        /// </summary>
        public const char Ctrl = '^';

        /// <summary>
        /// Кнопка Enter.
        /// </summary>
        public const string Enter = "{ENTER}";

        /// <summary>
        /// Кнопка Escape.
        /// </summary>
        public const string Escape = "{ESCAPE}";

        /// <summary>
        /// Кнопка +
        /// </summary>
        public const char Shift = '+';

        #endregion

        #region Fields

        private readonly Logger logger;

        #endregion

        #region Constructors and Destructors

        internal SendKeysExt(Logger logger)
        {
            this.logger = logger;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Эмулирует действие 'нажать и держать' над кнопкой.
        /// </summary>
        /// <param name="keyCode">
        /// Ключ целевой кнопки.
        /// </param>
        public IKeyboard KeyDown(VirtualKeyCode keyCode)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Эмулирует действие 'отпустить' над кнопкой.
        /// </summary>
        /// <param name="keyCode">
        /// Ключ целевой кнопки.
        /// </param>
        public IKeyboard KeyUp(VirtualKeyCode keyCode)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Эмулирует нажатие кнопки Backspace.
        /// </summary>
        public IKeyboard SendBackspace()
        {
            return this.SendKeysPrivate(Backspace);
        }

        /// <summary>
        /// Эмулирует нажатие сочетания кнопок Ctrl + A.
        /// </summary>
        public IKeyboard SendCtrlA()
        {
            return this.SendKeysPrivate(Ctrl + "a");
        }

        /// <summary>
        /// Эмулирует нажатие сочетания кнопок Ctrl + C.
        /// </summary>
        public IKeyboard SendCtrlC()
        {
            return this.SendKeysPrivate(Ctrl + "c");
        }

        /// <summary>
        /// Эмулирует нажатие сочетания кнопок Ctrl + V.
        /// </summary>
        public IKeyboard SendCtrlV()
        {
            return this.SendKeysPrivate(Ctrl + "v");
        }

        /// <summary>
        /// Эмулирует нажатие кнопки Enter.
        /// </summary>
        public IKeyboard SendEnter()
        {
            return this.SendKeysPrivate(Enter);
        }

        /// <summary>
        /// Эмулирует нажатие кнопки Escape.
        /// </summary>
        public IKeyboard SendEscape()
        {
            return this.SendKeysPrivate(Escape);
        }

        /// <summary>
        /// Эмулирует ввод текста.
        /// </summary>
        /// <param name="text">
        /// Текст.
        /// </param>
        public IKeyboard SendText(string text)
        {
            this.logger.Info("Send text '{0}'", text);
            return this.SendWaitPrivate(text);
        }

        #endregion

        #region Methods

        private IKeyboard SendKeysPrivate(string keys)
        {
            this.logger.Info("Send keys '{0}'", keys);
            return this.SendWaitPrivate(keys);
        }

        private IKeyboard SendWaitPrivate(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                SendKeys.SendWait(text);
                Thread.Sleep(250);
            }

            return this;
        }

        #endregion
    }
}
