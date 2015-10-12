namespace Winium.Cruciatus
{
    #region using

    using System;
    using System.Windows;
    using System.Windows.Automation;

    using Winium.Cruciatus.Core;
    using Winium.Cruciatus.Elements;
    using Winium.Cruciatus.Exceptions;
    using Winium.Cruciatus.Helpers;

    #endregion

    /// <summary>
    /// Класс для работы с диалоговым окном MessageBox.
    /// </summary>
    public static class MessageBox
    {
        #region Public Methods and Operators

        /// <summary>
        /// Кликает по заданной кнопке диалогового окна.
        /// </summary>
        /// <param name="dialogWindow">
        /// Диалоговое окно.
        /// </param>
        /// <param name="buttonsType">
        /// Тип набора кнопок диалогово окна.
        /// </param>
        /// <param name="targetButton">
        /// Целевая кнопка.
        /// </param>
        public static void ClickButton(
            CruciatusElement dialogWindow, 
            MessageBoxButton buttonsType, 
            MessageBoxResult targetButton)
        {
            if (dialogWindow == null)
            {
                throw new ArgumentNullException("dialogWindow");
            }

            var condition = new PropertyCondition(WindowPattern.IsModalProperty, true);
            var modalwindow = AutomationElementHelper.FindFirst(dialogWindow.Instance, TreeScope.Children, condition);
            if (modalwindow == null)
            {
                throw new CruciatusException("NOT CLICK BUTTON");
            }

            string uid;
            if (targetButton == MessageBoxResult.None)
            {
                uid = CruciatusFactory.Settings.MessageBoxButtonUid.CloseButton;
            }
            else
            {
                switch (buttonsType)
                {
                    case MessageBoxButton.OK:
                        switch (targetButton)
                        {
                            case MessageBoxResult.OK:
                                uid = CruciatusFactory.Settings.MessageBoxButtonUid.OkType.Ok;
                                break;
                            default:
                                throw new CruciatusException("NOT CLICK BUTTON");
                        }

                        break;

                    case MessageBoxButton.OKCancel:
                        switch (targetButton)
                        {
                            case MessageBoxResult.OK:
                                uid = CruciatusFactory.Settings.MessageBoxButtonUid.OkCancelType.Ok;
                                break;
                            case MessageBoxResult.Cancel:
                                uid = CruciatusFactory.Settings.MessageBoxButtonUid.OkCancelType.Cancel;
                                break;
                            default:
                                throw new CruciatusException("NOT CLICK BUTTON");
                        }

                        break;

                    case MessageBoxButton.YesNo:
                        switch (targetButton)
                        {
                            case MessageBoxResult.Yes:
                                uid = CruciatusFactory.Settings.MessageBoxButtonUid.YesNoType.Yes;
                                break;
                            case MessageBoxResult.No:
                                uid = CruciatusFactory.Settings.MessageBoxButtonUid.YesNoType.No;
                                break;
                            default:
                                throw new CruciatusException("NOT CLICK BUTTON");
                        }

                        break;

                    case MessageBoxButton.YesNoCancel:
                        switch (targetButton)
                        {
                            case MessageBoxResult.Yes:
                                uid = CruciatusFactory.Settings.MessageBoxButtonUid.YesNoCancelType.Yes;
                                break;
                            case MessageBoxResult.No:
                                uid = CruciatusFactory.Settings.MessageBoxButtonUid.YesNoCancelType.No;
                                break;
                            case MessageBoxResult.Cancel:
                                uid = CruciatusFactory.Settings.MessageBoxButtonUid.YesNoCancelType.Cancel;
                                break;
                            default:
                                throw new CruciatusException("NOT CLICK BUTTON");
                        }

                        break;

                    default:
                        throw new CruciatusException("NOT CLICK BUTTON");
                }
            }

            var buttonElement = new CruciatusElement(dialogWindow, modalwindow, null).FindElement(By.Uid(uid));
            buttonElement.Click();
        }

        #endregion
    }
}
