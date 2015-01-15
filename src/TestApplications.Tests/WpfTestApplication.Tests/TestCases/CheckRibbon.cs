namespace WpfTestApplication.Tests.TestCases
{
    #region using

    using Cruciatus.Core;
    using Cruciatus.Elements;

    using NUnit.Framework;

    using WpfTestApplication.Tests.Map;

    #endregion

    [TestFixture]
    public class CheckRibbon
    {
        private WpfTestApplicationApp _application;

        private FirstRibbonTab _firstRibbonTab;

        private SecondRibbonTab _secondRibbonTab;

        private RibbonApplicationMenu _ribbonMenu;

        private Menu _simpleMenu;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            TestClassHelper.Initialize(out _application);

            _simpleMenu = _application.MainWindow.SimpleMenu;
            _ribbonMenu = _application.MainWindow.RibbonMenu;
            _firstRibbonTab = _application.MainWindow.RibbonTabItem1;
            _secondRibbonTab = _application.MainWindow.RibbonTabItem2;
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            TestClassHelper.Cleanup(_application);
        }

        [Test]
        public void SimpleMenuTestMethod1()
        {
            const string headersPath = "Level1$MultiLevel2$Level3";
            _simpleMenu.SelectItem(headersPath);
        }

        [Test]
        public void SimpleMenuTestMethod2()
        {
            const string headersPath = "Level1$MultiLevel2$MultiLevel3$MultiLevel4$Level5";
            _simpleMenu.SelectItem(headersPath);
            _simpleMenu.SelectItem(headersPath);
        }

        [Test]
        public void RibbonMenuTestMethod1()
        {
            _ribbonMenu.SelectItem("Print$Open");
        }

        [Test]
        public void RibbonMenuTestMethod2()
        {
            _ribbonMenu.SelectItem("Print$New$Save");
        }

        [Test]
        public void CheckingTabItem1()
        {
            _application.MainWindow.TabItem1.Select();
            Assert.IsTrue(_application.MainWindow.TabItem1.IsSelection);
        }

        [Test]
        public void CheckingRibbonTabItem1()
        {
            _firstRibbonTab.Select();
            Assert.IsTrue(_firstRibbonTab.IsSelection);
        }

        [Test]
        public void CheckingRibbonButton()
        {
            _firstRibbonTab.Select();

            _firstRibbonTab.RibbonButton.Click();
        }

        [Test]
        public void CheckingRibbonTextComboBox()
        {
            _firstRibbonTab.Select();
            
            _firstRibbonTab.RibbonTextComboBox.Expand();
            Assert.IsTrue(_firstRibbonTab.RibbonTextComboBox.IsExpanded);

            _firstRibbonTab.RibbonTextComboBox.Get(By.Name("Quarter")).Click();
        }

        [Test]
        public void CheckingRibbonCheckComboBox()
        {
            _firstRibbonTab.Select();

            _firstRibbonTab.RibbonCheckComboBox.Expand();
            Assert.IsTrue(_firstRibbonTab.RibbonCheckComboBox.IsExpanded);

            var element = _firstRibbonTab.RibbonCheckComboBox.GetCheckBoxByName("Quarter");
            element.Check();
            Assert.IsTrue(element.IsToggleOn, "Чекбокс Quarter в uncheck состоянии после check.");

            element = _firstRibbonTab.RibbonCheckComboBox.GetCheckBoxByName("Week");
            element.Check();
            Assert.IsTrue(element.IsToggleOn, "Чекбокс Week в uncheck состоянии после check.");

            _firstRibbonTab.RibbonCheckComboBox.Collapse();
        }

        [Test]
        public void CheckingRibbonTabItem2()
        {
            _secondRibbonTab.Select();
            Assert.IsTrue(_secondRibbonTab.IsSelection);
        }

        [Test]
        public void CheckingRibbonCheckBox()
        {
            _secondRibbonTab.Select();

            _secondRibbonTab.RibbonCheckBox.Uncheck();
            Assert.IsFalse(_secondRibbonTab.RibbonCheckBox.IsToggleOn, "Чекбокс в check состоянии после uncheck.");

            _secondRibbonTab.RibbonCheckBox.Check();
            Assert.IsTrue(_secondRibbonTab.RibbonCheckBox.IsToggleOn, "Чекбокс в uncheck состоянии после check.");
        }

        [Test]
        public void CheckingRibbonToggleButton()
        {
            _secondRibbonTab.Select();

            _secondRibbonTab.RibbonToggleButton.Uncheck();
            Assert.IsFalse(_secondRibbonTab.RibbonToggleButton.IsToggleOn, "Чекбокс в check состоянии после uncheck.");

            _secondRibbonTab.RibbonToggleButton.Check();
            Assert.IsTrue(_secondRibbonTab.RibbonToggleButton.IsToggleOn, "Чекбокс в uncheck состоянии после check.");
        }
    }
}
