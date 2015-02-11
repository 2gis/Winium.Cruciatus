namespace Cruciatus.Core
{
    #region using

    using System.Threading;
    using System.Windows.Forms;

    using NLog;

    #endregion

    public class SendKeysExt : IKeyboard
    {
        public const string Enter = "{ENTER}";

        public const string Backspace = "{BACKSPACE}";

        public const string Escape = "{ESCAPE}";

        public const char Ctrl = '^';

        public const char Alt = '%';

        public const char Shift = '+';

        private readonly Logger _logger;

        public SendKeysExt(Logger logger)
        {
            _logger = logger;
        }

        public IKeyboard SendText(string text)
        {
            _logger.Info("Send text '{0}'", text);
            return SendWaitEx(text);
        }

        public IKeyboard SendEnter()
        {
            return SendKeysPrivate(Enter);
        }

        public IKeyboard SendBackspace()
        {
            return SendKeysPrivate(Backspace);
        }

        public IKeyboard SendEscape()
        {
            return SendKeysPrivate(Escape);
        }

        public IKeyboard SendCtrlA()
        {
            return SendKeysPrivate(Ctrl + "a");
        }

        public IKeyboard SendCtrlC()
        {
            return SendKeysPrivate(Ctrl + "c");
        }

        public IKeyboard SendCtrlV()
        {
            return SendKeysPrivate(Ctrl + "v");
        }

        private IKeyboard SendKeysPrivate(string keys)
        {
            _logger.Info("Send keys '{0}'", keys);
            return SendWaitEx(keys);
        }

        private IKeyboard SendWaitEx(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                SendKeys.SendWait(text);
                Thread.Sleep(250);
            }

            return this;
        }
    }
}
