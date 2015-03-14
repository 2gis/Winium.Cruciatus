namespace Winium.Cruciatus.Elements
{
    #region using

    using System.Windows.Automation;

    using Winium.Cruciatus.Core;
    using Winium.Cruciatus.Extensions;

    #endregion

    /// <summary>
    /// Класс для работы с диалоговым окном Microsoft.Win32.OpenFileDialog.
    /// </summary>
    public class OpenFileDialog : CruciatusElement
    {
        #region Constructors and Destructors

        /// <summary>
        /// Конструктор класса по объекту диалогового окна.
        /// </summary>
        /// <param name="element">
        /// Исходный элемент.
        /// </param>
        public OpenFileDialog(CruciatusElement element)
            : base(element)
        {
        }

        /// <summary>
        /// Конструторк класса.  Поиск осуществится только при необходимости.
        /// </summary>
        /// <param name="parent">
        /// Родительский элемент.
        /// </param>
        /// <param name="getStrategy">
        /// Стретегия поиска.
        /// </param>
        public OpenFileDialog(CruciatusElement parent, By getStrategy)
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
                var uid = CruciatusFactory.Settings.OpenFileDialogUid.CancelButton;
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
                var uid = CruciatusFactory.Settings.OpenFileDialogUid.FileNameEditableComboBox;
                return this.FindElement(By.Uid(TreeScope.Children, uid)).ToComboBox();
            }
        }

        /// <summary>
        /// Возвращает кнопку Открыть.
        /// </summary>
        public CruciatusElement OpenButton
        {
            get
            {
                var uid = CruciatusFactory.Settings.OpenFileDialogUid.OpenButton;
                return this.FindElement(By.Uid(TreeScope.Children, uid));
            }
        }

        #endregion
    }
}
