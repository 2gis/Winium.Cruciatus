namespace Winium.Cruciatus.Settings
{
    #region using

    using System;

    #endregion

    /// <summary>
    /// Класс, описывающий уникальные идентификаторы элементов диалога SaveFileDialog.
    /// </summary>
    public class SaveFileDialogUid : ICloneable
    {
        #region Public Properties

        /// <summary>
        /// Кнопка Отмена.
        /// </summary>
        public string CancelButton { get; set; }

        /// <summary>
        /// Редактируемый выпадающий список с именем сохраняемого файла.
        /// </summary>
        public string FileNameEditableComboBox { get; set; }

        /// <summary>
        /// Выпадающий список с типом сохраняемого файла.
        /// </summary>
        public string FileTypeComboBox { get; set; }

        /// <summary>
        /// Кнопка Сохранить.
        /// </summary>
        public string SaveButton { get; set; }

        #endregion

        #region Public Methods and Operators

        public object Clone()
        {
            return new SaveFileDialogUid
                       {
                           SaveButton = this.SaveButton, 
                           CancelButton = this.CancelButton, 
                           FileNameEditableComboBox = this.FileNameEditableComboBox, 
                           FileTypeComboBox = this.FileTypeComboBox
                       };
        }

        #endregion
    }
}
