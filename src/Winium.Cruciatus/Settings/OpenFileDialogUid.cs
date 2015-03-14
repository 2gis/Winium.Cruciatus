namespace Winium.Cruciatus.Settings
{
    #region using

    using System;

    #endregion

    /// <summary>
    /// Класс, описывающий уникальные идентификаторы элементов диалога OpenFileDialog.
    /// </summary>
    public class OpenFileDialogUid : ICloneable
    {
        #region Public Properties

        /// <summary>
        /// Кнопка Отмена.
        /// </summary>
        public string CancelButton { get; set; }

        /// <summary>
        /// Редактируемый выпадающий список с именем открываемого файла.
        /// </summary>
        public string FileNameEditableComboBox { get; set; }

        /// <summary>
        /// Кнопка Открыть.
        /// </summary>
        public string OpenButton { get; set; }

        #endregion

        #region Public Methods and Operators

        public object Clone()
        {
            return new OpenFileDialogUid
                       {
                           OpenButton = this.OpenButton, 
                           CancelButton = this.CancelButton, 
                           FileNameEditableComboBox = this.FileNameEditableComboBox
                       };
        }

        #endregion
    }
}
