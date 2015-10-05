namespace WpfTestApplication.Tests
{
    #region using

    using System.Linq;

    using NUnit.Framework;

    using Winium.Cruciatus;
    using Winium.Cruciatus.Core;

    using WpfTestApplication.Tests.Map;

    #endregion

    [TestFixture]
    public class CheckFindByXPath
    {
        #region Fields

        private WpfTestApplicationApp application;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void FindElementsByPartName()
        {
            const string XPath = "*[starts-with(@Name, 'M')]";
            var results = this.application.MainWindow.TabItem1.TextListBox.FindElements(By.XPath(XPath)).ToList();
            Assert.That(results, Has.Count.EqualTo(2));
        }

        [Test]
        public void FindMainWindowFromRoot()
        {
            const string XPath = "*[@AutomationId='WpfTestApplicationMainWindow']";
            var mainWindow = CruciatusFactory.Root.FindElement(By.XPath(XPath));
            Assert.AreEqual(mainWindow, this.application.MainWindow);
        }

        [Test]
        public void FindMainWindowUseRootXPathFunction()
        {
            const string XPath = "/*[@AutomationId='WpfTestApplicationMainWindow']";
            var mainWindow = this.application.MainWindow.TabItem1.SetTextButton.FindElement(By.XPath(XPath));
            Assert.AreEqual(mainWindow, this.application.MainWindow);
        }

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            TestClassHelper.Initialize(out this.application);
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            TestClassHelper.Cleanup(this.application);
        }

        #endregion
    }
}
