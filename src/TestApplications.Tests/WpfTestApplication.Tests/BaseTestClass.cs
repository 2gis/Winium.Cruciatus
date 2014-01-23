
namespace WpfTestApplication.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WpfTestApplication.Tests.Map;

    [CodedUITest]
    public class BaseTestClass
    {
        public TestContext TestContext { get; set; }

        public static void ClassInitialize(out WpfTestApplicationApp application, TestContext testContext)
        {
            var path = Environment.GetEnvironmentVariable("UITestApps");
            application = new WpfTestApplicationApp(path + @"\WpfTestApplication\bin\Debug\WpfTestApplication.exe");
            Assert.IsTrue(application.Start(15000), "Не удалось запустить приложение WpfTestApplication.");
        }

        public static void ClassCleanup(WpfTestApplicationApp application)
        {
            //var additionalMessage = string.Empty;
            //if (testContextInstance.CurrentTestOutcome != UnitTestOutcome.Passed)
            //{
            //    additionalMessage = " A так же была ошибка внутри теста.";
            //}

            Assert.IsTrue(application.Close(), "Не удалось завершить приложение WpfTestApplication."); //+ additionalMessage);
        }
    }
}
