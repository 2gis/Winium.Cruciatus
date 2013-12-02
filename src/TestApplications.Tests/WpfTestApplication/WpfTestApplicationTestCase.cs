
namespace WpfTestApplication
{
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WpfTestApplication.Map;

    /// <summary>
    /// Summary desсription for WpfTestApplicationTestCase
    /// </summary>
    [CodedUITest]
    public class WpfTestApplicationTestCase
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return this.testContextInstance;
            }

            set
            {
                this.testContextInstance = value;
            }
        }

        protected WpfTestApplicationApp Application { get; set; }

        [TestInitialize]
        public void MyTestInitialize()
        {
            this.Application = new WpfTestApplicationApp(@"F:\Projects\cruciatus\src\TestApplications\WpfTestApplication\bin\Debug\WpfTestApplication.exe");
            Assert.IsTrue(this.Application.Start(), "Неудалось запустить приложение WpfTestApplication.");
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            var additionalMessage = string.Empty;
            if (this.testContextInstance.CurrentTestOutcome != UnitTestOutcome.Passed)
            {
                additionalMessage = " A так же была ошибка внутри теста.";
            }

            Assert.IsTrue(this.Application.Close(), "Неудалось завершить приложение WpfTestApplication." + additionalMessage);
        }
    }
}
