namespace WpfTestApplication.Tests
{
    #region using

    using System;
    using System.Configuration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WpfTestApplication.Tests.Map;

    #endregion

    public static class TestClassHelper
    {
        public static void ClassInitialize(out WpfTestApplicationApp application)
        {
            var appsFolderEnvVar = ConfigurationManager.AppSettings.Get("AppsFolderEnvVar");
            var appsFolder = Environment.GetEnvironmentVariable(appsFolderEnvVar);
            var appPath = appsFolder + ConfigurationManager.AppSettings.Get("PathToExe");
            application = new WpfTestApplicationApp(appPath);
            Assert.IsTrue(application.Start(15000), "Не удалось запустить приложение WpfTestApplication.");
        }

        public static void ClassCleanup(WpfTestApplicationApp application)
        {
            Assert.IsTrue(application.Close(), "Не удалось завершить приложение WpfTestApplication.");
        }
    }
}
