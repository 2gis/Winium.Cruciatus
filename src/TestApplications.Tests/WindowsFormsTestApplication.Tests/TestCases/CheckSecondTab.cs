namespace WindowsFormsTestApplication.Tests.TestCases
{
    #region using

    using WindowsFormsTestApplication.Tests.Map;

    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class CheckSecondTab
    {
        #region Бесполезно, пока не работает переключение вкладок

        private WindowsFormsTestApplicationApp _application;

        private SecondTab _secondTab;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            TestClassHelper.Initialize(out _application);

            _secondTab = _application.Window.TabItem2;
            _secondTab.Select();
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
            Assert.AreEqual(true, _secondTab.IsSelection, "Вкладка 2 оказалась не выбрана");
        }

        [Test]
        public void CheckingChangeEnabledButton()
        {
            if (!_secondTab.TextBox2.Properties.IsEnabled)
            {
                _secondTab.ChangeEnabledButton.Click();
            }

            _secondTab.ChangeEnabledButton.Click();
            Assert.AreEqual(false, _secondTab.TextBox2.Properties.IsEnabled);

            _secondTab.ChangeEnabledButton.Click();
            Assert.AreEqual(true, _secondTab.TextBox2.Properties.IsEnabled);
        }

        [Test]
        public void CheckingTextBox2()
        {
            _secondTab.TextBox2.SetText("start test text");
            var startText = _secondTab.TextBox2.Text();

            _secondTab.TextBox2.SetText("new test text");
            var currentText = _secondTab.TextBox2.Text();

            Assert.AreNotEqual(startText, currentText, "Текст не изменился.");
        }

        [Test]
        public void CheckingCheckBox2()
        {
            _secondTab.CheckBox2.Uncheck();
            Assert.AreEqual(false, _secondTab.CheckBox2.IsToggleOn, "Чекбокс в check состоянии после uncheck.");

            _secondTab.CheckBox2.Check();
            Assert.AreEqual(true, _secondTab.CheckBox2.IsToggleOn, "Чекбокс в uncheck состоянии после check.");
        }

        [Test]
        public void CheckingCheckListBox()
        {
            var month = _secondTab.CheckListBox.ScrollToCheckBoxByName("December");
            
            month.Check();
            Assert.AreEqual(true, month.IsToggleOn, "Чекбокс December в uncheck состоянии после check.");
        }

        [Test]
        public void CheckingChangeAfterCheckBox2()
        {
            var monthMarch = _secondTab.CheckListBox.ScrollToCheckBoxByName("March");
            Assert.AreEqual(false, monthMarch.IsToggleOn, "Чекбокс March в check состоянии.");

            var monthDecember = _secondTab.CheckListBox.ScrollToCheckBoxByName("December");
            Assert.AreEqual(false, monthDecember.IsToggleOn, "Чекбокс December в check состоянии.");

            _secondTab.CheckBox2.Check();
            Assert.AreEqual(true, _secondTab.CheckBox2.IsToggleOn, "Чекбокс в uncheck состоянии после check.");

            _secondTab.CheckListBox.ScrollToCheckBoxByName("March");
            Assert.AreEqual(true, monthMarch.IsToggleOn, "Чекбокс March остался в uncheck состоянии.");

            _secondTab.CheckListBox.ScrollToCheckBoxByName("December");
            Assert.AreEqual(true, monthDecember.IsToggleOn, "Чекбокс December остался в uncheck состоянии.");
        }

        #endregion
    }
}
