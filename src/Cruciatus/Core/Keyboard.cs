namespace Cruciatus.Core
{
    #region using

    using NLog;

    #endregion

    public class Keyboard
    {
        public const string Enter = "{ENTER}";

        public const string Backspace = "{BACKSPACE}";

        public const string CtrlA = "^a";

        public const string CtrlC = "^c";

        public const string CtrlV = "^v";

        private static readonly Logger Logger = CruciatusFactory.Logger;

        public void SendKeys(string text)
        {
            Logger.Info("Send keys '{0}'", text);
            System.Windows.Forms.SendKeys.SendWait(text);
        }

        public void SendEnter()
        {
            SendKeys(Enter);
        }

        public void SendBackspace()
        {
            SendKeys(Backspace);
        }

        public void SendCtrlA()
        {
            SendKeys(CtrlA);
        }

        public void SendCtrlC()
        {
            SendKeys(CtrlC);
        }

        public void SendCtrlV()
        {
            SendKeys(CtrlV);
        }
    }
}
