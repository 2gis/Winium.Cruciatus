
namespace WpfTestApplication.Map
{
    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class SecondRibbonTab : TabItem
    {
        public CheckBox RibbonCheckBox
        {
            get
            {
                return this.GetElement<CheckBox>("RibbonCheckBox");
            }
        }

        public CheckBox RibbonToggleButton
        {
            get
            {
                return this.GetElement<CheckBox>("RibbonToggleButton");
            }
        }

        protected override T GetElement<T>(string automationId)
        {
            var element = base.GetElement<T>(automationId);
            Assert.IsNotNull(element, this.LastErrorMessage);
            return element;
        }
    }
}
