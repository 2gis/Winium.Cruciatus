namespace Cruciatus.Core
{
    #region using

    using NLog;

    #endregion

    public static class Keyboard
    {
        public const string Enter = "{ENTER}";

        public const string Backspace = "{BACKSPACE}";

        public const string Escape = "{ESCAPE}";

        public const string CtrlA = "^a";

        public const string CtrlC = "^c";

        public const string CtrlV = "^v";

        private static readonly Logger Logger = CruciatusFactory.Logger;

        public static void SendKeys(string text)
        {
            Logger.Info("Send keys '{0}'", text);
            System.Windows.Forms.SendKeys.SendWait(text);
        }

        public static void SendEnter()
        {
            SendKeys(Enter);
        }

        public static void SendBackspace()
        {
            SendKeys(Backspace);
        }

        public static void SendCtrlA()
        {
            SendKeys(CtrlA);
        }

        public static void SendCtrlC()
        {
            SendKeys(CtrlC);
        }

        public static void SendCtrlV()
        {
            SendKeys(CtrlV);
        }
    }
}
