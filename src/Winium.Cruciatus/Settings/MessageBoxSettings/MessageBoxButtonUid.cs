namespace Winium.Cruciatus.Settings.MessageBoxSettings
{
    #region using

    using System;

    #endregion

    /// <summary>
    /// Класс описывающий уникальные идентификаторы кнопок диалогового окна MessageBox.
    /// </summary>
    public class MessageBoxButtonUid : ICloneable
    {
        #region Public Properties

        /// <summary>
        /// Уникальный идентификатор кнопки Отмена.
        /// </summary>
        public string CloseButton { get; set; }

        /// <summary>
        /// Тип набор из 2 кнопок - Ок и Отмена.
        /// </summary>
        public OkCancelType OkCancelType { get; set; }

        /// <summary>
        /// Тип набор из 1 кнопки - Ок.
        /// </summary>
        public OkType OkType { get; set; }

        /// <summary>
        /// Тип набор из 2 кнопок - Да, Нет и Отмена.
        /// </summary>
        public YesNoCancelType YesNoCancelType { get; set; }

        /// <summary>
        /// Тип набор из 2 кнопок - Да и Нет.
        /// </summary>
        public YesNoType YesNoType { get; set; }

        #endregion

        #region Public Methods and Operators

        public object Clone()
        {
            return new MessageBoxButtonUid
                       {
                           CloseButton = this.CloseButton, 
                           OkType = (OkType)this.OkType.Clone(), 
                           OkCancelType = (OkCancelType)this.OkCancelType.Clone(), 
                           YesNoType = (YesNoType)this.YesNoType.Clone(), 
                           YesNoCancelType = (YesNoCancelType)this.YesNoCancelType.Clone()
                       };
        }

        #endregion
    }
}
