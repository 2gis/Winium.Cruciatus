namespace WindowsFormsTestApplication.Tests.TestCases
{
    #region using

    using WindowsFormsTestApplication.Tests.Map;

    using Cruciatus.Core;

    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class CheckFirstTab
    {
        private WindowsFormsTestApplicationApp _application;

        private FirstTab _firstTab;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            TestClassHelper.Initialize(out _application);

            _firstTab = _application.Window.TabItem1;
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            TestClassHelper.Cleanup(_application);
        }

        [Test]
        public void CheckingTabItem1()
        {
            _application.Window.TabItem1.Select();
        }

        [Test]
        public void CheckingSetTextButton()
        {
            _firstTab.SetTextButton.Click();
        }

        [Test]
        public void CheckingTextBox1()
        {
            const string startText = "new test text";
            _firstTab.TextBox1.SetText(null);
            _firstTab.TextBox1.SetText(startText);

            var currentText = _firstTab.TextBox1.Text();
            Assert.AreEqual(startText, currentText, "Текст после ввода не стал new test text.");
        }

        [Test]
        public void CheckingTextComboBox()
        {
            _firstTab.TextComboBox.Expand();

            var element = _firstTab.TextComboBox.Get(By.Name("Quarter"));
            element.Click();
        }

        [Test]
        public void CheckingTextListBox()
        {
            var month = _firstTab.TextListBox.ScrollTo(By.Name("December"));
            month.Click();

            month = _firstTab.TextListBox.ScrollTo(By.Name("October"));
            month.Click();
        }

        [Test]
        public void CheckingSetTextToTextBox1()
        {
            _firstTab.TextBox1.SetText(null);
            _firstTab.SetTextButton.Click();

            var currentText = _firstTab.TextBox1.Text();
            Assert.AreEqual("CARAMBA", currentText, "После клика текст не стал = CARAMBA.");
        }

        [Test]
        public void CheckingCheckBox1()
        {
            _firstTab.CheckBox1.Uncheck();
            Assert.IsFalse(_firstTab.CheckBox1.IsToggleOn, "Чекбокс в check состоянии после uncheck.");

            _firstTab.CheckBox1.Check();
            Assert.IsTrue(_firstTab.CheckBox1.IsToggleOn, "Чекбокс в uncheck состоянии после check.");
        }

        [Test]
        public void CheckingChangeEnabledTextListBox()
        {
            _firstTab.CheckBox1.Uncheck();
            Assert.IsFalse(_firstTab.CheckBox1.IsToggleOn, "Чекбокс в check состоянии после uncheck.");
            Assert.IsFalse(_firstTab.TextListBox.Properties.IsEnabled, "TextListBox включен после uncheсk-а.");

            _firstTab.CheckBox1.Check();
            Assert.IsTrue(_firstTab.CheckBox1.IsToggleOn, "Чекбокс в uncheck состоянии после check.");
            Assert.IsTrue(_firstTab.TextListBox.Properties.IsEnabled, "TextListBox выключен после cheсk-а.");
        }
    }
}
