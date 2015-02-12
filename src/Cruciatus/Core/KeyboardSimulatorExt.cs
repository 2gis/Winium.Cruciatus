namespace Cruciatus.Core
{
    #region using

    using System.Threading;

    using WindowsInput;
    using WindowsInput.Native;

    using NLog;

    #endregion

    public class KeyboardSimulatorExt : IKeyboard
    {
        private readonly Logger _logger;

        private readonly IKeyboardSimulator _keyboardSimulator;

        public KeyboardSimulatorExt(IKeyboardSimulator keyboardSimulator, Logger logger)
        {
            _logger = logger;
            _keyboardSimulator = keyboardSimulator;
        }

        public IKeyboard SendText(string text)
        {
            _logger.Info("Send text '{0}'", text);
            if (!string.IsNullOrEmpty(text))
            {
                _keyboardSimulator.TextEntry(text);
                Thread.Sleep(250);
            }

            return this;
        }

        public IKeyboard SendEnter()
        {
            KeyPress(VirtualKeyCode.RETURN);
            return this;
        }

        public IKeyboard SendBackspace()
        {
            KeyPress(VirtualKeyCode.BACK);
            return this;
        }

        public IKeyboard SendEscape()
        {
            KeyPress(VirtualKeyCode.ESCAPE);
            return this;
        }

        public IKeyboard SendCtrlA()
        {
            KeyPressSimultaneous(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_A);
            return this;
        }

        public IKeyboard SendCtrlC()
        {
            KeyPressSimultaneous(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);
            return this;
        }

        public IKeyboard SendCtrlV()
        {
            KeyPressSimultaneous(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);
            return this;
        }

        public void KeyPress(VirtualKeyCode keyCode)
        {
            _logger.Info("Key press '{0}'", keyCode.ToString());
            _keyboardSimulator.KeyPress(keyCode);
            Thread.Sleep(250);
        }

        public void KeyPressSimultaneous(VirtualKeyCode keyCode1, VirtualKeyCode keyCode2)
        {
            _logger.Info("Press key combo '{0} + {1}'", keyCode1, keyCode2);
            _keyboardSimulator.ModifiedKeyStroke(keyCode1, keyCode2);
            Thread.Sleep(250);
        }

        public void KeyPressSimultaneous(VirtualKeyCode keyCode1, VirtualKeyCode keyCode2, VirtualKeyCode keyCode3)
        {
            _logger.Info("Press key combo '{0} + {1} + {2}'", keyCode1, keyCode2, keyCode3);
            _keyboardSimulator.ModifiedKeyStroke(new[] { keyCode1, keyCode2 }, keyCode3);
            Thread.Sleep(250);
        }
    }
}
