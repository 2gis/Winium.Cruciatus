
namespace WpfTestApplication.Tests.TestCases
{
    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The test class 2.
    /// </summary>
    public partial class TestClass2
    {
        [TestMethod]
        public void CheckingRibbonTabItem1()
        {
            Assert.IsTrue(this.firstRibbonTab.Select(), this.firstRibbonTab.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingRibbonButton()
        {
            Assert.IsTrue(this.firstRibbonTab.Select(), this.firstRibbonTab.LastErrorMessage);

            Assert.IsTrue(this.firstRibbonTab.RibbonButton.Click(), this.firstRibbonTab.RibbonButton.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingRibbonTextComboBox()
        {
            Assert.IsTrue(this.firstRibbonTab.Select(), this.firstRibbonTab.LastErrorMessage);

            Assert.IsTrue(this.firstRibbonTab.RibbonTextComboBox.Expand(), this.firstRibbonTab.RibbonTextComboBox.LastErrorMessage);

            var element = this.firstRibbonTab.RibbonTextComboBox.Item<TextBlock>("Quarter");
            Assert.IsNotNull(element, this.firstRibbonTab.RibbonTextComboBox.LastErrorMessage);

            Assert.IsTrue(element.Click(), element.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingRibbonCheckComboBox()
        {
            Assert.IsTrue(this.firstRibbonTab.Select(), this.firstRibbonTab.LastErrorMessage);

            Assert.IsTrue(this.firstRibbonTab.RibbonCheckComboBox.Expand(), this.firstRibbonTab.RibbonCheckComboBox.LastErrorMessage);

            var element = this.firstRibbonTab.RibbonCheckComboBox.Item<CheckBox>("Quarter");
            Assert.IsNotNull(element, this.firstRibbonTab.RibbonCheckComboBox.LastErrorMessage);

            Assert.IsTrue(element.Check(), element.LastErrorMessage);
            Assert.IsTrue(element.IsChecked, "Чекбокс Quarter в uncheck состоянии после check.");

            element = this.firstRibbonTab.RibbonCheckComboBox.Item<CheckBox>("Week");
            Assert.IsNotNull(element, this.firstRibbonTab.RibbonCheckComboBox.LastErrorMessage);

            Assert.IsTrue(element.Check(), element.LastErrorMessage);
            Assert.IsTrue(element.IsChecked, "Чекбокс Week в uncheck состоянии после check.");

            Assert.IsTrue(this.firstRibbonTab.RibbonCheckComboBox.Collapse(), this.firstRibbonTab.RibbonCheckComboBox.LastErrorMessage);
        }
    }
}
