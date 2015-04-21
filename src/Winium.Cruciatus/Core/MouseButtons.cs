namespace Winium.Cruciatus.Core
{
    #region using

    using System.Diagnostics.CodeAnalysis;

    #endregion

    /// <summary>
    /// Перечисление поддерживаемых кнопок мыши.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags", Justification = "Reviewed.")]
    public enum MouseButton
    {
        /// <summary>
        /// Левая кнопка мыши.
        /// </summary>
        Left = 0, 

        /// <summary>
        /// Правая кнопка мыши.
        /// </summary>
        Right = 2
    }
}
