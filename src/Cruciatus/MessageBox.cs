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
    using System.Linq;
    using System.Windows;
    using System.Windows.Automation;

    using Cruciatus.Core;
    using Cruciatus.Elements;
    using Cruciatus.Exceptions;

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
            return AutomationElementHelper.FindAll(parent.Instanse, TreeScope.Children, condition, 60).Count();
        }

        public static void ClickButton(CruciatusElement parent, MessageBoxButton buttonsType, MessageBoxResult button)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            var condition = new PropertyCondition(WindowPattern.IsModalProperty, true);
            var modalwindow = AutomationElementHelper.FindFirst(parent.Instanse, TreeScope.Children, condition);
            if (modalwindow == null)
            {
                throw new CruciatusException("NOT CLICK BUTTON");
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
                                throw new CruciatusException("NOT CLICK BUTTON");
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
                                throw new CruciatusException("NOT CLICK BUTTON");
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
                                throw new CruciatusException("NOT CLICK BUTTON");
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
                                throw new CruciatusException("NOT CLICK BUTTON");
                        }

                        break;

                    default:
                        throw new CruciatusException("NOT CLICK BUTTON");
                }
            }

            var buttonElement = new CruciatusElement(parent.Instanse, modalwindow, null).Get(By.Uid(uid));
            buttonElement.Click();
        }
    }
}
