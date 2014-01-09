
namespace Cruciatus
{
    using System.Windows;
    using System.Windows.Automation;

    using Cruciatus.Elements;

    using Window = Cruciatus.Elements.Window;

    internal static class MessageBox
    {
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
                uid = "Close";
            }
            else
            {
                switch (buttonsType)
                {
                    case MessageBoxButton.OK:
                        switch (button)
                        {
                            case MessageBoxResult.OK:
                                uid = "2";
                                break;
                            default:
                                return false;
                        }

                        break;

                    case MessageBoxButton.OKCancel:
                        switch (button)
                        {
                            case MessageBoxResult.OK:
                                uid = "1";
                                break;
                            case MessageBoxResult.Cancel:
                                uid = "2";
                                break;
                            default:
                                return false;
                        }

                        break;

                    case MessageBoxButton.YesNo:
                        switch (button)
                        {
                            case MessageBoxResult.Yes:
                                uid = "6";
                                break;
                            case MessageBoxResult.No:
                                uid = "7";
                                break;
                            default:
                                return false;
                        }

                        break;

                    case MessageBoxButton.YesNoCancel:
                        switch (button)
                        {
                            case MessageBoxResult.Yes:
                                uid = "6";
                                break;
                            case MessageBoxResult.No:
                                uid = "7";
                                break;
                            case MessageBoxResult.Cancel:
                                uid = "2";
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
