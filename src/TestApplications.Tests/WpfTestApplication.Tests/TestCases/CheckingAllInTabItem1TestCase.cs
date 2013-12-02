
namespace WpfTestApplication.Tests.TestCases
{
    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WpfTestApplication.Tests.Map;

    [CodedUITest]
    public class CheckingAllInTabItem1TestCase : WpfTestApplicationTestCase
    {
        private FirstTab tab;

        [TestInitialize]
        public void MyInitialize()
        {
            this.tab = Application.MainWindow.TabItem1;
            Assert.IsTrue(this.tab.Select(), this.tab.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingTabItem1()
        {
            Assert.IsTrue(Application.MainWindow.TabItem1.Select(), Application.MainWindow.TabItem1.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingSetTextButton()
        {
            Assert.IsTrue(this.tab.SetTextButton.Click(), this.tab.SetTextButton.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingTextBox1()
        {
            var startText = this.tab.TextBox1.Text;
            Assert.IsNotNull(startText, this.tab.TextBox1.LastErrorMessage);

            Assert.IsTrue(this.tab.TextBox1.SetText("new test text"), this.tab.TextBox1.LastErrorMessage);

            var currentText = this.tab.TextBox1.Text;
            Assert.IsNotNull(currentText, this.tab.TextBox1.LastErrorMessage);

            Assert.AreNotEqual(startText, currentText, "Текст не изменился.");
        }

        [TestMethod]
        public void CheckingTextComboBox()
        {
            Assert.IsTrue(this.tab.TextComboBox.Expand(), this.tab.TextComboBox.LastErrorMessage);

            var element = this.tab.TextComboBox.Item<TextBlock>("Quarter");
            Assert.IsNotNull(element, this.tab.TextComboBox.LastErrorMessage);

            Assert.IsTrue(element.Click(), element.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingCheckBox1()
        {
            Assert.IsTrue(this.tab.CheckBox1.UnCheck(), this.tab.CheckBox1.LastErrorMessage);
            Assert.IsNotNull(this.tab.CheckBox1.IsChecked, "Чекбокс в неопределенном состоянии после uncheck.");
            Assert.IsFalse(this.tab.CheckBox1.IsChecked.Value, "Чекбокс в check состоянии после uncheck.");

            Assert.IsTrue(this.tab.CheckBox1.Check(), this.tab.CheckBox1.LastErrorMessage);
            Assert.IsNotNull(this.tab.CheckBox1.IsChecked, "Чекбокс в неопределенном состоянии после check.");
            Assert.IsTrue(this.tab.CheckBox1.IsChecked.Value, "Чекбокс в uncheck состоянии после check.");
        }

        [TestMethod]
        public void TextListBox()
        {
            Assert.IsTrue(this.tab.TextListBox.ScrollTo<TextBlock>("December"), this.tab.TextListBox.LastErrorMessage);
            var month = this.tab.TextListBox.Item<TextBlock>("December");
            Assert.IsNotNull(month, this.tab.TextListBox.LastErrorMessage);
            Assert.IsTrue(month.Click(), month.LastErrorMessage);

            Assert.IsTrue(this.tab.TextListBox.ScrollTo<TextBlock>(10), this.tab.TextListBox.LastErrorMessage);
            month = this.tab.TextListBox.Item<TextBlock>(10);
            Assert.IsNotNull(month, this.tab.TextListBox.LastErrorMessage);
            Assert.IsTrue(month.Click(), month.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingSetTextToTextBox1()
        {
            var startText = this.tab.TextBox1.Text;
            Assert.IsNotNull(startText, this.tab.TextBox1.LastErrorMessage);

            Assert.IsTrue(this.tab.SetTextButton.Click(), this.tab.SetTextButton.LastErrorMessage);

            var currentText = this.tab.TextBox1.Text;
            Assert.IsNotNull(currentText, this.tab.TextBox1.LastErrorMessage);

            Assert.AreNotEqual(startText, currentText, "Текст не изменился.");
        }

        [TestMethod]
        public void CheckingChangeEnabledTextListBox()
        {
            Assert.Fail("Ручная остановка. В ListBox нет проверки на включенность.");
            //Assert.IsTrue(this.tab.TextListBox.IsEnabled, "TextListBox в начале оказался не включен.");

            Assert.IsTrue(this.tab.CheckBox1.UnCheck(), this.tab.CheckBox1.LastErrorMessage);
            Assert.IsNotNull(this.tab.CheckBox1.IsChecked, "Чекбокс в неопределенном состоянии после uncheck.");
            Assert.IsFalse(this.tab.CheckBox1.IsChecked.Value, "Чекбокс в check состоянии после uncheck.");

            //Assert.IsFalse(this.tab.TextListBox.IsEnabled, "TextListBox не стал включенным.");
        }
    }
}
