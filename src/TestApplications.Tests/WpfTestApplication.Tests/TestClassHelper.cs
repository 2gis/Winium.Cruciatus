namespace WpfTestApplication.Tests
{
    #region using

    using System;
    using System.Configuration;

    using NUnit.Framework;

    using WpfTestApplication.Tests.Map;

    #endregion

    public static class TestClassHelper
    {
        #region Public Methods and Operators

        public static void Cleanup(WpfTestApplicationApp application)
        {
            var isClose = application.Close();
            if (!isClose)
            {
                Assert.IsTrue(application.Kill(), "Не удалось завершить и убить приложение WpfTestApplication.");
            }
        }

        public static void Initialize(out WpfTestApplicationApp application)
        {
            var appsFolderEnvVar = ConfigurationManager.AppSettings.Get("AppsFolderEnvVar");
            var appsFolder = Environment.GetEnvironmentVariable(appsFolderEnvVar);
            var appPath = appsFolder + ConfigurationManager.AppSettings.Get("PathToExe");
            application = new WpfTestApplicationApp(appPath);
            application.Start();
        }

        #endregion
    }
}
