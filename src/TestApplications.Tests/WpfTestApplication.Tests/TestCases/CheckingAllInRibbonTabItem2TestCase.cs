
namespace WpfTestApplication.Tests.TestCases
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The test class 2.
    /// </summary>
    public partial class TestClass2
    {
        [TestMethod]
        public void CheckingRibbonTabItem2()
        {
            Assert.IsTrue(this.secondRibbonTab.Select(), this.secondRibbonTab.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingRibbonCheckBox()
        {
            Assert.IsTrue(this.secondRibbonTab.Select(), this.secondRibbonTab.LastErrorMessage);

            Assert.IsTrue(this.secondRibbonTab.RibbonCheckBox.Uncheck(), this.secondRibbonTab.RibbonCheckBox.LastErrorMessage);
            Assert.IsFalse(this.secondRibbonTab.RibbonCheckBox.IsChecked, "Чекбокс в check состоянии после uncheck.");

            Assert.IsTrue(this.secondRibbonTab.RibbonCheckBox.Check(), this.secondRibbonTab.RibbonCheckBox.LastErrorMessage);
            Assert.IsTrue(this.secondRibbonTab.RibbonCheckBox.IsChecked, "Чекбокс в uncheck состоянии после check.");
        }

        [TestMethod]
        public void CheckingRibbonToggleButton()
        {
            Assert.IsTrue(this.secondRibbonTab.Select(), this.secondRibbonTab.LastErrorMessage);

            Assert.IsTrue(this.secondRibbonTab.RibbonToggleButton.Uncheck(), this.secondRibbonTab.RibbonToggleButton.LastErrorMessage);
            Assert.IsFalse(this.secondRibbonTab.RibbonToggleButton.IsChecked, "Чекбокс в check состоянии после uncheck.");

            Assert.IsTrue(this.secondRibbonTab.RibbonToggleButton.Check(), this.secondRibbonTab.RibbonToggleButton.LastErrorMessage);
            Assert.IsTrue(this.secondRibbonTab.RibbonToggleButton.IsChecked, "Чекбокс в uncheck состоянии после check.");
        }
    }
}
