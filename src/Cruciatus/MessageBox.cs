// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageBox.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет класс для работы с диалогом MessageBox.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus
{
    #region using

    using System;
    using System.Windows;
    using System.Windows.Automation;

    using Cruciatus.Elements;

    #endregion

    public static class MessageBox
    {
        public static int NumberOfOpenModalWindow(CruciatusElement parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            var condition = new PropertyCondition(WindowPattern.IsModalProperty, true);
            return parent.Element.FindAll(TreeScope.Children, condition).Count;
        }

        public static bool ClickButton(CruciatusElement parent, MessageBoxButton buttonsType, MessageBoxResult button)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            var condition = new PropertyCondition(WindowPattern.IsModalProperty, true);
            var modalwindow = parent.Element.FindFirst(TreeScope.Children, condition);
            if (modalwindow == null)
            {
                return false;
            }

            string uid;
            if (button == MessageBoxResult.None)
            {
                uid = CruciatusFactory.Settings.MessageBoxButtonUid.CloseButton;
            }
            else
            {
                switch (buttonsType)
                {
                    case MessageBoxButton.OK:
                        switch (button)
                        {
                            case MessageBoxResult.OK:
                                uid = CruciatusFactory.Settings.MessageBoxButtonUid.OkType.Ok;
                                break;
                            default:
                                return false;
                        }

                        break;

                    case MessageBoxButton.OKCancel:
                        switch (button)
                        {
                            case MessageBoxResult.OK:
                                uid = CruciatusFactory.Settings.MessageBoxButtonUid.OkCancelType.Ok;
                                break;
                            case MessageBoxResult.Cancel:
                                uid = CruciatusFactory.Settings.MessageBoxButtonUid.OkCancelType.Cancel;
                                break;
                            default:
                                return false;
                        }

                        break;

                    case MessageBoxButton.YesNo:
                        switch (button)
                        {
                            case MessageBoxResult.Yes:
                                uid = CruciatusFactory.Settings.MessageBoxButtonUid.YesNoType.Yes;
                                break;
                            case MessageBoxResult.No:
                                uid = CruciatusFactory.Settings.MessageBoxButtonUid.YesNoType.No;
                                break;
                            default:
                                return false;
                        }

                        break;

                    case MessageBoxButton.YesNoCancel:
                        switch (button)
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
                                return false;
                        }

                        break;

                    default:
                        return false;
                }
            }

            var buttonElement = new Button { Parent = modalwindow, AutomationId = uid };
            return buttonElement.Click();
        }
    }
}
