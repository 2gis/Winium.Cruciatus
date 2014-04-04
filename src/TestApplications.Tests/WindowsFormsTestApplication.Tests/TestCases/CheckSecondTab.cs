namespace WindowsFormsTestApplication.Tests.TestCases
{
    #region using

    using WindowsFormsTestApplication.Tests.Map;

    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    #endregion

    [CodedUITest]
    public class CheckSecondTab
    {
        #region Бесполезно, пока не работает переключение вкладок

        ////private static bool _firstClassStartFlag = true;

        ////private static WindowsFormsTestApplicationApp _application;

        ////private static SecondTab _secondTab;

        ////[ClassInitialize]
        ////public static void ClassInitialize(TestContext testContext)
        ////{
        ////    TestClassHelper.ClassInitialize(out _application);
        ////}

        ////[ClassCleanup]
        ////public static void ClassCleanup()
        ////{
        ////    TestClassHelper.ClassCleanup(_application);
        ////}

        ////[TestInitialize]
        ////public void TestInitialize()
        ////{
        ////    _secondTab = _application.MainWindow.TabItem2;

        ////    if (_firstClassStartFlag)
        ////    {
        ////        // Это пока не работает
        ////        ////Assert.IsTrue(_secondTab.Select(), _secondTab.LastErrorMessage);
        ////        _firstClassStartFlag = false;
        ////    }
        ////}

        ////[TestMethod]
        ////public void CheckingTabItem2()
        ////{
        ////    Assert.IsTrue(_application.MainWindow.TabItem2.Select(), _application.MainWindow.TabItem2.LastErrorMessage);
        ////}

        ////[TestMethod]
        ////public void CheckingChangeEnabledButton()
        ////{
        ////    Assert.IsTrue(_secondTab.ChangeEnabledButton.Click(), _secondTab.ChangeEnabledButton.LastErrorMessage);
        ////}

        ////[TestMethod]
        ////public void CheckingTextBox2()
        ////{
        ////    var startText = _secondTab.TextBox2.Text;
        ////    Assert.IsNotNull(startText, _secondTab.TextBox2.LastErrorMessage);

        ////    Assert.IsTrue(_secondTab.TextBox2.SetText("new test text"), _secondTab.TextBox2.LastErrorMessage);

        ////    var currentText = _secondTab.TextBox2.Text;
        ////    Assert.IsNotNull(currentText, _secondTab.TextBox2.LastErrorMessage);

        ////    Assert.AreNotEqual(startText, currentText, "Текст не изменился.");
        ////}

        ////[TestMethod]
        ////public void CheckingCheckBox2()
        ////{
        ////    Assert.IsTrue(_secondTab.CheckBox2.Uncheck(), _secondTab.CheckBox2.LastErrorMessage);
        ////    Assert.IsFalse(_secondTab.CheckBox2.IsChecked, "Чекбокс в check состоянии после uncheck.");

        ////    Assert.IsTrue(_secondTab.CheckBox2.Check(), _secondTab.CheckBox2.LastErrorMessage);
        ////    Assert.IsTrue(_secondTab.CheckBox2.IsChecked, "Чекбокс в uncheck состоянии после check.");
        ////}

        ////[TestMethod]
        ////public void CheckingCheckListBox()
        ////{
        ////    var month = _secondTab.CheckListBox.ScrollTo<CheckBox>("December");
        ////    Assert.IsNotNull(month, _secondTab.CheckListBox.LastErrorMessage);
        ////    Assert.IsTrue(month.Check(), month.LastErrorMessage);
        ////    Assert.IsTrue(month.IsChecked, "Чекбокс December в uncheck состоянии после check.");
        ////}

        ////[TestMethod]
        ////public void CheckingChangeEnabledTextBox2()
        ////{
        ////    Assert.IsTrue(_secondTab.TextBox2.IsEnabled, "TextBox2 в начале оказался не включен.");

        ////    Assert.IsTrue(_secondTab.ChangeEnabledButton.Click(), _secondTab.ChangeEnabledButton.LastErrorMessage);

        ////    Assert.IsFalse(_secondTab.TextBox2.IsEnabled, "TextBox2 не стал включенным.");
        ////}

        ////[TestMethod]
        ////public void CheckingChangeAfterCheckBox2()
        ////{
        ////    var monthMarch = _secondTab.CheckListBox.ScrollTo<CheckBox>("March");
        ////    Assert.IsNotNull(monthMarch, _secondTab.CheckListBox.LastErrorMessage);
        ////    Assert.IsFalse(monthMarch.IsChecked, "Чекбокс March в check состоянии.");

        ////    var monthDecember = _secondTab.CheckListBox.ScrollTo<CheckBox>("December");
        ////    Assert.IsNotNull(monthDecember, _secondTab.CheckListBox.LastErrorMessage);
        ////    Assert.IsFalse(monthDecember.IsChecked, "Чекбокс December в check состоянии.");

        ////    Assert.IsTrue(_secondTab.CheckBox2.Check(), _secondTab.CheckBox2.LastErrorMessage);
        ////    Assert.IsTrue(_secondTab.CheckBox2.IsChecked, "Чекбокс в uncheck состоянии после check.");

        ////    Assert.Fail("Ручная остановка. Не работает скролл, когда начальное состояние внизу, а интересуемое вверху.");
        ////    Assert.IsNotNull(_secondTab.CheckListBox.ScrollTo<CheckBox>("March"), 
        ////                     _secondTab.CheckListBox.LastErrorMessage);
        ////    Assert.IsTrue(monthMarch.IsChecked, "Чекбокс March остался в uncheck состоянии.");

        ////    Assert.IsNotNull(_secondTab.CheckListBox.ScrollTo<CheckBox>("December"), 
        ////                     _secondTab.CheckListBox.LastErrorMessage);
        ////    Assert.IsTrue(monthDecember.IsChecked, "Чекбокс December остался в uncheck состоянии.");
        ////}

        #endregion
    }
}
