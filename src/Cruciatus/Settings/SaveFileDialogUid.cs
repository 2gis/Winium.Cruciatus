namespace Cruciatus.Settings
{
    #region using

    using System;

    #endregion

    /// <summary>
    /// Класс, описывающий уникальные идентификаторы элементов диалога SaveFileDialog.
    /// </summary>
    public class SaveFileDialogUid : ICloneable
    {
        /// <summary>
        /// Кнопка Сохранить.
        /// </summary>
        public string SaveButton { get; set; }

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

        public object Clone()
        {
            return new SaveFileDialogUid
            {
                SaveButton = SaveButton,
                CancelButton = CancelButton,
                FileNameEditableComboBox = FileNameEditableComboBox,
                FileTypeComboBox = FileTypeComboBox
            };
        }
    }
}
