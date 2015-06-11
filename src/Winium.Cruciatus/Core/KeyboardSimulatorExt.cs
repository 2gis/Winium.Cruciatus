namespace Winium.Cruciatus.Core
{
    #region using

    using System.Threading;

    using NLog;

    using WindowsInput;
    using WindowsInput.Native;

    #endregion

    /// <summary>
    /// Симулятор клавиатуры. Обёртка над WindowsInput.KeyboardSimulator .
    /// </summary>
    public class KeyboardSimulatorExt : IKeyboard
    {
        #region Fields

        private readonly IKeyboardSimulator keyboardSimulator;

        private readonly Logger logger;

        #endregion

        #region Constructors and Destructors

        internal KeyboardSimulatorExt(IKeyboardSimulator keyboardSimulator, Logger logger)
        {
            this.logger = logger;
            this.keyboardSimulator = keyboardSimulator;
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
            this.logger.Info("Key down '{0}'", keyCode.ToString());
            this.keyboardSimulator.KeyDown(keyCode);
            Thread.Sleep(250);
            return this;
        }

        /// <summary>
        /// Эмулирует действие 'нажать и отпустить' над кнопкой.
        /// </summary>
        /// <param name="keyCode">
        /// Ключ целевой кнопки.
        /// </param>
        public void KeyPress(VirtualKeyCode keyCode)
        {
            this.logger.Info("Key press '{0}'", keyCode.ToString());
            this.keyboardSimulator.KeyPress(keyCode);
            Thread.Sleep(250);
        }

        /// <summary>
        /// Эмулирует нажатие сочетания двух кнопок keyCode1 + keyCode2.
        /// </summary>
        /// <param name="keyCode1">
        /// Ключ первой целевой кнопки.
        /// </param>
        /// <param name="keyCode2">
        /// Ключ второй целевой кнопки.
        /// </param>
        public void KeyPressSimultaneous(VirtualKeyCode keyCode1, VirtualKeyCode keyCode2)
        {
            this.logger.Info("Press key combo '{0} + {1}'", keyCode1, keyCode2);
            this.keyboardSimulator.ModifiedKeyStroke(keyCode1, keyCode2);
            Thread.Sleep(250);
        }

        /// <summary>
        /// Эмулирует нажатие сочетания трёх кнопок keyCode1 + keyCode2 + keyCode3.
        /// </summary>
        /// <param name="keyCode1">
        /// Ключ первой целевой кнопки.
        /// </param>
        /// <param name="keyCode2">
        /// Ключ второй целевой кнопки.
        /// </param>
        /// <param name="keyCode3">
        /// Ключ третьей целевой кнопки.
        /// </param>
        public void KeyPressSimultaneous(VirtualKeyCode keyCode1, VirtualKeyCode keyCode2, VirtualKeyCode keyCode3)
        {
            this.logger.Info("Press key combo '{0} + {1} + {2}'", keyCode1, keyCode2, keyCode3);
            this.keyboardSimulator.ModifiedKeyStroke(new[] { keyCode1, keyCode2 }, keyCode3);
            Thread.Sleep(250);
        }

        /// <summary>
        /// Эмулирует действие 'отпустить' над кнопкой.
        /// </summary>
        /// <param name="keyCode">
        /// Ключ целевой кнопки.
        /// </param>
        public IKeyboard KeyUp(VirtualKeyCode keyCode)
        {
            this.logger.Info("Key up '{0}'", keyCode.ToString());
            this.keyboardSimulator.KeyUp(keyCode);
            Thread.Sleep(250);
            return this;
        }

        /// <summary>
        /// Эмулирует нажатие кнопки Backspace.
        /// </summary>
        public IKeyboard SendBackspace()
        {
            this.KeyPress(VirtualKeyCode.BACK);
            return this;
        }

        /// <summary>
        /// Эмулирует нажатие сочетания кнопок Ctrl + A.
        /// </summary>
        public IKeyboard SendCtrlA()
        {
            this.KeyPressSimultaneous(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_A);
            return this;
        }

        /// <summary>
        /// Эмулирует нажатие сочетания кнопок Ctrl + C.
        /// </summary>
        public IKeyboard SendCtrlC()
        {
            this.KeyPressSimultaneous(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);
            return this;
        }

        /// <summary>
        /// Эмулирует нажатие сочетания кнопок Ctrl + V.
        /// </summary>
        public IKeyboard SendCtrlV()
        {
            this.KeyPressSimultaneous(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);
            return this;
        }

        /// <summary>
        /// Эмулирует нажатие кнопки Enter.
        /// </summary>
        public IKeyboard SendEnter()
        {
            this.KeyPress(VirtualKeyCode.RETURN);
            return this;
        }

        /// <summary>
        /// Эмулирует нажатие кнопки Escape.
        /// </summary>
        public IKeyboard SendEscape()
        {
            this.KeyPress(VirtualKeyCode.ESCAPE);
            return this;
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
            if (!string.IsNullOrEmpty(text))
            {
                this.keyboardSimulator.TextEntry(text);
                Thread.Sleep(250);
            }

            return this;
        }

        #endregion
    }
}
