namespace Winium.Cruciatus.Elements
{
    #region using

    using System.Windows.Automation;

    using Winium.Cruciatus.Core;
    using Winium.Cruciatus.Extensions;

    #endregion

    /// <summary>
    /// Класс для работы с диалоговым окном Microsoft.Win32.SaveFileDialog.
    /// </summary>
    public class SaveFileDialog : CruciatusElement
    {
        #region Constructors and Destructors

        /// <summary>
        /// Создает экземпляр диалогового окна.
        /// </summary>
        /// <param name="element">
        /// Исходный элемент.
        /// </param>
        public SaveFileDialog(CruciatusElement element)
            : base(element)
        {
        }

        /// <summary>
        /// Создает экземпляр диалогового окна. Поиск осуществится только при необходимости.
        /// </summary>
        /// <param name="parent">
        /// Родительский элемент.
        /// </param>
        /// <param name="getStrategy">
        /// Стратегия поиска элемента.
        /// </param>
        public SaveFileDialog(CruciatusElement parent, By getStrategy)
            : base(parent, getStrategy)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Возвращает кнопку Отмена.
        /// </summary>
        public CruciatusElement CancelButton
        {
            get
            {
                var uid = CruciatusFactory.Settings.SaveFileDialogUid.CancelButton;
                return this.FindElement(By.Uid(TreeScope.Children, uid));
            }
        }

        /// <summary>
        /// Возвращает выпадающий список с именем открываемого файла.
        /// </summary>
        public ComboBox FileNameComboBox
        {
            get
            {
                var uid = CruciatusFactory.Settings.SaveFileDialogUid.FileNameEditableComboBox;
                return this.FindElement(By.Uid(TreeScope.Subtree, uid)).ToComboBox();
            }
        }

        /// <summary>
        /// Возвращает выпадающий список с типом открываемого файла.
        /// </summary>
        public ComboBox FileTypeComboBox
        {
            get
            {
                var uid = CruciatusFactory.Settings.SaveFileDialogUid.FileTypeComboBox;
                return this.FindElement(By.Uid(TreeScope.Subtree, uid)).ToComboBox();
            }
        }

        /// <summary>
        /// Возвращает кнопку Сохранить.
        /// </summary>
        public CruciatusElement SaveButton
        {
            get
            {
                var uid = CruciatusFactory.Settings.SaveFileDialogUid.SaveButton;
                return this.FindElement(By.Uid(TreeScope.Children, uid));
            }
        }

        #endregion
    }
}
