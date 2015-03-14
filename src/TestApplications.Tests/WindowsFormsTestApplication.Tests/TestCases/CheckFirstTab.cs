namespace WindowsFormsTestApplication.Tests.TestCases
{
    #region using

    using NUnit.Framework;

    using WindowsFormsTestApplication.Tests.Map;

    using Winium.Cruciatus.Core;

    #endregion

    [TestFixture]
    public class CheckFirstTab
    {
        #region Fields

        private WindowsFormsTestApplicationApp application;

        private FirstTab firstTab;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void CheckingChangeEnabledTextListBox()
        {
            this.firstTab.CheckBox1.Uncheck();
            Assert.IsFalse(this.firstTab.CheckBox1.IsToggleOn, "Чекбокс в check состоянии после uncheck.");
            Assert.IsFalse(this.firstTab.TextListBox.Properties.IsEnabled, "TextListBox включен после uncheсk-а.");

            this.firstTab.CheckBox1.Check();
            Assert.IsTrue(this.firstTab.CheckBox1.IsToggleOn, "Чекбокс в uncheck состоянии после check.");
            Assert.IsTrue(this.firstTab.TextListBox.Properties.IsEnabled, "TextListBox выключен после cheсk-а.");
        }

        [Test]
        public void CheckingCheckBox1()
        {
            this.firstTab.CheckBox1.Uncheck();
            Assert.IsFalse(this.firstTab.CheckBox1.IsToggleOn, "Чекбокс в check состоянии после uncheck.");

            this.firstTab.CheckBox1.Check();
            Assert.IsTrue(this.firstTab.CheckBox1.IsToggleOn, "Чекбокс в uncheck состоянии после check.");
        }

        [Test]
        public void CheckingSetTextButton()
        {
            this.firstTab.SetTextButton.Click();
        }

        [Test]
        public void CheckingSetTextToTextBox1()
        {
            this.firstTab.TextBox1.SetText(null);
            this.firstTab.SetTextButton.Click();

            var currentText = this.firstTab.TextBox1.Text();
            Assert.AreEqual("CARAMBA", currentText, "После клика текст не стал = CARAMBA.");
        }

        [Test]
        public void CheckingTabItem1()
        {
            this.application.Window.TabItem1.Select();
        }

        [Test]
        public void CheckingTextBox1()
        {
            const string StartText = "new test text";
            this.firstTab.TextBox1.SetText(null);
            this.firstTab.TextBox1.SetText(StartText);

            var currentText = this.firstTab.TextBox1.Text();
            Assert.AreEqual(StartText, currentText, "Текст после ввода не стал new test text.");
        }

        [Test]
        public void CheckingTextComboBox()
        {
            this.firstTab.TextComboBox.Expand();

            var element = this.firstTab.TextComboBox.FindElement(By.Name("Quarter"));
            element.Click();
        }

        [Test]
        public void CheckingTextListBox()
        {
            var month = this.firstTab.TextListBox.ScrollTo(By.Name("December"));
            month.Click();

            month = this.firstTab.TextListBox.ScrollTo(By.Name("October"));
            month.Click();
        }

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            TestClassHelper.Initialize(out this.application);

            this.firstTab = this.application.Window.TabItem1;
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            TestClassHelper.Cleanup(this.application);
        }

        #endregion
    }
}
