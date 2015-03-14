namespace Winium.Cruciatus.Settings.MessageBoxSettings
{
    #region using

    using System;

    #endregion

    /// <summary>
    /// Класс описывающий набор из 2 кнопок - Ок и Отмена.
    /// </summary>
    public class OkCancelType : ICloneable
    {
        #region Public Properties

        /// <summary>
        /// Уникальный идентификатор кнопки Отмена.
        /// </summary>
        public string Cancel { get; set; }

        /// <summary>
        /// Уникальный идентификатор кнопки Ок.
        /// </summary>
        public string Ok { get; set; }

        #endregion

        #region Public Methods and Operators

        public object Clone()
        {
            return new OkCancelType { Ok = this.Ok, Cancel = this.Cancel };
        }

        #endregion
    }
}
