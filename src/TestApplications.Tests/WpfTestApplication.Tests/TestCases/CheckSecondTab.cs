namespace WpfTestApplication.Tests.TestCases
{
    #region using

    using NUnit.Framework;

    using WpfTestApplication.Tests.Map;

    #endregion

    [TestFixture]
    public class CheckSecondTab
    {
        #region Fields

        private WpfTestApplicationApp application;

        private SecondTab secondTab;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void CheckingChangeAfterCheckBox2()
        {
            this.secondTab.CheckBox2.Check();
            this.secondTab.CheckBox2.Uncheck();
            Assert.AreEqual(false, this.secondTab.CheckBox2.IsToggleOn, "Чекбокс в check состоянии после uncheck.");

            var monthMarch = this.secondTab.CheckListBox.GetCheckBoxByName("March");
            var monthDecember = this.secondTab.CheckListBox.ScrollToCheckBoxByName("December");

            this.secondTab.CheckBox2.Check();
            Assert.AreEqual(true, this.secondTab.CheckBox2.IsToggleOn, "Чекбокс в uncheck состоянии после check.");
            Assert.AreEqual(true, monthMarch.IsToggleOn, "У чекбокса March не изменилась чекнутость.");
            Assert.AreEqual(true, monthDecember.IsToggleOn, "У чекбокса December не изменилась чекнутость.");
        }

        [Test]
        public void CheckingChangeEnabledButton()
        {
            this.secondTab.ChangeEnabledButton.Click();
        }

        [Test]
        public void CheckingChangeEnabledTextBox2()
        {
            var isEnabled = this.secondTab.TextBox2.Properties.IsEnabled;

            this.secondTab.ChangeEnabledButton.Click();

            Assert.AreNotEqual(
                isEnabled, 
                this.secondTab.TextBox2.Properties.IsEnabled, 
                "Включенность TextBox2 не изменилась.");
        }

        [Test]
        public void CheckingCheckBox2()
        {
            this.secondTab.CheckBox2.Uncheck();
            Assert.That(
                this.secondTab.CheckBox2.IsToggleOn, 
                Is.False.After(100, 2000), 
                "Чекбокс в check состоянии после uncheck.");

            this.secondTab.CheckBox2.Check();
            Assert.That(
                this.secondTab.CheckBox2.IsToggleOn, 
                Is.True.After(100, 2000), 
                "Чекбокс в uncheck состоянии после check.");
        }

        [Test]
        public void CheckingCheckComboBox()
        {
            this.secondTab.CheckComboBox.Expand();
            Assert.AreEqual(true, this.secondTab.CheckComboBox.IsExpanded);

            var element = this.secondTab.CheckComboBox.GetCheckBoxByName("Quarter");

            element.Check();
            Assert.That(
                element.IsToggleOn, 
                Is.True.After(100, 2000), 
                "Чекбокс Quarter в uncheck состоянии после check.");

            element = this.secondTab.CheckComboBox.GetCheckBoxByName("Week");

            element.Check();
            Assert.AreEqual(true, element.IsToggleOn, "Чекбокс Week в uncheck состоянии после check.");

            this.secondTab.CheckComboBox.Collapse();
            Assert.AreEqual(false, this.secondTab.CheckComboBox.IsExpanded);
        }

        [Test]
        public void CheckingCheckListBox()
        {
            var month = this.secondTab.CheckListBox.ScrollToCheckBoxByName("December");
            month.Check();
            Assert.AreEqual(true, month.IsToggleOn, "Чекбокс December в uncheck состоянии после check.");

            month = this.secondTab.CheckListBox.ScrollToCheckBoxByName("October");
            month.Check();
            Assert.AreEqual(true, month.IsToggleOn, "Чекбокс 10ый в uncheck состоянии после check.");
        }

        [Test]
        public void CheckingTabItem2()
        {
            this.secondTab.Select();
            Assert.IsTrue(this.secondTab.IsSelection);
        }

        [Test]
        public void CheckingTextBox2()
        {
            if (!this.secondTab.TextBox2.Properties.IsEnabled)
            {
                this.secondTab.ChangeEnabledButton.Click();
            }

            var startText = this.secondTab.TextBox2.Text();

            this.secondTab.TextBox2.SetText("new test text");

            var currentText = this.secondTab.TextBox2.Text();

            Assert.AreNotEqual(startText, currentText, "Текст не изменился.");
        }

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            TestClassHelper.Initialize(out this.application);

            this.secondTab = this.application.MainWindow.TabItem2;

            this.secondTab.Select();
            Assert.IsTrue(this.secondTab.IsSelection);
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            TestClassHelper.Cleanup(this.application);
        }

        #endregion
    }
}
