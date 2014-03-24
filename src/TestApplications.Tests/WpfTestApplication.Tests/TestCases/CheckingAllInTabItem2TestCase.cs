
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
        public void CheckingTabItem2()
        {
            Assert.IsTrue(Application.MainWindow.TabItem2.Select(), Application.MainWindow.TabItem2.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingChangeEnabledButton()
        {
            Assert.IsTrue(this.secondTab.Select(), this.secondTab.LastErrorMessage);

            Assert.IsTrue(this.secondTab.ChangeEnabledButton.Click(), this.secondTab.ChangeEnabledButton.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingTextBox2()
        {
            Assert.IsTrue(this.secondTab.Select(), this.secondTab.LastErrorMessage);

            if (!this.secondTab.TextBox2.IsEnabled)
            {
                Assert.IsTrue(this.secondTab.ChangeEnabledButton.Click(), this.secondTab.ChangeEnabledButton.LastErrorMessage);
            }

            var startText = this.secondTab.TextBox2.Text;
            Assert.IsNotNull(startText, this.secondTab.TextBox2.LastErrorMessage);

            Assert.IsTrue(this.secondTab.TextBox2.SetText("new test text"), this.secondTab.TextBox2.LastErrorMessage);

            var currentText = this.secondTab.TextBox2.Text;
            Assert.IsNotNull(currentText, this.secondTab.TextBox2.LastErrorMessage);

            Assert.AreNotEqual(startText, currentText, "Текст не изменился.");
        }

        [TestMethod]
        public void CheckingCheckComboBox()
        {
            Assert.IsTrue(this.secondTab.Select(), this.secondTab.LastErrorMessage);

            Assert.IsTrue(this.secondTab.CheckComboBox.Expand(), this.secondTab.CheckComboBox.LastErrorMessage);

            var element = this.secondTab.CheckComboBox.Item<CheckBox>("Quarter");
            Assert.IsNotNull(element, this.secondTab.CheckComboBox.LastErrorMessage);

            Assert.IsTrue(element.Check(), element.LastErrorMessage);
            Assert.IsTrue(element.IsChecked, "Чекбокс Quarter в uncheck состоянии после check.");

            element = this.secondTab.CheckComboBox.Item<CheckBox>("Week");
            Assert.IsNotNull(element, this.secondTab.CheckComboBox.LastErrorMessage);

            Assert.IsTrue(element.Check(), element.LastErrorMessage);
            Assert.IsTrue(element.IsChecked, "Чекбокс Week в uncheck состоянии после check.");

            Assert.IsTrue(this.secondTab.CheckComboBox.Collapse(), this.secondTab.CheckComboBox.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingCheckBox2()
        {
            Assert.IsTrue(this.secondTab.Select(), this.secondTab.LastErrorMessage);

            Assert.IsTrue(this.secondTab.CheckBox2.Uncheck(), this.secondTab.CheckBox2.LastErrorMessage);
            Assert.IsFalse(this.secondTab.CheckBox2.IsChecked, "Чекбокс в check состоянии после uncheck.");

            Assert.IsTrue(this.secondTab.CheckBox2.Check(), this.secondTab.CheckBox2.LastErrorMessage);
            Assert.IsTrue(this.secondTab.CheckBox2.IsChecked, "Чекбокс в uncheck состоянии после check.");
        }

        [TestMethod]
        public void CheckingCheckListBox()
        {
            Assert.IsTrue(this.secondTab.Select(), this.secondTab.LastErrorMessage);

            var month = this.secondTab.CheckListBox.ScrollTo<CheckBox>("December");
            Assert.IsNotNull(month, this.secondTab.CheckListBox.LastErrorMessage);
            Assert.IsTrue(month.Check(), month.LastErrorMessage);
            Assert.IsTrue(month.IsChecked, "Чекбокс December в uncheck состоянии после check.");

            month = this.secondTab.CheckListBox.ScrollTo<CheckBox>("October");
            Assert.IsNotNull(month, this.secondTab.CheckListBox.LastErrorMessage);
            Assert.IsTrue(month.Check(), month.LastErrorMessage);
            Assert.IsTrue(month.IsChecked, "Чекбокс 10ый в uncheck состоянии после check.");
        }

        [TestMethod]
        public void CheckingChangeEnabledTextBox2()
        {
            Assert.IsTrue(this.secondTab.Select(), this.secondTab.LastErrorMessage);

            var isEnabled = this.secondTab.TextBox2.IsEnabled;

            Assert.IsTrue(this.secondTab.ChangeEnabledButton.Click(), this.secondTab.ChangeEnabledButton.LastErrorMessage);

            Assert.AreNotEqual(isEnabled, this.secondTab.TextBox2.IsEnabled, "Включенность TextBox2 не изменилась.");
        }

        [TestMethod]
        public void CheckingChangeAfterCheckBox2()
        {
            Assert.IsTrue(this.secondTab.Select(), this.secondTab.LastErrorMessage);

            Assert.IsTrue(this.secondTab.CheckBox2.Check(), this.secondTab.CheckBox2.LastErrorMessage);
            Assert.IsTrue(this.secondTab.CheckBox2.Uncheck(), this.secondTab.CheckBox2.LastErrorMessage);
            Assert.IsFalse(this.secondTab.CheckBox2.IsChecked, "Чекбокс в check состоянии после uncheck.");

            var monthMarch = this.secondTab.CheckListBox.ScrollTo<CheckBox>("March");
            Assert.IsNotNull(monthMarch, this.secondTab.CheckListBox.LastErrorMessage);
            var marchIsChecked = monthMarch.IsChecked;

            var monthDecember = this.secondTab.CheckListBox.ScrollTo<CheckBox>("December");
            Assert.IsNotNull(monthDecember, this.secondTab.CheckListBox.LastErrorMessage);
            var decemberIsChecked = monthDecember.IsChecked;

            Assert.IsTrue(this.secondTab.CheckBox2.Check(), this.secondTab.CheckBox2.LastErrorMessage);
            Assert.IsTrue(this.secondTab.CheckBox2.IsChecked, "Чекбокс в uncheck состоянии после check.");

            Assert.IsNotNull(this.secondTab.CheckListBox.ScrollTo<CheckBox>("March"), this.secondTab.CheckListBox.LastErrorMessage);
            Assert.IsTrue(marchIsChecked != monthMarch.IsChecked, "У чекбокса March не изменилась чекнутость.");

            Assert.IsNotNull(this.secondTab.CheckListBox.ScrollTo<CheckBox>("December"), this.secondTab.CheckListBox.LastErrorMessage);
            Assert.IsTrue(decemberIsChecked != monthDecember.IsChecked, "У чекбокса December не изменилась чекнутость.");
        }
    }
}
