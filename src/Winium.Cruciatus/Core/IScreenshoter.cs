namespace Winium.Cruciatus.Core
{
    #region using

    using System.Diagnostics.CodeAnalysis;

    #endregion

    /// <summary>
    /// Интерфейс снимателя скриншотов.
    /// </summary>
    public interface IScreenshoter
    {
        #region Public Methods and Operators

        /// <summary>
        /// Возвращает скриншот рабочего стола.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Reviewed.")]
        Screenshot GetScreenshot();

        #endregion
    }
}
