namespace Winium.Cruciatus.Core
{
    #region using

    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Windows;

    using Point = System.Drawing.Point;

    #endregion

    /// <summary>
    /// Класс для создания скриншотов рабочего стола.
    /// </summary>
    public class Screenshoter : IScreenshoter
    {
        #region Public Methods and Operators

        /// <summary>
        /// Возвращает скриншот рабочего стола.
        /// </summary>
        public Screenshot GetScreenshot()
        {
            byte[] imageBytes;
            var rect = new Rectangle(
                (int)SystemParameters.VirtualScreenLeft, 
                (int)SystemParameters.VirtualScreenTop, 
                (int)SystemParameters.VirtualScreenWidth, 
                (int)SystemParameters.VirtualScreenHeight);
            using (var bitmap = new Bitmap(rect.Width, rect.Height))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(new Point(rect.Left, rect.Top), Point.Empty, rect.Size);
                }

                using (var stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Png);
                    imageBytes = stream.ToArray();
                }
            }

            return new Screenshot(imageBytes);
        }

        #endregion
    }
}
