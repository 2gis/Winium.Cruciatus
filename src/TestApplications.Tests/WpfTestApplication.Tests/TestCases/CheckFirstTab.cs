namespace WpfTestApplication.Tests.TestCases
{
    #region using

    using System.Threading;
    using System.Windows.Forms;

    using Cruciatus;
    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WpfTestApplication.Tests.Map;

    using ContextMenu = Cruciatus.Elements.ContextMenu;
    using Menu = Cruciatus.Elements.Menu;

    #endregion

    [CodedUITest]
    public class CheckFirstTab
    {
        private static bool _firstClassStartFlag = true;

        private static WpfTestApplicationApp _application;

        private static FirstTab _firstTab;

        private static ContextMenu _setTextButtonContextMenu;

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
            _firstTab = _application.MainWindow.TabItem1;
            _setTextButtonContextMenu = _application.MainWindow.SetTextButtonContextMenu;

            if (_firstClassStartFlag)
            {
                Assert.IsTrue(_firstTab.Select(), _firstTab.LastErrorMessage);
                _firstClassStartFlag = false;
            }
        }

        [TestMethod]
        public void CheckingTabItem2()
        {
            Assert.IsTrue(_firstTab.Select(), _firstTab.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingSetTextButton()
        {
            Assert.IsTrue(_firstTab.SetTextButton.Click(), _firstTab.SetTextButton.LastErrorMessage);

            var currentText = _firstTab.TextBox1.Text;
            Assert.IsNotNull(currentText, _firstTab.TextBox1.LastErrorMessage);

            Assert.AreEqual(currentText, "CARAMBA", "Верный текст не установлен в текстовое поле.");
        }

        [TestMethod]
        public void CheckingSetTextButtonContextMenu1()
        {
            Assert.IsTrue(_firstTab.SetTextButton.Click(MouseButtons.Right), _firstTab.SetTextButton.LastErrorMessage);
            Assert.IsTrue(_setTextButtonContextMenu.SelectItem("Menu item 1"),
                          _setTextButtonContextMenu.LastErrorMessage);

            Assert.IsTrue(_firstTab.SetTextButton.Click(MouseButtons.Right), _firstTab.SetTextButton.LastErrorMessage);
            Assert.IsFalse(_setTextButtonContextMenu.ItemIsEnabled("Menu item 3"), "Пункт Menu item 3 оказался активен.");
            Keyboard.SendKeys("{ESCAPE}");
        }

        [TestMethod]
        public void CheckingTextBox1()
        {
            var startText = _firstTab.TextBox1.Text;
            Assert.IsNotNull(startText, _firstTab.TextBox1.LastErrorMessage);

            Assert.IsTrue(_firstTab.TextBox1.SetText("new test text"), _firstTab.TextBox1.LastErrorMessage);

            var currentText = _firstTab.TextBox1.Text;
            Assert.IsNotNull(currentText, _firstTab.TextBox1.LastErrorMessage);

            Assert.AreNotEqual(startText, currentText, "Текст не изменился.");
        }

        [TestMethod]
        public void CheckingTextComboBox()
        {
            Assert.IsTrue(_firstTab.TextComboBox.Expand(), _firstTab.TextComboBox.LastErrorMessage);

            var element = _firstTab.TextComboBox.Item<TextBlock>("Quarter");
            Assert.IsNotNull(element, _firstTab.TextComboBox.LastErrorMessage);

            Assert.IsTrue(element.Click(), element.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingCheckBox1()
        {
            Assert.IsTrue(_firstTab.CheckBox1.Uncheck(), _firstTab.CheckBox1.LastErrorMessage);
            Assert.IsFalse(_firstTab.CheckBox1.IsChecked, "Чекбокс в check состоянии после uncheck.");

            Assert.IsTrue(_firstTab.CheckBox1.Check(), _firstTab.CheckBox1.LastErrorMessage);
            Assert.IsTrue(_firstTab.CheckBox1.IsChecked, "Чекбокс в uncheck состоянии после check.");
        }

        [TestMethod]
        public void CheckingTextListBox()
        {
            if (!_firstTab.TextListBox.IsEnabled)
            {
                Assert.IsTrue(_firstTab.CheckBox1.Check(), _firstTab.CheckBox1.LastErrorMessage);
            }

            var month = _firstTab.TextListBox.ScrollTo<TextBlock>("December");
            Assert.IsNotNull(month, _firstTab.TextListBox.LastErrorMessage);
            Assert.IsTrue(month.Click(), month.LastErrorMessage);

            month = _firstTab.TextListBox.ScrollTo<TextBlock>("October");
            Assert.IsNotNull(month, _firstTab.TextListBox.LastErrorMessage);
            Assert.IsTrue(month.Click(), month.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingChangeEnabledTextListBox()
        {
            Assert.IsTrue(_firstTab.TextListBox.IsEnabled, "TextListBox в начале оказался не включен.");

            Assert.IsTrue(_firstTab.CheckBox1.Uncheck(), _firstTab.CheckBox1.LastErrorMessage);
            Assert.IsFalse(_firstTab.CheckBox1.IsChecked, "Чекбокс в check состоянии после uncheck.");

            Assert.IsFalse(_firstTab.TextListBox.IsEnabled, "TextListBox не стал включенным.");
        }
    }
}
