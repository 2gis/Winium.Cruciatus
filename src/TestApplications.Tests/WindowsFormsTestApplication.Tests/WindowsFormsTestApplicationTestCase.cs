
namespace WindowsFormsTestApplication.Tests
{
    using System;
    using System.Configuration;

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
            var appsFolderEnvVar = ConfigurationManager.AppSettings.Get("AppsFolderEnvVar");
            var appsFolder = Environment.GetEnvironmentVariable(appsFolderEnvVar);
            var appPath = appsFolder + ConfigurationManager.AppSettings.Get("PathToExe");
            this.Application = new WindowsFormsTestApplicationApp(appPath);
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
