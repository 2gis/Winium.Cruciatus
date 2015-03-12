namespace WindowsFormsTestApplication.Tests
{
    #region using

    using System;
    using System.Configuration;

    using NUnit.Framework;

    using WindowsFormsTestApplication.Tests.Map;

    #endregion

    public static class TestClassHelper
    {
        #region Public Methods and Operators

        public static void Cleanup(WindowsFormsTestApplicationApp application)
        {
            Assert.IsTrue(application.Close(), "Не удалось завершить приложение WindowsFormsTestApplication.");
        }

        public static void Initialize(out WindowsFormsTestApplicationApp application)
        {
            var appsFolderEnvVar = ConfigurationManager.AppSettings.Get("AppsFolderEnvVar");
            var appsFolder = Environment.GetEnvironmentVariable(appsFolderEnvVar);
            var appPath = appsFolder + ConfigurationManager.AppSettings.Get("PathToExe");
            application = new WindowsFormsTestApplicationApp(appPath);
            application.Start();
        }

        #endregion
    }
}
