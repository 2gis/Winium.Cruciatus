namespace WpfTestApplication.Tests.Map
{
    #region using

    using System.Windows.Automation;

    using Winium.Cruciatus;
    using Winium.Cruciatus.Core;

    #endregion

    public class WpfTestApplicationApp : Application
    {
        #region Constructors and Destructors

        public WpfTestApplicationApp(string fullPath)
            : base(fullPath)
        {
        }

        #endregion

        #region Public Properties

        public MainWindow MainWindow
        {
            get
            {
                return new MainWindow(CruciatusFactory.Root, By.Uid(TreeScope.Children, "WpfTestApplicationMainWindow"));
            }
        }

        #endregion
    }
}
