
namespace WpfTestApplication.Tests.TestCases
{
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WpfTestApplication.Tests.Map;

    [CodedUITest]
    public class CheckingAllInRibbonTabItem2TestCase : WpfTestApplicationTestCase
    {
        private SecondRibbonTab tab;

        [TestInitialize]
        public void MyInitialize()
        {
            this.tab = Application.MainWindow.RibbonTabItem2;
            Assert.IsTrue(this.tab.Select(), this.tab.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingRibbonTabItem2()
        {
            Assert.IsTrue(this.tab.Select(), this.tab.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingRibbonCheckBox()
        {
            Assert.IsTrue(this.tab.RibbonCheckBox.UnCheck(), this.tab.RibbonCheckBox.LastErrorMessage);
            Assert.IsFalse(this.tab.RibbonCheckBox.IsChecked, "Чекбокс в check состоянии после uncheck.");

            Assert.IsTrue(this.tab.RibbonCheckBox.Check(), this.tab.RibbonCheckBox.LastErrorMessage);
            Assert.IsTrue(this.tab.RibbonCheckBox.IsChecked, "Чекбокс в uncheck состоянии после check.");
        }

        [TestMethod]
        public void CheckingRibbonToggleButton()
        {
            Assert.IsTrue(this.tab.RibbonToggleButton.UnCheck(), this.tab.RibbonToggleButton.LastErrorMessage);
            Assert.IsFalse(this.tab.RibbonToggleButton.IsChecked, "Чекбокс в check состоянии после uncheck.");

            Assert.IsTrue(this.tab.RibbonToggleButton.Check(), this.tab.RibbonToggleButton.LastErrorMessage);
            Assert.IsTrue(this.tab.RibbonToggleButton.IsChecked, "Чекбокс в uncheck состоянии после check.");
        }
    }
}