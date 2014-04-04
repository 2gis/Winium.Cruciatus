namespace WpfTestApplication.Tests.TestCases
{
    #region using

    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WpfTestApplication.Tests.Map;

    #endregion

    [CodedUITest]
    public class CheckSecondTab
    {
        private static bool _firstClassStartFlag = true;

        private static WpfTestApplicationApp _application;

        private SecondTab _secondTab;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassHelper.ClassInitialize(out _application);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestClassHelper.ClassCleanup(_application);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _secondTab = _application.MainWindow.TabItem2;

            if (_firstClassStartFlag)
            {
                Assert.IsTrue(_secondTab.Select(), _secondTab.LastErrorMessage);
                _firstClassStartFlag = false;
            }
        }

        [TestMethod]
        public void CheckingTabItem2()
        {
            Assert.IsTrue(_secondTab.Select(), _secondTab.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingChangeEnabledButton()
        {
            Assert.IsTrue(_secondTab.ChangeEnabledButton.Click(), _secondTab.ChangeEnabledButton.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingTextBox2()
        {
            if (!_secondTab.TextBox2.IsEnabled)
            {
                Assert.IsTrue(_secondTab.ChangeEnabledButton.Click(), _secondTab.ChangeEnabledButton.LastErrorMessage);
            }

            var startText = _secondTab.TextBox2.Text;
            Assert.IsNotNull(startText, _secondTab.TextBox2.LastErrorMessage);

            Assert.IsTrue(_secondTab.TextBox2.SetText("new test text"), _secondTab.TextBox2.LastErrorMessage);

            var currentText = _secondTab.TextBox2.Text;
            Assert.IsNotNull(currentText, _secondTab.TextBox2.LastErrorMessage);

            Assert.AreNotEqual(startText, currentText, "Текст не изменился.");
        }

        [TestMethod]
        public void CheckingCheckComboBox()
        {
            Assert.IsTrue(_secondTab.CheckComboBox.Expand(), _secondTab.CheckComboBox.LastErrorMessage);

            var element = _secondTab.CheckComboBox.Item<CheckBox>("Quarter");
            Assert.IsNotNull(element, _secondTab.CheckComboBox.LastErrorMessage);

            Assert.IsTrue(element.Check(), element.LastErrorMessage);
            Assert.IsTrue(element.IsChecked, "Чекбокс Quarter в uncheck состоянии после check.");

            element = _secondTab.CheckComboBox.Item<CheckBox>("Week");
            Assert.IsNotNull(element, _secondTab.CheckComboBox.LastErrorMessage);

            Assert.IsTrue(element.Check(), element.LastErrorMessage);
            Assert.IsTrue(element.IsChecked, "Чекбокс Week в uncheck состоянии после check.");

            Assert.IsTrue(_secondTab.CheckComboBox.Collapse(), _secondTab.CheckComboBox.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingCheckBox2()
        {
            Assert.IsTrue(_secondTab.CheckBox2.Uncheck(), _secondTab.CheckBox2.LastErrorMessage);
            Assert.IsFalse(_secondTab.CheckBox2.IsChecked, "Чекбокс в check состоянии после uncheck.");

            Assert.IsTrue(_secondTab.CheckBox2.Check(), _secondTab.CheckBox2.LastErrorMessage);
            Assert.IsTrue(_secondTab.CheckBox2.IsChecked, "Чекбокс в uncheck состоянии после check.");
        }

        [TestMethod]
        public void CheckingCheckListBox()
        {
            var month = _secondTab.CheckListBox.ScrollTo<CheckBox>("December");
            Assert.IsNotNull(month, _secondTab.CheckListBox.LastErrorMessage);
            Assert.IsTrue(month.Check(), month.LastErrorMessage);
            Assert.IsTrue(month.IsChecked, "Чекбокс December в uncheck состоянии после check.");

            month = _secondTab.CheckListBox.ScrollTo<CheckBox>("October");
            Assert.IsNotNull(month, _secondTab.CheckListBox.LastErrorMessage);
            Assert.IsTrue(month.Check(), month.LastErrorMessage);
            Assert.IsTrue(month.IsChecked, "Чекбокс 10ый в uncheck состоянии после check.");
        }

        [TestMethod]
        public void CheckingChangeEnabledTextBox2()
        {
            var isEnabled = _secondTab.TextBox2.IsEnabled;

            Assert.IsTrue(_secondTab.ChangeEnabledButton.Click(), _secondTab.ChangeEnabledButton.LastErrorMessage);

            Assert.AreNotEqual(isEnabled, _secondTab.TextBox2.IsEnabled, "Включенность TextBox2 не изменилась.");
        }

        [TestMethod]
        public void CheckingChangeAfterCheckBox2()
        {
            Assert.IsTrue(_secondTab.CheckBox2.Check(), _secondTab.CheckBox2.LastErrorMessage);
            Assert.IsTrue(_secondTab.CheckBox2.Uncheck(), _secondTab.CheckBox2.LastErrorMessage);
            Assert.IsFalse(_secondTab.CheckBox2.IsChecked, "Чекбокс в check состоянии после uncheck.");

            var monthMarch = _secondTab.CheckListBox.ScrollTo<CheckBox>("March");
            Assert.IsNotNull(monthMarch, _secondTab.CheckListBox.LastErrorMessage);
            var marchIsChecked = monthMarch.IsChecked;

            var monthDecember = _secondTab.CheckListBox.ScrollTo<CheckBox>("December");
            Assert.IsNotNull(monthDecember, _secondTab.CheckListBox.LastErrorMessage);
            var decemberIsChecked = monthDecember.IsChecked;

            Assert.IsTrue(_secondTab.CheckBox2.Check(), _secondTab.CheckBox2.LastErrorMessage);
            Assert.IsTrue(_secondTab.CheckBox2.IsChecked, "Чекбокс в uncheck состоянии после check.");

            Assert.IsNotNull(_secondTab.CheckListBox.ScrollTo<CheckBox>("March"), 
                             _secondTab.CheckListBox.LastErrorMessage);
            Assert.IsTrue(marchIsChecked != monthMarch.IsChecked, "У чекбокса March не изменилась чекнутость.");

            Assert.IsNotNull(_secondTab.CheckListBox.ScrollTo<CheckBox>("December"), 
                             _secondTab.CheckListBox.LastErrorMessage);
            Assert.IsTrue(decemberIsChecked != monthDecember.IsChecked, "У чекбокса December не изменилась чекнутость.");
        }
    }
}
