namespace WpfTestApplication.Tests
{
    #region using

    using NUnit.Framework;

    using WpfTestApplication.Tests.Map;

    #endregion

    [TestFixture]
    public class CheckElementIsStaleProperty
    {
        #region Fields

        private WpfTestApplicationApp application;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ElementNotStaleTest()
        {
            Assert.IsFalse(this.application.MainWindow.IsStale);
        }

        [Test]
        public void FoundElementStaleTest()
        {
            var mainWindow = this.FindMainWindow();

            this.CloseAppWithRetry();
            Assert.IsTrue(mainWindow.IsStale);
        }

        [Test]
        public void NotFoundElementStaleTest()
        {
            this.CloseAppWithRetry();
            Assert.IsTrue(this.application.MainWindow.IsStale);
        }

        [Test]
        public void NotFoundElementStaleWhenParentStaleTest()
        {
            var mainWindow = this.FindMainWindow();

            this.CloseAppWithRetry();
            Assert.IsTrue(mainWindow.TabItem1.IsStale);
        }

        [SetUp]
        public void SetUp()
        {
            TestClassHelper.Initialize(out this.application);
        }

        [TearDown]
        public void TearDown()
        {
            TestClassHelper.Cleanup(this.application);
        }

        #endregion

        #region Methods

        private void CloseAppWithRetry()
        {
            Assert.That(() => this.application.Close(), Is.True.After(2000));
        }

        private MainWindow FindMainWindow()
        {
            var mainWindow = this.application.MainWindow;

            // Needed for real find main window element
            var winName = mainWindow.Properties.Name;

            return mainWindow;
        }

        #endregion
    }
}
