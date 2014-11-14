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

    using System;
    using System.Windows.Automation;

    using Cruciatus.Elements;

    #endregion

    /// <summary>
    /// Класс для работы с диалоговым окном Microsoft.Win32.OpenFileDialog.
    /// </summary>
    public static class OpenFileDialog
    {
        /// <summary>
        /// Возвращает кнопку Открыть.
        /// </summary>
        /// <param name="parent">
        /// Родительский элемент диалогового окна.
        /// </param>
        public static Button GetOpenButton(CruciatusElement parent)
        {
            var dialog = GetInstance(parent);
            var uid = CruciatusFactory.Settings.OpenFileDialogUid.OpenButton;
            var buttonInstance = CruciatusFactory.Find(dialog, uid, TreeScope.Children);
            return new Button
            {
                Parent = dialog,
                AutomationId = uid,
                ElementInstance = buttonInstance
            };
        }

        /// <summary>
        /// Возвращает кнопку Отмена.
        /// </summary>
        /// <param name="parent">
        /// Родительский элемент диалогового окна.
        /// </param>
        public static Button GetCancelButton(CruciatusElement parent)
        {
            var dialog = GetInstance(parent);
            var uid = CruciatusFactory.Settings.OpenFileDialogUid.CancelButton;
            var buttonInstance = CruciatusFactory.Find(dialog, uid, TreeScope.Children);
            return new Button
            {
                Parent = dialog,
                AutomationId = uid,
                ElementInstance = buttonInstance
            };
        }

        /// <summary>
        /// Возвращает редактируемый выпадающий список с именем открываемого файла.
        /// </summary>
        /// <param name="parent">
        /// Родительский элемент диалогового окна.
        /// </param>
        public static EditableComboBox GetFileNameEditableComboBox(CruciatusElement parent)
        {
            var dialog = GetInstance(parent);
            var uid = CruciatusFactory.Settings.OpenFileDialogUid.FileNameComboBox;
            var buttonInstance = CruciatusFactory.Find(dialog, uid, TreeScope.Children);
            return new EditableComboBox
            {
                Parent = dialog,
                AutomationId = uid,
                ElementInstance = buttonInstance
            };
        }

        private static AutomationElement GetInstance(CruciatusElement parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            var condition = new PropertyCondition(WindowPattern.IsModalProperty, true);
            var instance = CruciatusFactory.WaitingValues(() => parent.Element.FindFirst(TreeScope.Subtree, condition),
                                                          value => value == null,
                                                          CruciatusFactory.Settings.SearchTimeout);
            return instance;
        }
    }
}
