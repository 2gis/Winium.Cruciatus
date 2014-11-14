namespace Cruciatus.Settings
{
    #region using

    using System;

    #endregion

    /// <summary>
    /// Класс, описывающий уникальные идентификаторы элементов диалога OpenFileDialog.
    /// </summary>
    public class OpenFileDialogUid : ICloneable
    {
        /// <summary>
        /// Кнопка Открыть.
        /// </summary>
        public string OpenButton { get; set; }

        /// <summary>
        /// Кнопка Отмена.
        /// </summary>
        public string CancelButton { get; set; }

        /// <summary>
        /// Редактируемый выпадающий список с именем открываемого файла.
        /// </summary>
        public string FileNameEditableComboBox { get; set; }

        public object Clone()
        {
            return new OpenFileDialogUid
            {
                OpenButton = OpenButton,
                CancelButton = CancelButton,
                FileNameEditableComboBox = FileNameEditableComboBox
            };
        }
    }
}
