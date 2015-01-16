namespace WpfTestApplication.Tests.TestCases
{
    #region using

    using NUnit.Framework;

    using WpfTestApplication.Tests.Map;

    #endregion

    [TestFixture]
    public class CheckSecondTab
    {
        private WpfTestApplicationApp _application;

        private SecondTab _secondTab;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            TestClassHelper.Initialize(out _application);

            _secondTab = _application.MainWindow.TabItem2;

            _secondTab.Select();
            Assert.IsTrue(_secondTab.IsSelection);
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            TestClassHelper.Cleanup(_application);
        }

        [Test]
        public void CheckingTabItem2()
        {
            _secondTab.Select();
            Assert.IsTrue(_secondTab.IsSelection);
        }

        [Test]
        public void CheckingChangeEnabledButton()
        {
            _secondTab.ChangeEnabledButton.Click();
        }

        [Test]
        public void CheckingTextBox2()
        {
            if (!_secondTab.TextBox2.Properties.IsEnabled)
            {
                _secondTab.ChangeEnabledButton.Click();
            }

            var startText = _secondTab.TextBox2.Text();

            _secondTab.TextBox2.SetText("new test text");

            var currentText = _secondTab.TextBox2.Text();

            Assert.AreNotEqual(startText, currentText, "Текст не изменился.");
        }

        [Test]
        public void CheckingCheckComboBox()
        {
            _secondTab.CheckComboBox.Expand();
            Assert.AreEqual(true, _secondTab.CheckComboBox.IsExpanded);

            var element = _secondTab.CheckComboBox.GetCheckBoxByName("Quarter");

            element.Check();
            Assert.AreEqual(true, element.IsToggleOn, "Чекбокс Quarter в uncheck состоянии после check.");

            element = _secondTab.CheckComboBox.GetCheckBoxByName("Week");

            element.Check();
            Assert.AreEqual(true, element.IsToggleOn, "Чекбокс Week в uncheck состоянии после check.");

            _secondTab.CheckComboBox.Collapse();
            Assert.AreEqual(false, _secondTab.CheckComboBox.IsExpanded);
        }

        [Test]
        public void CheckingCheckBox2()
        {
            _secondTab.CheckBox2.Uncheck();
            Assert.IsFalse(_secondTab.CheckBox2.IsToggleOn, "Чекбокс в check состоянии после uncheck.");

            _secondTab.CheckBox2.Check();
            Assert.IsTrue(_secondTab.CheckBox2.IsToggleOn, "Чекбокс в uncheck состоянии после check.");
        }

        [Test]
        public void CheckingCheckListBox()
        {
            var month = _secondTab.CheckListBox.ScrollToCheckBoxByName("December");
            month.Check();
            Assert.AreEqual(true, month.IsToggleOn, "Чекбокс December в uncheck состоянии после check.");

            month = _secondTab.CheckListBox.ScrollToCheckBoxByName("October");
            month.Check();
            Assert.AreEqual(true, month.IsToggleOn, "Чекбокс 10ый в uncheck состоянии после check.");
        }

        [Test]
        public void CheckingChangeEnabledTextBox2()
        {
            var isEnabled = _secondTab.TextBox2.Properties.IsEnabled;

            _secondTab.ChangeEnabledButton.Click();

            Assert.AreNotEqual(isEnabled, _secondTab.TextBox2.Properties.IsEnabled, "Включенность TextBox2 не изменилась.");
        }

        [Test]
        public void CheckingChangeAfterCheckBox2()
        {
            _secondTab.CheckBox2.Check();
            _secondTab.CheckBox2.Uncheck();
            Assert.AreEqual(false, _secondTab.CheckBox2.IsToggleOn, "Чекбокс в check состоянии после uncheck.");

            var monthMarch = _secondTab.CheckListBox.GetCheckBoxByName("March");
            var monthDecember = _secondTab.CheckListBox.ScrollToCheckBoxByName("December");

            _secondTab.CheckBox2.Check();
            Assert.AreEqual(true, _secondTab.CheckBox2.IsToggleOn, "Чекбокс в uncheck состоянии после check.");
            Assert.AreEqual(true, monthMarch.IsToggleOn, "У чекбокса March не изменилась чекнутость.");
            Assert.AreEqual(true, monthDecember.IsToggleOn, "У чекбокса December не изменилась чекнутость.");
        }
    }
}
