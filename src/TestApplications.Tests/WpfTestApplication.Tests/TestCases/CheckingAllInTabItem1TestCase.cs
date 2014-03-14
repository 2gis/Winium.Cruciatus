
namespace WpfTestApplication.Tests.TestCases
{
    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The test class 1.
    /// </summary>
    public partial class TestClass1
    {
        [TestMethod]
        public void CheckingTabItem1()
        {
            Assert.IsTrue(Application.MainWindow.TabItem1.Select(), Application.MainWindow.TabItem1.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingSetTextButton()
        {
            Assert.IsTrue(this.firstTab.Select(), this.firstTab.LastErrorMessage);

            Assert.IsTrue(this.firstTab.SetTextButton.Click(), this.firstTab.SetTextButton.LastErrorMessage);

            var currentText = this.firstTab.TextBox1.Text;
            Assert.IsNotNull(currentText, this.firstTab.TextBox1.LastErrorMessage);

            Assert.AreEqual(currentText, "CARAMBA", "Верный текст не установлен в текстовое поле.");
        }

        [TestMethod]
        public void CheckingTextBox1()
        {
            Assert.IsTrue(this.firstTab.Select(), this.firstTab.LastErrorMessage);

            var startText = this.firstTab.TextBox1.Text;
            Assert.IsNotNull(startText, this.firstTab.TextBox1.LastErrorMessage);

            Assert.IsTrue(this.firstTab.TextBox1.SetText("new test text"), this.firstTab.TextBox1.LastErrorMessage);

            var currentText = this.firstTab.TextBox1.Text;
            Assert.IsNotNull(currentText, this.firstTab.TextBox1.LastErrorMessage);

            Assert.AreNotEqual(startText, currentText, "Текст не изменился.");
        }

        [TestMethod]
        public void CheckingTextComboBox()
        {
            Assert.IsTrue(this.firstTab.Select(), this.firstTab.LastErrorMessage);

            Assert.IsTrue(this.firstTab.TextComboBox.Expand(), this.firstTab.TextComboBox.LastErrorMessage);

            var element = this.firstTab.TextComboBox.Item<TextBlock>("Quarter");
            Assert.IsNotNull(element, this.firstTab.TextComboBox.LastErrorMessage);

            Assert.IsTrue(element.Click(), element.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingCheckBox1()
        {
            Assert.IsTrue(this.firstTab.Select(), this.firstTab.LastErrorMessage);

            Assert.IsTrue(this.firstTab.CheckBox1.Uncheck(), this.firstTab.CheckBox1.LastErrorMessage);
            Assert.IsFalse(this.firstTab.CheckBox1.IsChecked, "Чекбокс в check состоянии после uncheck.");

            Assert.IsTrue(this.firstTab.CheckBox1.Check(), this.firstTab.CheckBox1.LastErrorMessage);
            Assert.IsTrue(this.firstTab.CheckBox1.IsChecked, "Чекбокс в uncheck состоянии после check.");
        }

        [TestMethod]
        public void CheckingTextListBox()
        {
            Assert.IsTrue(this.firstTab.Select(), this.firstTab.LastErrorMessage);

            if (!this.firstTab.TextListBox.IsEnabled)
            {
                Assert.IsTrue(this.firstTab.CheckBox1.Check(), this.firstTab.CheckBox1.LastErrorMessage);
            }

            var month = this.firstTab.TextListBox.ScrollTo<TextBlock>("December");
            Assert.IsNotNull(month, this.firstTab.TextListBox.LastErrorMessage);
            Assert.IsTrue(month.Click(), month.LastErrorMessage);

            month = this.firstTab.TextListBox.ScrollTo<TextBlock>("October");
            Assert.IsNotNull(month, this.firstTab.TextListBox.LastErrorMessage);
            Assert.IsTrue(month.Click(), month.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingChangeEnabledTextListBox()
        {
            Assert.IsTrue(this.firstTab.Select(), this.firstTab.LastErrorMessage);

            Assert.IsTrue(this.firstTab.TextListBox.IsEnabled, "TextListBox в начале оказался не включен.");

            Assert.IsTrue(this.firstTab.CheckBox1.Uncheck(), this.firstTab.CheckBox1.LastErrorMessage);
            Assert.IsFalse(this.firstTab.CheckBox1.IsChecked, "Чекбокс в check состоянии после uncheck.");

            Assert.IsFalse(this.firstTab.TextListBox.IsEnabled, "TextListBox не стал включенным.");
        }
    }
}
