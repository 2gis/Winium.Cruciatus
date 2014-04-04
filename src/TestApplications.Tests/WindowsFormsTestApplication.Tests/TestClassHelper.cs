namespace WindowsFormsTestApplication.Tests
{
    #region using

    using System;
    using System.Configuration;

    using WindowsFormsTestApplication.Tests.Map;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    #endregion

    public static class TestClassHelper
    {
        public static void ClassInitialize(out WindowsFormsTestApplicationApp application)
        {
            var appsFolderEnvVar = ConfigurationManager.AppSettings.Get("AppsFolderEnvVar");
            var appsFolder = Environment.GetEnvironmentVariable(appsFolderEnvVar);
            var appPath = appsFolder + ConfigurationManager.AppSettings.Get("PathToExe");
            application = new WindowsFormsTestApplicationApp(appPath);
            Assert.IsTrue(application.Start(), "Не удалось запустить приложение WindowsFormsTestApplication.");
        }

        public static void ClassCleanup(WindowsFormsTestApplicationApp application)
        {
            Assert.IsTrue(application.Close(), "Не удалось завершить приложение WindowsFormsTestApplication.");
        }
    }
}
