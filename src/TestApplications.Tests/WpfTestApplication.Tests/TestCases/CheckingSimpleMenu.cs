
namespace WpfTestApplication.Tests.TestCases
{
    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The wpf test application test class 1.
    /// </summary>
    [CodedUITest]
    public partial class WpfTestApplicationTestClass1 : WpfTestApplicationTestCase
    {
        private Menu simpleMenu;
        private RibbonApplicationMenu ribbonMenu;

        [TestInitialize]
        public void MyInitialize()
        {
            this.simpleMenu = Application.MainWindow.SimpleMenu;
            this.ribbonMenu = Application.MainWindow.RibbonMenu;
        }

        [TestMethod]
        public void SimpleMenuTestMethod1()
        {
            const string HeadersPath = "Level1$MultiLevel2$Level3";
            Assert.IsTrue(this.simpleMenu.Select(HeadersPath), this.simpleMenu.LastErrorMessage);
        }

        [TestMethod]
        public void SimpleMenuTestMethod2()
        {
            const string HeadersPath = "Level1$MultiLevel2$MultiLevel3$MultiLevel4$Level5";
            Assert.IsTrue(this.simpleMenu.Select(HeadersPath), this.simpleMenu.LastErrorMessage);
        }
    }
}