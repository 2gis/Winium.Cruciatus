namespace Cruciatus.Core
{
    #region using

    using System;
    using System.Threading;

    using WindowsInput;
    using WindowsInput.Native;

    using NLog;

    #endregion

    public class KeyboardSimulatorExt : IKeyboard
    {
        private readonly Logger _logger;

        private readonly IKeyboardSimulator _keyboardSimulator;

        public KeyboardSimulatorExt(Logger logger, IKeyboardSimulator keyboardSimulator)
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
            KeyPressWithModifier(VirtualKeyCode.VK_A, VirtualKeyCode.CONTROL);
            return this;
        }

        public IKeyboard SendCtrlC()
        {
            KeyPressWithModifier(VirtualKeyCode.VK_C, VirtualKeyCode.CONTROL);
            return this;
        }

        public IKeyboard SendCtrlV()
        {
            KeyPressWithModifier(VirtualKeyCode.VK_V, VirtualKeyCode.CONTROL);
            return this;
        }

        private void KeyPress(VirtualKeyCode keyCode)
        {
            _logger.Info("Key press '{0}'", keyCode.ToString());
            _keyboardSimulator.KeyPress(keyCode);
            Thread.Sleep(250);
        }

        private void KeyPressWithModifier(VirtualKeyCode keyCode, VirtualKeyCode modifierKeyCode)
        {
            _logger.Info("Press key combo '{0} + {1}'", modifierKeyCode, keyCode);
            _keyboardSimulator.ModifiedKeyStroke(modifierKeyCode, keyCode);
            Thread.Sleep(250);
        }
    }
}
