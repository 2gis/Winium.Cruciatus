namespace WpfTestApplication.Tests.TestCases
{
    #region using

    using NUnit.Framework;

    using Winium.Cruciatus.Core;
    using Winium.Cruciatus.Elements;

    using WpfTestApplication.Tests.Map;

    #endregion

    [TestFixture]
    public class CheckRibbon
    {
        #region Fields

        private WpfTestApplicationApp application;

        private FirstRibbonTab firstRibbonTab;

        private Menu ribbonMenu;

        private SecondRibbonTab secondRibbonTab;

        private Menu simpleMenu;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void CheckingRibbonButton()
        {
            this.firstRibbonTab.Select();

            this.firstRibbonTab.RibbonButton.Click();
        }

        [Test]
        public void CheckingRibbonCheckBox()
        {
            this.secondRibbonTab.Select();

            this.secondRibbonTab.RibbonCheckBox.Uncheck();
            Assert.IsFalse(this.secondRibbonTab.RibbonCheckBox.IsToggleOn, "Чекбокс в check состоянии после uncheck.");

            this.secondRibbonTab.RibbonCheckBox.Check();
            Assert.IsTrue(this.secondRibbonTab.RibbonCheckBox.IsToggleOn, "Чекбокс в uncheck состоянии после check.");
        }

        [Test]
        public void CheckingRibbonCheckComboBox()
        {
            this.firstRibbonTab.Select();

            this.firstRibbonTab.RibbonCheckComboBox.Expand();
            Assert.IsTrue(this.firstRibbonTab.RibbonCheckComboBox.IsExpanded);

            var element = this.firstRibbonTab.RibbonCheckComboBox.GetCheckBoxByName("Quarter");
            element.Check();
            Assert.IsTrue(element.IsToggleOn, "Чекбокс Quarter в uncheck состоянии после check.");

            element = this.firstRibbonTab.RibbonCheckComboBox.GetCheckBoxByName("Week");
            element.Check();
            Assert.IsTrue(element.IsToggleOn, "Чекбокс Week в uncheck состоянии после check.");

            this.firstRibbonTab.RibbonCheckComboBox.Collapse();
        }

        [Test]
        public void CheckingRibbonTabItem1()
        {
            this.firstRibbonTab.Select();
            Assert.IsTrue(this.firstRibbonTab.IsSelection);
        }

        [Test]
        public void CheckingRibbonTabItem2()
        {
            this.secondRibbonTab.Select();
            Assert.IsTrue(this.secondRibbonTab.IsSelection);
        }

        [Test]
        public void CheckingRibbonTextComboBox()
        {
            this.firstRibbonTab.Select();

            this.firstRibbonTab.RibbonTextComboBox.Expand();
            Assert.IsTrue(this.firstRibbonTab.RibbonTextComboBox.IsExpanded);

            this.firstRibbonTab.RibbonTextComboBox.FindElement(By.Name("Quarter")).Click();
        }

        [Test]
        public void CheckingRibbonToggleButton()
        {
            this.secondRibbonTab.Select();

            this.secondRibbonTab.RibbonToggleButton.Uncheck();
            Assert.IsFalse(
                this.secondRibbonTab.RibbonToggleButton.IsToggleOn, 
                "Чекбокс в check состоянии после uncheck.");

            this.secondRibbonTab.RibbonToggleButton.Check();
            Assert.IsTrue(
                this.secondRibbonTab.RibbonToggleButton.IsToggleOn, 
                "Чекбокс в uncheck состоянии после check.");
        }

        [Test]
        public void CheckingTabItem1()
        {
            this.application.MainWindow.TabItem1.Select();
            Assert.IsTrue(this.application.MainWindow.TabItem1.IsSelection);
        }

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            TestClassHelper.Initialize(out this.application);

            this.simpleMenu = this.application.MainWindow.SimpleMenu;
            this.ribbonMenu = this.application.MainWindow.RibbonMenu;
            this.firstRibbonTab = this.application.MainWindow.RibbonTabItem1;
            this.secondRibbonTab = this.application.MainWindow.RibbonTabItem2;
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            TestClassHelper.Cleanup(this.application);
        }

        [Test]
        public void RibbonMenuTestMethod1()
        {
            this.ribbonMenu.Click();
            this.ribbonMenu.SelectItem("Print$Open");
        }

        [Test]
        public void RibbonMenuTestMethod2()
        {
            this.ribbonMenu.Click();
            this.ribbonMenu.SelectItem("Print$New$Save");
        }

        [Test]
        public void SimpleMenuTestMethod1()
        {
            const string HeadersPath = "Level1$MultiLevel2$Level3";
            this.simpleMenu.SelectItem(HeadersPath);
        }

        [Test]
        public void SimpleMenuTestMethod2()
        {
            const string HeadersPath = "Level1$MultiLevel2$MultiLevel3$MultiLevel4$Level5";
            this.simpleMenu.SelectItem(HeadersPath);
            this.simpleMenu.SelectItem(HeadersPath);
        }

        #endregion
    }
}
