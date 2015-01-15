// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenFileDialog.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет класс для работы с диалоговым окном Microsoft.Win32.OpenFileDialog.
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
    /// Класс для работы с диалоговым окном Microsoft.Win32.OpenFileDialog.
    /// </summary>
    public class OpenFileDialog : CruciatusElement
    {
        public OpenFileDialog(CruciatusElement element)
            : base(element)
        {
        }

        public OpenFileDialog(CruciatusElement parent, By selector)
            : base(parent, selector)
        {
        }

        /// <summary>
        /// Возвращает кнопку Открыть.
        /// </summary>
        public CruciatusElement OpenButton
        {
            get
            {
                var uid = CruciatusFactory.Settings.OpenFileDialogUid.OpenButton;
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
                var uid = CruciatusFactory.Settings.OpenFileDialogUid.CancelButton;
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
                var uid = CruciatusFactory.Settings.OpenFileDialogUid.FileNameEditableComboBox;
                return Get(By.Uid(TreeScope.Children, uid)).ToComboBox();
            }
        }
    }
}
