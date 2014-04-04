namespace WindowsFormsTestApplication.Tests.TestCases
{
    #region using

    using WindowsFormsTestApplication.Tests.Map;

    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    #endregion

    [CodedUITest]
    public class CheckFirstTab
    {
        private static bool _firstClassStartFlag = true;

        private static WindowsFormsTestApplicationApp _application;

        // Так пока не работает, поэтому временно из MainWindow
        ////private FirstTab _firstTab;
        private MainWindow _firstTab;

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
            // Так пока не работает, поэтому временно из MainWindow
            ////_firstTab = _application.MainWindow.TabItem1;
            _firstTab = _application.MainWindow;

            if (_firstClassStartFlag)
            {
                // Это пока не работает
                ////Assert.IsTrue(_firstTab.Select(), _firstTab.LastErrorMessage);
                _firstClassStartFlag = false;
            }
        }

        [TestMethod]
        public void CheckingTabItem1()
        {
            Assert.Inconclusive("Ручная остановка. В винформс вкладку надо искать по имени.");
            Assert.IsTrue(_application.MainWindow.TabItem1.Select(), _application.MainWindow.TabItem1.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingSetTextButton()
        {
            Assert.IsTrue(_firstTab.SetTextButton.Click(), _firstTab.SetTextButton.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingTextBox1()
        {
            const string startText = "new test text";
            Assert.IsTrue(_firstTab.TextBox1.SetText(null), _firstTab.TextBox1.LastErrorMessage);
            Assert.IsTrue(_firstTab.TextBox1.SetText(startText), _firstTab.TextBox1.LastErrorMessage);

            var currentText = _firstTab.TextBox1.Text;
            Assert.IsNotNull(currentText, _firstTab.TextBox1.LastErrorMessage);

            Assert.AreEqual(startText, currentText, "Текст после ввода не стал new test text.");
        }

        [TestMethod]
        public void CheckingTextComboBox()
        {
            Assert.Inconclusive("Ручная остановка. В винформс c ComboBox круциатус пока нормально не работает.");
            Assert.IsTrue(_firstTab.TextComboBox.Expand(), _firstTab.TextComboBox.LastErrorMessage);

            var element = _firstTab.TextComboBox.Item<TextBlock>("Quarter");
            Assert.IsNotNull(element, _firstTab.TextComboBox.LastErrorMessage);

            Assert.IsTrue(element.Click(), element.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingTextListBox()
        {
            Assert.Inconclusive("Ручная остановка. В винформс невидимый элемент списка не имеет точки клика.");
            var month = _firstTab.TextListBox.ScrollTo<TextBlock>("December");
            Assert.IsNotNull(month, _firstTab.TextListBox.LastErrorMessage);
            Assert.IsTrue(month.Click(), month.LastErrorMessage);

            month = _firstTab.TextListBox.ScrollTo<TextBlock>("October");
            Assert.IsNotNull(month, _firstTab.TextListBox.LastErrorMessage);
            Assert.IsTrue(month.Click(), month.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingSetTextToTextBox1()
        {
            Assert.IsTrue(_firstTab.TextBox1.SetText(null), _firstTab.TextBox1.LastErrorMessage);
            Assert.IsTrue(_firstTab.SetTextButton.Click(), _firstTab.SetTextButton.LastErrorMessage);

            var currentText = _firstTab.TextBox1.Text;
            Assert.IsNotNull(currentText, _firstTab.TextBox1.LastErrorMessage);

            Assert.AreEqual("CARAMBA", currentText, "После клика текст не стал = CARAMBA.");
        }

        #region черная дыра

        // Тут происходит неведомая фигня в виде (при последовательном старте тестов на одном экземпляре приложения):
        // один тест отрабатывает нормально с чекбоксом, а вот во втором чекбокс говорит,
        // что не поддерживает свойство TogglePattern.ToggleStateProperty
        ////[TestMethod]
        public void CheckingCheckBox1()
        {
            Assert.IsTrue(_firstTab.CheckBox1.Uncheck(), _firstTab.CheckBox1.LastErrorMessage);
            Assert.IsFalse(_firstTab.CheckBox1.IsChecked, "Чекбокс в check состоянии после uncheck.");

            Assert.IsTrue(_firstTab.CheckBox1.Check(), _firstTab.CheckBox1.LastErrorMessage);
            Assert.IsTrue(_firstTab.CheckBox1.IsChecked, "Чекбокс в uncheck состоянии после check.");
        }

        ////[TestMethod]
        public void CheckingChangeEnabledTextListBox()
        {
            Assert.IsTrue(_firstTab.CheckBox1.Uncheck(), _firstTab.CheckBox1.LastErrorMessage);
            Assert.IsFalse(_firstTab.CheckBox1.IsChecked, "Чекбокс в check состоянии после uncheck.");
            Assert.IsFalse(_firstTab.TextListBox.IsEnabled, "TextListBox включен после uncheсk-а.");

            Assert.IsTrue(_firstTab.CheckBox1.Check(), _firstTab.CheckBox1.LastErrorMessage);
            Assert.IsTrue(_firstTab.CheckBox1.IsChecked, "Чекбокс в uncheck состоянии после check.");
            Assert.IsTrue(_firstTab.TextListBox.IsEnabled, "TextListBox выключен после cheсk-а.");
        }

        #endregion
    }
}
