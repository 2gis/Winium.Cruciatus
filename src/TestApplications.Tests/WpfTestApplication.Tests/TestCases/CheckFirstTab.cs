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
        private static WpfTestApplicationApp _application;

        private static FirstTab _firstTab;

        private static Menu _setTextButtonContextMenu;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            TestClassHelper.Initialize(out _application);

            _firstTab = _application.MainWindow.TabItem1;
            _setTextButtonContextMenu = _application.MainWindow.SetTextButtonContextMenu;

            _firstTab.Select();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            TestClassHelper.Cleanup(_application);
        }

        [Test]
        public void CheckingTabItem2()
        {
            _firstTab.Select();
        }

        [Test]
        public void CheckingSetTextButton()
        {
            _firstTab.SetTextButton.Click();

            var currentText = _firstTab.TextBox1.Text();
            Assert.AreEqual(currentText, "CARAMBA", "Верный текст не установлен в текстовое поле.");
        }

        [Test]
        public void CheckingSetTextButtonContextMenu1()
        {
            _firstTab.SetTextButton.Click(MouseButton.Right);
            _setTextButtonContextMenu.SelectItem("Menu item 1");

            _firstTab.SetTextButton.Click(MouseButton.Right);
            Assert.IsFalse(_setTextButtonContextMenu.GetItem("Menu item 3").Properties.IsEnabled, "Пункт Menu item 3 оказался активен.");
            CruciatusFactory.Keyboard.SendEscape();
        }

        [Test]
        public void CheckingTextBox1()
        {
            const string text = "new test text";
            
            _firstTab.TextBox1.SetText(text);
            var currentText = _firstTab.TextBox1.Text();

            Assert.AreEqual(text, currentText, "Текст не изменился.");
        }

        [Test]
        public void CheckingTextComboBox()
        {
            _firstTab.TextComboBox.Expand();

            var element = _firstTab.TextComboBox.Get(By.Name("Quarter"));
            Assert.IsNotNull(element);
            
            element.Click();
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
        public void CheckingTextListBox()
        {
            if (!_firstTab.TextListBox.Properties.IsEnabled)
            {
                _firstTab.CheckBox1.Check();
            }

            _firstTab.TextListBox.ScrollTo(By.Name("December")).Click();

            _firstTab.TextListBox.ScrollTo(By.Name("October")).Click();
        }

        [Test]
        public void CheckingChangeEnabledTextListBox()
        {
            Assert.IsTrue(_firstTab.TextListBox.Properties.IsEnabled, "TextListBox в начале оказался не включен.");

            _firstTab.CheckBox1.Uncheck();
            Assert.IsFalse(_firstTab.CheckBox1.IsToggleOn, "Чекбокс в check состоянии после uncheck.");

            Assert.IsFalse(_firstTab.TextListBox.Properties.IsEnabled, "TextListBox не стал включенным.");
        }
    }
}
