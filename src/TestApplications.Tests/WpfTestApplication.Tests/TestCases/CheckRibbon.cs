namespace WpfTestApplication.Tests.TestCases
{
    #region using

    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WpfTestApplication.Tests.Map;

    #endregion

    [CodedUITest]
    public class CheckRibbon
    {
        private static bool _firstClassStartFlag = true;

        private static WpfTestApplicationApp _application;

        private FirstRibbonTab _firstRibbonTab;

        private SecondRibbonTab _secondRibbonTab;

        private RibbonApplicationMenu _ribbonMenu;

        private Menu _simpleMenu;

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
            _simpleMenu = _application.MainWindow.SimpleMenu;
            _ribbonMenu = _application.MainWindow.RibbonMenu;
            _firstRibbonTab = _application.MainWindow.RibbonTabItem1;
            _secondRibbonTab = _application.MainWindow.RibbonTabItem2;

            if (_firstClassStartFlag)
            {
                _firstClassStartFlag = false;
            }
        }

        [TestMethod]
        public void SimpleMenuTestMethod1()
        {
            const string headersPath = "Level1$MultiLevel2$Level3";
            Assert.IsTrue(_simpleMenu.SelectItem(headersPath), _simpleMenu.LastErrorMessage);
        }

        [TestMethod]
        public void SimpleMenuTestMethod2()
        {
            const string headersPath = "Level1$MultiLevel2$MultiLevel3$MultiLevel4$Level5";
            Assert.IsTrue(_simpleMenu.SelectItem(headersPath), _simpleMenu.LastErrorMessage);
        }

        [TestMethod]
        public void RibbonMenuTestMethod1()
        {
            Assert.IsTrue(_ribbonMenu.SelectItem("Print$Open"), _ribbonMenu.LastErrorMessage);
        }

        [TestMethod]
        public void RibbonMenuTestMethod2()
        {
            Assert.IsTrue(_ribbonMenu.SelectItem("Print$New$Save"), _ribbonMenu.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingTabItem1()
        {
            Assert.IsTrue(_application.MainWindow.TabItem1.Select(), _application.MainWindow.TabItem1.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingRibbonTabItem1()
        {
            Assert.IsTrue(_firstRibbonTab.Select(), _firstRibbonTab.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingRibbonButton()
        {
            Assert.IsTrue(_firstRibbonTab.Select(), _firstRibbonTab.LastErrorMessage);

            Assert.IsTrue(_firstRibbonTab.RibbonButton.Click(), _firstRibbonTab.RibbonButton.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingRibbonTextComboBox()
        {
            Assert.IsTrue(_firstRibbonTab.Select(), _firstRibbonTab.LastErrorMessage);

            Assert.IsTrue(_firstRibbonTab.RibbonTextComboBox.Expand(), 
                          _firstRibbonTab.RibbonTextComboBox.LastErrorMessage);

            var element = _firstRibbonTab.RibbonTextComboBox.Item<TextBlock>("Quarter");
            Assert.IsNotNull(element, _firstRibbonTab.RibbonTextComboBox.LastErrorMessage);

            Assert.IsTrue(element.Click(), element.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingRibbonCheckComboBox()
        {
            Assert.IsTrue(_firstRibbonTab.Select(), _firstRibbonTab.LastErrorMessage);

            Assert.IsTrue(_firstRibbonTab.RibbonCheckComboBox.Expand(), 
                          _firstRibbonTab.RibbonCheckComboBox.LastErrorMessage);

            var element = _firstRibbonTab.RibbonCheckComboBox.Item<CheckBox>("Quarter");
            Assert.IsNotNull(element, _firstRibbonTab.RibbonCheckComboBox.LastErrorMessage);

            Assert.IsTrue(element.Check(), element.LastErrorMessage);
            Assert.IsTrue(element.IsChecked, "Чекбокс Quarter в uncheck состоянии после check.");

            element = _firstRibbonTab.RibbonCheckComboBox.Item<CheckBox>("Week");
            Assert.IsNotNull(element, _firstRibbonTab.RibbonCheckComboBox.LastErrorMessage);

            Assert.IsTrue(element.Check(), element.LastErrorMessage);
            Assert.IsTrue(element.IsChecked, "Чекбокс Week в uncheck состоянии после check.");

            Assert.IsTrue(_firstRibbonTab.RibbonCheckComboBox.Collapse(), 
                          _firstRibbonTab.RibbonCheckComboBox.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingRibbonTabItem2()
        {
            Assert.IsTrue(_secondRibbonTab.Select(), _secondRibbonTab.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingRibbonCheckBox()
        {
            Assert.IsTrue(_secondRibbonTab.Select(), _secondRibbonTab.LastErrorMessage);

            Assert.IsTrue(_secondRibbonTab.RibbonCheckBox.Uncheck(), _secondRibbonTab.RibbonCheckBox.LastErrorMessage);
            Assert.IsFalse(_secondRibbonTab.RibbonCheckBox.IsChecked, "Чекбокс в check состоянии после uncheck.");

            Assert.IsTrue(_secondRibbonTab.RibbonCheckBox.Check(), _secondRibbonTab.RibbonCheckBox.LastErrorMessage);
            Assert.IsTrue(_secondRibbonTab.RibbonCheckBox.IsChecked, "Чекбокс в uncheck состоянии после check.");
        }

        [TestMethod]
        public void CheckingRibbonToggleButton()
        {
            Assert.IsTrue(_secondRibbonTab.Select(), _secondRibbonTab.LastErrorMessage);

            Assert.IsTrue(_secondRibbonTab.RibbonToggleButton.Uncheck(), 
                          _secondRibbonTab.RibbonToggleButton.LastErrorMessage);
            Assert.IsFalse(_secondRibbonTab.RibbonToggleButton.IsChecked, "Чекбокс в check состоянии после uncheck.");

            Assert.IsTrue(_secondRibbonTab.RibbonToggleButton.Check(), 
                          _secondRibbonTab.RibbonToggleButton.LastErrorMessage);
            Assert.IsTrue(_secondRibbonTab.RibbonToggleButton.IsChecked, "Чекбокс в uncheck состоянии после check.");
        }
    }
}
