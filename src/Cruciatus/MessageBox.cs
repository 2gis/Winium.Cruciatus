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
    using System.Windows;
    using System.Windows.Automation;

    using Cruciatus.Elements;

    using Window = Cruciatus.Elements.Window;

    public static class MessageBox
    {
        #region Структуры для описания UID кнопок
        public struct OkType
        {
            public string OkUid;
        }

        public struct OkCancelType
        {
            public string OkUid;

            public string CancelUid;
        }

        public struct YesNoType
        {
            public string YesUid;

            public string NoUid;
        }

        public struct YesNoCancelType
        {
            public string YesUid;

            public string NoUid;

            public string CancelUid;
        }

        public struct ButtonUid
        {
            public string CloseButtonUid;

            public OkType OkType;

            public OkCancelType OkCancelType;

            public YesNoType YesNoType;

            public YesNoCancelType YesNoCancelType;
        }
        #endregion

        internal static int NumberOfOpenModalWindow(Window window)
        {
            var condition = new PropertyCondition(WindowPattern.IsModalProperty, true);
            return window.Element.FindAll(TreeScope.Children, condition).Count;
        }

        internal static bool ClickButton(Window window, MessageBoxButton buttonsType, MessageBoxResult button)
        {
            var condition = new PropertyCondition(WindowPattern.IsModalProperty, true);
            var modalwindow = window.Element.FindFirst(TreeScope.Children, condition);
            if (modalwindow == null)
            {
                return false;
            }

            string uid;
            if (button == MessageBoxResult.None)
            {
                uid = CruciatusFactory.Settings.MessageBoxButtonUid.CloseButtonUid;
            }
            else
            {
                switch (buttonsType)
                {
                    case MessageBoxButton.OK:
                        switch (button)
                        {
                            case MessageBoxResult.OK:
                                uid = CruciatusFactory.Settings.MessageBoxButtonUid.OkType.OkUid;
                                break;
                            default:
                                return false;
                        }

                        break;

                    case MessageBoxButton.OKCancel:
                        switch (button)
                        {
                            case MessageBoxResult.OK:
                                uid = CruciatusFactory.Settings.MessageBoxButtonUid.OkCancelType.OkUid;
                                break;
                            case MessageBoxResult.Cancel:
                                uid = CruciatusFactory.Settings.MessageBoxButtonUid.OkCancelType.CancelUid;
                                break;
                            default:
                                return false;
                        }

                        break;

                    case MessageBoxButton.YesNo:
                        switch (button)
                        {
                            case MessageBoxResult.Yes:
                                uid = CruciatusFactory.Settings.MessageBoxButtonUid.YesNoType.YesUid;
                                break;
                            case MessageBoxResult.No:
                                uid = CruciatusFactory.Settings.MessageBoxButtonUid.YesNoType.NoUid;
                                break;
                            default:
                                return false;
                        }

                        break;

                    case MessageBoxButton.YesNoCancel:
                        switch (button)
                        {
                            case MessageBoxResult.Yes:
                                uid = CruciatusFactory.Settings.MessageBoxButtonUid.YesNoCancelType.YesUid;
                                break;
                            case MessageBoxResult.No:
                                uid = CruciatusFactory.Settings.MessageBoxButtonUid.YesNoCancelType.NoUid;
                                break;
                            case MessageBoxResult.Cancel:
                                uid = CruciatusFactory.Settings.MessageBoxButtonUid.YesNoCancelType.CancelUid;
                                break;
                            default:
                                return false;
                        }

                        break;

                    default:
                        return false;
                }
            }

            var buttonElement = new Button(modalwindow, uid);
            return buttonElement.Click();
        }
    }
}
