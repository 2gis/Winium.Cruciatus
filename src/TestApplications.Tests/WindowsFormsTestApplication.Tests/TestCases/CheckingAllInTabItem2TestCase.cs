
namespace WindowsFormsTestApplication.Tests.TestCases
{
    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WindowsFormsTestApplication.Tests.Map;

    [CodedUITest]
    public class CheckingAllInTabItem2TestCase : WindowsFormsTestApplicationTestCase
    {
        #region Бесполезно, пока не работает переключение вкладок
        //private SecondTab tab;

        //[TestInitialize]
        //public void MyInitialize()
        //{
        //    this.tab = Application.MainWindow.TabItem2;

        //    //Это пока не работает
        //    //Assert.IsTrue(this.tab.Select(), this.tab.LastErrorMessage);
        //}

        //[TestMethod]
        //public void CheckingTabItem2()
        //{
        //    Assert.IsTrue(Application.MainWindow.TabItem2.Select(), Application.MainWindow.TabItem2.LastErrorMessage);
        //}

        //[TestMethod]
        //public void CheckingChangeEnabledButton()
        //{
        //    Assert.IsTrue(this.tab.ChangeEnabledButton.Click(), this.tab.ChangeEnabledButton.LastErrorMessage);
        //}

        //[TestMethod]
        //public void CheckingTextBox2()
        //{
        //    var startText = this.tab.TextBox2.Text;
        //    Assert.IsNotNull(startText, this.tab.TextBox2.LastErrorMessage);

        //    Assert.IsTrue(this.tab.TextBox2.SetText("new test text"), this.tab.TextBox2.LastErrorMessage);

        //    var currentText = this.tab.TextBox2.Text;
        //    Assert.IsNotNull(currentText, this.tab.TextBox2.LastErrorMessage);

        //    Assert.AreNotEqual(startText, currentText, "Текст не изменился.");
        //}

        //[TestMethod]
        //public void CheckingCheckBox2()
        //{
        //    Assert.IsTrue(this.tab.CheckBox2.UnCheck(), this.tab.CheckBox2.LastErrorMessage);
        //    Assert.IsFalse(this.tab.CheckBox2.IsChecked, "Чекбокс в check состоянии после uncheck.");

        //    Assert.IsTrue(this.tab.CheckBox2.Check(), this.tab.CheckBox2.LastErrorMessage);
        //    Assert.IsTrue(this.tab.CheckBox2.IsChecked, "Чекбокс в uncheck состоянии после check.");
        //}

        //[TestMethod]
        //public void CheckingCheckListBox()
        //{
        //    Assert.IsTrue(this.tab.CheckListBox.ScrollTo<CheckBox>("December"), this.tab.CheckListBox.LastErrorMessage);
        //    var month = this.tab.CheckListBox.Item<CheckBox>("December");
        //    Assert.IsNotNull(month, this.tab.CheckListBox.LastErrorMessage);
        //    Assert.IsTrue(month.Check(), month.LastErrorMessage);
        //    Assert.IsTrue(month.IsChecked, "Чекбокс December в uncheck состоянии после check.");

        //    Assert.IsTrue(this.tab.CheckListBox.ScrollTo<CheckBox>(10), this.tab.CheckListBox.LastErrorMessage);
        //    month = this.tab.CheckListBox.Item<CheckBox>(10);
        //    Assert.IsNotNull(month, this.tab.CheckListBox.LastErrorMessage);
        //    Assert.IsTrue(month.Check(), month.LastErrorMessage);
        //    Assert.IsTrue(month.IsChecked, "Чекбокс 10ый в uncheck состоянии после check.");
        //}

        //[TestMethod]
        //public void CheckingChangeEnabledTextBox2()
        //{
        //    Assert.IsTrue(this.tab.TextBox2.IsEnabled, "TextBox2 в начале оказался не включен.");

        //    Assert.IsTrue(this.tab.ChangeEnabledButton.Click(), this.tab.ChangeEnabledButton.LastErrorMessage);

        //    Assert.IsFalse(this.tab.TextBox2.IsEnabled, "TextBox2 не стал включенным.");
        //}

        //[TestMethod]
        //public void CheckingChangeAfterCheckBox2()
        //{
        //    Assert.IsTrue(this.tab.CheckListBox.ScrollTo<CheckBox>("March"), this.tab.CheckListBox.LastErrorMessage);
        //    var monthMarch = this.tab.CheckListBox.Item<CheckBox>("March");
        //    Assert.IsNotNull(monthMarch, this.tab.CheckListBox.LastErrorMessage);
        //    Assert.IsFalse(monthMarch.IsChecked, "Чекбокс March в check состоянии.");

        //    Assert.IsTrue(this.tab.CheckListBox.ScrollTo<CheckBox>("December"), this.tab.CheckListBox.LastErrorMessage);
        //    var monthDecember = this.tab.CheckListBox.Item<CheckBox>("December");
        //    Assert.IsNotNull(monthDecember, this.tab.CheckListBox.LastErrorMessage);
        //    Assert.IsFalse(monthDecember.IsChecked, "Чекбокс December в check состоянии.");

        //    Assert.IsTrue(this.tab.CheckBox2.Check(), this.tab.CheckBox2.LastErrorMessage);
        //    Assert.IsTrue(this.tab.CheckBox2.IsChecked, "Чекбокс в uncheck состоянии после check.");

        //    Assert.Fail("Ручная остановка. Не работает скролл, когда начальное состояние внизу, а интересуемое вверху.");
        //    Assert.IsTrue(this.tab.CheckListBox.ScrollTo<CheckBox>("March"), this.tab.CheckListBox.LastErrorMessage);
        //    Assert.IsTrue(monthMarch.IsChecked, "Чекбокс March остался в uncheck состоянии.");

        //    Assert.IsTrue(this.tab.CheckListBox.ScrollTo<CheckBox>("December"), this.tab.CheckListBox.LastErrorMessage);
        //    Assert.IsTrue(monthDecember.IsChecked, "Чекбокс December остался в uncheck состоянии.");
        //}
        #endregion
    }
}