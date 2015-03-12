namespace WindowsFormsTestApplication.Tests.TestCases
{
    #region using

    using NUnit.Framework;

    using WindowsFormsTestApplication.Tests.Map;

    #endregion

    [TestFixture]
    public class CheckSecondTab
    {
        #region Fields

        private WindowsFormsTestApplicationApp application;

        private SecondTab secondTab;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void CheckingChangeAfterCheckBox2()
        {
            var monthMarch = this.secondTab.CheckListBox.ScrollToCheckBoxByName("March");
            Assert.AreEqual(false, monthMarch.IsToggleOn, "Чекбокс March в check состоянии.");

            var monthDecember = this.secondTab.CheckListBox.ScrollToCheckBoxByName("December");
            Assert.AreEqual(false, monthDecember.IsToggleOn, "Чекбокс December в check состоянии.");

            this.secondTab.CheckBox2.Check();
            Assert.AreEqual(true, this.secondTab.CheckBox2.IsToggleOn, "Чекбокс в uncheck состоянии после check.");

            this.secondTab.CheckListBox.ScrollToCheckBoxByName("March");
            Assert.AreEqual(true, monthMarch.IsToggleOn, "Чекбокс March остался в uncheck состоянии.");

            this.secondTab.CheckListBox.ScrollToCheckBoxByName("December");
            Assert.AreEqual(true, monthDecember.IsToggleOn, "Чекбокс December остался в uncheck состоянии.");
        }

        [Test]
        public void CheckingChangeEnabledButton()
        {
            if (!this.secondTab.TextBox2.Properties.IsEnabled)
            {
                this.secondTab.ChangeEnabledButton.Click();
            }

            this.secondTab.ChangeEnabledButton.Click();
            Assert.AreEqual(false, this.secondTab.TextBox2.Properties.IsEnabled);

            this.secondTab.ChangeEnabledButton.Click();
            Assert.AreEqual(true, this.secondTab.TextBox2.Properties.IsEnabled);
        }

        [Test]
        public void CheckingCheckBox2()
        {
            this.secondTab.CheckBox2.Uncheck();
            Assert.AreEqual(false, this.secondTab.CheckBox2.IsToggleOn, "Чекбокс в check состоянии после uncheck.");

            this.secondTab.CheckBox2.Check();
            Assert.AreEqual(true, this.secondTab.CheckBox2.IsToggleOn, "Чекбокс в uncheck состоянии после check.");
        }

        [Test]
        public void CheckingCheckListBox()
        {
            var month = this.secondTab.CheckListBox.ScrollToCheckBoxByName("December");

            month.Check();
            Assert.AreEqual(true, month.IsToggleOn, "Чекбокс December в uncheck состоянии после check.");
        }

        [Test]
        public void CheckingTabItem2()
        {
            this.secondTab.Select();
            Assert.AreEqual(true, this.secondTab.IsSelection, "Вкладка 2 оказалась не выбрана");
        }

        [Test]
        public void CheckingTextBox2()
        {
            this.secondTab.TextBox2.SetText("start test text");
            var startText = this.secondTab.TextBox2.Text();

            this.secondTab.TextBox2.SetText("new test text");
            var currentText = this.secondTab.TextBox2.Text();

            Assert.AreNotEqual(startText, currentText, "Текст не изменился.");
        }

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            TestClassHelper.Initialize(out this.application);

            this.secondTab = this.application.Window.TabItem2;
            this.secondTab.Select();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            TestClassHelper.Cleanup(this.application);
        }

        #endregion
    }
}
