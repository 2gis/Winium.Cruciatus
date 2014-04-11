namespace WpfTestApplication.Tests.Map
{
    #region using

    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    #endregion

    public class SecondRibbonTab : TabItem
    {
        public CheckBox RibbonCheckBox
        {
            get
            {
                return GetElement<CheckBox>("RibbonCheckBox");
            }
        }

        public CheckBox RibbonToggleButton
        {
            get
            {
                return GetElement<CheckBox>("RibbonToggleButton");
            }
        }

        public override T GetElement<T>(string automationId)
        {
            var element = base.GetElement<T>(automationId);
            Assert.IsNotNull(element, LastErrorMessage);
            return element;
        }
    }
}
