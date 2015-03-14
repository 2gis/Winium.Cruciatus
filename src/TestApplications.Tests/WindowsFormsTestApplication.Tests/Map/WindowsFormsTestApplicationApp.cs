namespace WindowsFormsTestApplication.Tests.Map
{
    #region using

    using Winium.Cruciatus;
    using Winium.Cruciatus.Core;

    #endregion

    public class WindowsFormsTestApplicationApp : Application
    {
        #region Constructors and Destructors

        public WindowsFormsTestApplicationApp(string fullPath)
            : base(fullPath)
        {
        }

        #endregion

        #region Public Properties

        public MainWindow Window
        {
            get
            {
                return new MainWindow(CruciatusFactory.Root, By.Uid("Form1"));
            }
        }

        #endregion
    }
}
