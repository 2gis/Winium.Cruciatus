
namespace WpfTestApplication.Tests.TestCases
{
    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WpfTestApplication.Tests.Map;

    /// <summary>
    /// The test class 1.
    /// </summary>
    [CodedUITest]
    public partial class TestClass1 : BaseTestClass
    {
        private static WpfTestApplicationApp Application;

        private Menu simpleMenu;

        private RibbonApplicationMenu ribbonMenu;

        private FirstTab firstTab;

        private SecondTab secondTab;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            ClassInitialize(out Application, testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            ClassCleanup(Application);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            this.simpleMenu = Application.MainWindow.SimpleMenu;
            this.ribbonMenu = Application.MainWindow.RibbonMenu;
            this.firstTab = Application.MainWindow.TabItem1;
            this.secondTab = Application.MainWindow.TabItem2;
        }
    }
}
