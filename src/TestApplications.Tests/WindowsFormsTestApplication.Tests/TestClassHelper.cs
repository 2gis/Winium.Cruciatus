namespace WindowsFormsTestApplication.Tests
{
    #region using

    using System;
    using System.Configuration;

    using WindowsFormsTestApplication.Tests.Map;

    using NUnit.Framework;

    #endregion

    public static class TestClassHelper
    {
        public static void Initialize(out WindowsFormsTestApplicationApp application)
        {
            var appsFolderEnvVar = ConfigurationManager.AppSettings.Get("AppsFolderEnvVar");
            var appsFolder = Environment.GetEnvironmentVariable(appsFolderEnvVar);
            var appPath = appsFolder + ConfigurationManager.AppSettings.Get("PathToExe");
            application = new WindowsFormsTestApplicationApp(appPath);
            application.Start();
        }

        public static void Cleanup(WindowsFormsTestApplicationApp application)
        {
            Assert.IsTrue(application.Close(), "Не удалось завершить приложение WindowsFormsTestApplication.");
        }
    }
}
