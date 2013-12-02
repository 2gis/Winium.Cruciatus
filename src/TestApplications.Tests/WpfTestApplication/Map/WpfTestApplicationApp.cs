
namespace WpfTestApplication.Map
{
    using Cruciatus;

    public class WpfTestApplicationApp : Application<MainWindow>
    {
        public WpfTestApplicationApp(string fullPath)
            : base(fullPath, "WpfTestApplicationMainWindow")
        {
        }
    }
}