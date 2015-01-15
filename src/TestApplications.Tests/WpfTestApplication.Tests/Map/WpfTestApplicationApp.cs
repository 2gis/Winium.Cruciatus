namespace WpfTestApplication.Tests.Map
{
    #region using

    using System.Windows.Automation;

    using Cruciatus;
    using Cruciatus.Core;

    #endregion

    public class WpfTestApplicationApp : Application
    {
        public WpfTestApplicationApp(string fullPath)
            : base(fullPath)
        {
        }

        public MainWindow MainWindow
        {
            get
            {
                return new MainWindow(CruciatusFactory.Root, By.Uid(TreeScope.Children, "WpfTestApplicationMainWindow"));
            }
        }
    }
}
