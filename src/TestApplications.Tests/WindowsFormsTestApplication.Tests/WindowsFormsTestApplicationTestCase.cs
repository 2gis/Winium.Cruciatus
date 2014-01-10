
namespace WindowsFormsTestApplication.Tests
{
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WindowsFormsTestApplication.Tests.Map;

    /// <summary>
    /// Summary desсription for WindowsFormsTestApplicationTestCase
    /// </summary>
    [CodedUITest]
    public class WindowsFormsTestApplicationTestCase
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

        protected WindowsFormsTestApplicationApp Application { get; set; }

        [TestInitialize]
        public void MyTestInitialize()
        {
            this.Application =
                new WindowsFormsTestApplicationApp(
                    @"F:\Projects\cruciatus\src\TestApplications\WindowsFormsTestApplication\bin\Debug\WindowsFormsTestApplication.exe");
            Assert.IsTrue(this.Application.Start(), "Не удалось запустить приложение WindowsFormsTestApplication.");
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            var additionalMessage = string.Empty;
            if (this.testContextInstance.CurrentTestOutcome != UnitTestOutcome.Passed)
            {
                additionalMessage = " A так же была ошибка внутри теста.";
            }

            Assert.IsTrue(this.Application.Close(), "Не удалось завершить приложение WindowsFormsTestApplication." + additionalMessage);
        }
    }
}
