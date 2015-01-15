// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenFileDialog.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет класс для работы с диалоговым окном Microsoft.Win32.OpenFileDialog.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus
{
    #region using

    using System.Windows.Automation;

    using Cruciatus.Core;
    using Cruciatus.Elements;
    using Cruciatus.Extensions;

    #endregion

    /// <summary>
    /// Класс для работы с диалоговым окном Microsoft.Win32.OpenFileDialog.
    /// </summary>
    public class OpenFileDialog
    {
        private readonly CruciatusElement _parent;

        private readonly By _selector;

        private CruciatusElement _instance;

        public OpenFileDialog(CruciatusElement parent)
        {
            _parent = parent;
            _selector = By.AutomationProperty(TreeScope.Subtree, WindowPattern.IsModalProperty, true);
        }

        private CruciatusElement Instance
        {
            get
            {
                return _instance ?? (_instance = _parent.Get(_selector));
            }
        }

        /// <summary>
        /// Возвращает кнопку Открыть.
        /// </summary>
        public CruciatusElement GetOpenButton()
        {
            var uid = CruciatusFactory.Settings.OpenFileDialogUid.OpenButton;
            return Instance.Get(By.Uid(TreeScope.Children, uid));
        }

        /// <summary>
        /// Возвращает кнопку Отмена.
        /// </summary>
        public CruciatusElement GetCancelButton()
        {
            var uid = CruciatusFactory.Settings.OpenFileDialogUid.CancelButton;
            return Instance.Get(By.Uid(TreeScope.Children, uid));
        }

        /// <summary>
        /// Возвращает редактируемый выпадающий список с именем открываемого файла.
        /// </summary>
        public ComboBox GetFileNameEditableComboBox()
        {
            var uid = CruciatusFactory.Settings.OpenFileDialogUid.FileNameEditableComboBox;
            return Instance.Get(By.Uid(TreeScope.Children, uid)).ToComboBox();
        }
    }
}
