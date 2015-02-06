namespace Cruciatus.Core
{
    #region using

    using System;

    using WindowsInput;
    using WindowsInput.Native;

    using NLog;

    #endregion

    public class Keyboard
    {
        private readonly Logger _logger;

        private readonly IKeyboardSimulator _keyboardSimulator;

        public Keyboard(Logger logger, IKeyboardSimulator keyboardSimulator)
        {
            _logger = logger;
            _keyboardSimulator = keyboardSimulator;
        }

        [Obsolete("SendKeys is deprecated, please use SendText instead.")]
        public Keyboard SendKeys(string text)
        {
            return SendText(text);
        }

        public Keyboard SendText(string text)
        {
            _logger.Info("Send text '{0}'", text);
            _keyboardSimulator.TextEntry(text);
            return this;
        }

        public Keyboard SendEnter()
        {
            KeyPress(VirtualKeyCode.RETURN);
            return this;
        }

        public Keyboard SendBackspace()
        {
            KeyPress(VirtualKeyCode.BACK);
            return this;
        }

        public Keyboard SendEscape()
        {
            KeyPress(VirtualKeyCode.ESCAPE);
            return this;
        }

        public Keyboard SendCtrlA()
        {
            KeyPressWithModifier(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_A);
            return this;
        }

        public Keyboard SendCtrlC()
        {
            KeyPressWithModifier(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);
            return this;
        }

        public Keyboard SendCtrlV()
        {
            KeyPressWithModifier(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);
            return this;
        }

        private IKeyboardSimulator KeyPress(VirtualKeyCode keyCode)
        {
            _logger.Info("Key press '{0}'", keyCode.ToString());
            return _keyboardSimulator.KeyPress(keyCode);
        }

        private IKeyboardSimulator KeyPressWithModifier(VirtualKeyCode keyCode, VirtualKeyCode modifierKeyCode)
        {
            _logger.Info("Press key combo '{0} + {1}'", modifierKeyCode, keyCode);
            return _keyboardSimulator.ModifiedKeyStroke(modifierKeyCode, keyCode);
        }
    }
}
