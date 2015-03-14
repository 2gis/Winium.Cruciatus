namespace Winium.Cruciatus.Core
{
    #region using

    using System;

    #endregion

    /// <summary>
    /// Перечисление поддерживаемых стратегий клика. Имеет атрибут Flags.
    /// </summary>
    [Flags]
    public enum ClickStrategies
    {
        /// <summary>
        /// Отсутствие определенной стратегии.
        /// </summary>
        None = 0, 

        /// <summary>
        /// Стратегия использования свойства элемента ClickablePoint.
        /// </summary>
        ClickablePoint = 1, 

        /// <summary>
        /// Стратегия использования свойства элемента BoundingRectangle.
        /// </summary>
        BoundingRectangleCenter = 2, 

        /// <summary>
        /// Стратегия использования интерфейса InvokePattern.
        /// </summary>
        InvokePattern = 4
    }
}
