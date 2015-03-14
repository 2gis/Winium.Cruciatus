namespace Winium.Cruciatus.Settings.MessageBoxSettings
{
    #region using

    using System;

    #endregion

    /// <summary>
    /// Класс описывающий набор из 2 кнопок - Да и Нет.
    /// </summary>
    public class YesNoType : ICloneable
    {
        #region Public Properties

        /// <summary>
        /// Уникальный идентификатор кнопки Нет.
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// Уникальный идентификатор кнопки Да.
        /// </summary>
        public string Yes { get; set; }

        #endregion

        #region Public Methods and Operators

        public object Clone()
        {
            return new YesNoType { Yes = this.Yes, No = this.No };
        }

        #endregion
    }
}
