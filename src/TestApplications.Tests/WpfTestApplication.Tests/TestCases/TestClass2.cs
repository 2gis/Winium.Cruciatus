
namespace WpfTestApplication.Tests.TestCases
{
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WpfTestApplication.Tests.Map;

    /// <summary>
    /// The test class 2.
    /// </summary>
    [CodedUITest]
    public partial class TestClass2 : BaseTestClass
    {
        private static WpfTestApplicationApp Application;

        private FirstRibbonTab firstRibbonTab;

        private SecondRibbonTab secondRibbonTab;

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
            this.firstRibbonTab = Application.MainWindow.RibbonTabItem1;
            this.secondRibbonTab = Application.MainWindow.RibbonTabItem2;
        }
    }
}
