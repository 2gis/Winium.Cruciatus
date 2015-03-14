namespace Winium.Cruciatus.Core
{
    /// <summary>
    /// Перечисление поддерживаемых стратегий раскрытия выпадающего элемента.
    /// </summary>
    public enum ExpandStrategy
    {
        /// <summary>
        /// Стратегия раскрытия через клик по элементу.
        /// </summary>
        Click = 0, 

        /// <summary>
        /// Стратегия использования интерфейса ExpandCollapsePattern.
        /// </summary>
        ExpandCollapsePattern = 1
    }
}
