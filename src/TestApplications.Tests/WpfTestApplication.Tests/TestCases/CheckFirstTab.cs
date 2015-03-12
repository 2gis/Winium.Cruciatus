namespace WpfTestApplication.Tests.TestCases
{
    #region using

    using Cruciatus;
    using Cruciatus.Core;
    using Cruciatus.Elements;

    using NUnit.Framework;

    using WpfTestApplication.Tests.Map;

    #endregion

    [TestFixture]
    public class CheckFirstTab
    {
        #region Static Fields

        private static WpfTestApplicationApp application;

        private static FirstTab firstTab;

        private static Menu setTextButtonContextMenu;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void CheckingChangeEnabledTextListBox()
        {
            Assert.IsTrue(firstTab.TextListBox.Properties.IsEnabled, "TextListBox в начале оказался не включен.");

            firstTab.CheckBox1.Uncheck();
            Assert.IsFalse(firstTab.CheckBox1.IsToggleOn, "Чекбокс в check состоянии после uncheck.");

            Assert.IsFalse(firstTab.TextListBox.Properties.IsEnabled, "TextListBox не стал включенным.");
        }

        [Test]
        public void CheckingCheckBox1()
        {
            firstTab.CheckBox1.Uncheck();
            Assert.IsFalse(firstTab.CheckBox1.IsToggleOn, "Чекбокс в check состоянии после uncheck.");

            firstTab.CheckBox1.Check();
            Assert.IsTrue(firstTab.CheckBox1.IsToggleOn, "Чекбокс в uncheck состоянии после check.");
        }

        [Test]
        public void CheckingSetTextButton()
        {
            firstTab.SetTextButton.Click();

            var currentText = firstTab.TextBox1.Text();
            Assert.AreEqual(currentText, "CARAMBA", "Верный текст не установлен в текстовое поле.");
        }

        [Test]
        public void CheckingSetTextButtonContextMenu1()
        {
            firstTab.SetTextButton.Click(MouseButton.Right);
            setTextButtonContextMenu.SelectItem("Menu item 1");

            firstTab.SetTextButton.Click(MouseButton.Right);
            Assert.IsFalse(
                setTextButtonContextMenu.GetItem("Menu item 3").Properties.IsEnabled, 
                "Пункт Menu item 3 оказался активен.");
            CruciatusFactory.Keyboard.SendEscape();
        }

        [Test]
        public void CheckingTabItem2()
        {
            firstTab.Select();
        }

        [Test]
        public void CheckingTextBox1()
        {
            const string Text = "new test text";

            firstTab.TextBox1.SetText(Text);
            var currentText = firstTab.TextBox1.Text();

            Assert.AreEqual(Text, currentText, "Текст не изменился.");
        }

        [Test]
        public void CheckingTextComboBox()
        {
            firstTab.TextComboBox.Expand();

            var element = firstTab.TextComboBox.FindElement(By.Name("Quarter"));
            Assert.IsNotNull(element);

            element.Click();
        }

        [Test]
        public void CheckingTextListBox()
        {
            if (!firstTab.TextListBox.Properties.IsEnabled)
            {
                firstTab.CheckBox1.Check();
            }

            firstTab.TextListBox.ScrollTo(By.Name("December")).Click();

            firstTab.TextListBox.ScrollTo(By.Name("October")).Click();
        }

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            TestClassHelper.Initialize(out application);

            firstTab = application.MainWindow.TabItem1;
            setTextButtonContextMenu = application.MainWindow.SetTextButtonContextMenu;

            firstTab.Select();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            TestClassHelper.Cleanup(application);
        }

        #endregion
    }
}
