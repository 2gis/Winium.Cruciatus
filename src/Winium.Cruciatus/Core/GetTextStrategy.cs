namespace Winium.Cruciatus.Core
{
    #region using

    using System;

    #endregion

    /// <summary>
    /// Перечисление поддерживаемых стратегий получения текста. Имеет атрибут Flags.
    /// </summary>
    [Flags]
    public enum GetTextStrategies
    {
        /// <summary>
        /// Отсутствие определенной стратегии.
        /// </summary>
        None = 0, 

        /// <summary>
        /// Стратегия использования интерфейса TextPattern.
        /// </summary>
        TextPattern = 1, 

        /// <summary>
        /// Стратегия использования интерфейса ValuePattern.
        /// </summary>
        ValuePattern = 2
    }
}
