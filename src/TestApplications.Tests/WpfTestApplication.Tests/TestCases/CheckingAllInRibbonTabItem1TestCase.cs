
namespace WpfTestApplication.Tests.TestCases
{
    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WpfTestApplication.Tests.Map;

    [CodedUITest]
    public class CheckingAllInRibbonTabItem1TestCase : WpfTestApplicationTestCase
    {
        private FirstRibbonTab tab;

        [TestInitialize]
        public void MyInitialize()
        {
            this.tab = Application.MainWindow.RibbonTabItem1;

            //Это пока не работает
            //Assert.IsTrue(this.tab.Select(), this.tab.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingRibbonTabItem1()
        {
            Assert.Fail("Ручная остановка. В мелкософтной вкладке риббона точка клика находится не на заголовке.");
            Assert.IsTrue(Application.MainWindow.TabItem1.Select(), Application.MainWindow.TabItem1.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingRibbonButton()
        {
            Assert.IsTrue(this.tab.RibbonButton.Click(), this.tab.RibbonButton.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingRibbonTextComboBox()
        {
            Assert.IsTrue(this.tab.RibbonTextComboBox.Expand(), this.tab.RibbonTextComboBox.LastErrorMessage);

            var element = this.tab.RibbonTextComboBox.Item<TextBlock>("Quarter");
            Assert.IsNotNull(element, this.tab.RibbonTextComboBox.LastErrorMessage);

            Assert.IsTrue(element.Click(), element.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingRibbonCheckComboBox()
        {
            Assert.IsTrue(this.tab.RibbonCheckComboBox.Expand(), this.tab.RibbonCheckComboBox.LastErrorMessage);

            var element = this.tab.RibbonCheckComboBox.Item<CheckBox>("Quarter");
            Assert.IsNotNull(element, this.tab.RibbonCheckComboBox.LastErrorMessage);

            Assert.IsTrue(element.Check(), element.LastErrorMessage);
            Assert.IsTrue(element.IsChecked, "Чекбокс Quarter в uncheck состоянии после check.");

            element = this.tab.RibbonCheckComboBox.Item<CheckBox>("Week");
            Assert.IsNotNull(element, this.tab.RibbonCheckComboBox.LastErrorMessage);

            Assert.IsTrue(element.Check(), element.LastErrorMessage);
            Assert.IsTrue(element.IsChecked, "Чекбокс Week в uncheck состоянии после check.");

            Assert.IsTrue(this.tab.RibbonCheckComboBox.Collapse(), this.tab.RibbonCheckComboBox.LastErrorMessage);
        }
    }
}