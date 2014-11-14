namespace Cruciatus.Settings
{
    #region using

    using System;

    #endregion

    /// <summary>
    /// Класс, описывающий уникальные индетификаторы элементов диалога OpenFileDialog.
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
        public string FileNameComboBox { get; set; }

        public object Clone()
        {
            return new OpenFileDialogUid
            {
                OpenButton = OpenButton,
                CancelButton = CancelButton,
                FileNameComboBox = FileNameComboBox
            };
        }
    }
}
