// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveFileDialog.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет класс для работы с диалоговым окном Microsoft.Win32.SaveFileDialog.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus.Elements
{
    #region using

    using System.Windows.Automation;

    using Cruciatus.Core;
    using Cruciatus.Extensions;

    #endregion

    /// <summary>
    /// Класс для работы с диалоговым окном Microsoft.Win32.SaveFileDialog.
    /// </summary>
    public class SaveFileDialog : CruciatusElement
    {
        public SaveFileDialog(CruciatusElement element)
            : base(element)
        {
        }

        public SaveFileDialog(CruciatusElement parent, By selector)
            : base(parent, selector)
        {
        }

        /// <summary>
        /// Возвращает кнопку Сохранить.
        /// </summary>
        public CruciatusElement SaveButton
        {
            get
            {
                var uid = CruciatusFactory.Settings.SaveFileDialogUid.SaveButton;
                return Get(By.Uid(TreeScope.Children, uid));
            }
        }

        /// <summary>
        /// Возвращает кнопку Отмена.
        /// </summary>
        public CruciatusElement CancelButton
        {
            get
            {
                var uid = CruciatusFactory.Settings.SaveFileDialogUid.CancelButton;
                return Get(By.Uid(TreeScope.Children, uid));
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
                return Get(By.Uid(TreeScope.Subtree, uid)).ToComboBox();
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
                return Get(By.Uid(TreeScope.Subtree, uid)).ToComboBox();
            }
        }
    }
}
