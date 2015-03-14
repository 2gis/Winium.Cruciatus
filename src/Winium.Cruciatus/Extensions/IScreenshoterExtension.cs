namespace Winium.Cruciatus.Extensions
{
    #region using

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    using Winium.Cruciatus.Core;

    #endregion

    /// <summary>
    /// Набор расширений для объектов, реализующих интерфейс IScreenshoter.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class IScreenshoterExtension
    {
        #region Public Methods and Operators

        /// <summary>
        /// Снимает и сохраняет скриншот в случае когда флаг 
        /// CruciatusFactory.Settings.AutomaticScreenshotCapture равен true.
        /// </summary>
        public static void AutomaticScreenshotCaptureIfNeeded(this IScreenshoter screenshoter)
        {
            if (CruciatusFactory.Settings.AutomaticScreenshotCapture)
            {
                screenshoter.TakeScreenshot();
            }
        }

        /// <summary>
        /// Снимает и сохраняет скриншот.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Main argument in extension method cannot be null")]
        public static void TakeScreenshot(this IScreenshoter screenshoter)
        {
            var timeStamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff");
            var screenshotPath = Path.Combine(CruciatusFactory.Settings.ScreenshotsPath, timeStamp + ".png");
            screenshoter.GetScreenshot().SaveAsFile(screenshotPath);
            CruciatusFactory.Logger.Info("Saved screenshot to '{0}' file.", Path.GetFullPath(screenshotPath));
        }

        #endregion
    }
}
