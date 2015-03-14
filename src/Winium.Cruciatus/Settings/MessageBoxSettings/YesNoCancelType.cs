namespace Winium.Cruciatus.Settings.MessageBoxSettings
{
    #region using

    using System;

    #endregion

    /// <summary>
    /// Класс описывающий набор из 3 кнопок - Да, Нет и Отмена.
    /// </summary>
    public class YesNoCancelType : ICloneable
    {
        #region Public Properties

        /// <summary>
        /// Уникальный идентификатор кнопки Отмена.
        /// </summary>
        public string Cancel { get; set; }

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
            return new YesNoCancelType { Yes = this.Yes, No = this.No, Cancel = this.Cancel };
        }

        #endregion
    }
}
