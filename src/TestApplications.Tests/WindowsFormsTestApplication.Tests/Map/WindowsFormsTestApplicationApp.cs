namespace WindowsFormsTestApplication.Tests.Map
{
    #region using

    using Cruciatus;

    #endregion

    public class WindowsFormsTestApplicationApp : Application<MainWindow>
    {
        public WindowsFormsTestApplicationApp(string fullPath)
            : base(fullPath, "Form1")
        {
        }
    }
}
