namespace WindowsFormsTestApplication.Tests.Map
{
    #region using

    using Cruciatus;
    using Cruciatus.Core;

    #endregion

    public class WindowsFormsTestApplicationApp : Application
    {
        public WindowsFormsTestApplicationApp(string fullPath)
            : base(fullPath)
        {
        }

        public MainWindow Window
        {
            get
            {
                return new MainWindow(CruciatusFactory.Root, By.Uid("Form1"));
            }
        }
    }
}
