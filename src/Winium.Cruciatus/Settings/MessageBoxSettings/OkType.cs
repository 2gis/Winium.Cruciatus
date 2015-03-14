namespace Winium.Cruciatus.Settings.MessageBoxSettings
{
    #region using

    using System;

    #endregion

    /// <summary>
    /// Класс описывающий набор из 1 кнопки - Ок.
    /// </summary>
    public class OkType : ICloneable
    {
        #region Public Properties

        /// <summary>
        /// Уникальный идентификатор кнопки Ок.
        /// </summary>
        public string Ok { get; set; }

        #endregion

        #region Public Methods and Operators

        public object Clone()
        {
            return new OkType { Ok = this.Ok };
        }

        #endregion
    }
}
