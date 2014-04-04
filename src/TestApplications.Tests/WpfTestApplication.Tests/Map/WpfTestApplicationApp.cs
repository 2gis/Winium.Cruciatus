namespace WpfTestApplication.Tests.Map
{
    #region using

    using Cruciatus;

    #endregion

    public class WpfTestApplicationApp : Application<MainWindow>
    {
        public WpfTestApplicationApp(string fullPath)
            : base(fullPath, "WpfTestApplicationMainWindow")
        {
        }
    }
}
