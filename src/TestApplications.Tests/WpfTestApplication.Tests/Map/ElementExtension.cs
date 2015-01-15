namespace WpfTestApplication.Tests.Map
{
    #region using

    using System.Windows.Automation;

    using Cruciatus.Core;
    using Cruciatus.Elements;
    using Cruciatus.Extensions;

    #endregion

    public static class ElementExtension
    {
        public static CheckBox GetCheckBoxByName(this CruciatusElement element, string name)
        {
            return element.Get(By.Name(name).AndType(ControlType.CheckBox)).ToCheckBox();
        }

        public static CheckBox ScrollToCheckBoxByName(this ListBox element, string name)
        {
            return element.ScrollTo(By.Name(name).AndType(ControlType.CheckBox)).ToCheckBox();
        }
    }
}
