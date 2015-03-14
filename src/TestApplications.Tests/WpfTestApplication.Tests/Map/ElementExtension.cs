namespace WpfTestApplication.Tests.Map
{
    #region using

    using System.Windows.Automation;

    using Winium.Cruciatus.Core;
    using Winium.Cruciatus.Elements;
    using Winium.Cruciatus.Extensions;

    #endregion

    public static class ElementExtension
    {
        #region Public Methods and Operators

        public static CheckBox GetCheckBoxByName(this CruciatusElement element, string name)
        {
            return element.FindElement(By.Name(name).AndType(ControlType.CheckBox)).ToCheckBox();
        }

        public static CheckBox ScrollToCheckBoxByName(this ListBox element, string name)
        {
            return element.ScrollTo(By.Name(name).AndType(ControlType.CheckBox)).ToCheckBox();
        }

        #endregion
    }
}
