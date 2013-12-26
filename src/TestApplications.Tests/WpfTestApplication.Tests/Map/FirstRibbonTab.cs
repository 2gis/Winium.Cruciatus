
namespace WpfTestApplication.Tests.Map
{
    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class FirstRibbonTab : TabItem
    {
        public Button RibbonButton
        {
            get
            {
                return this.GetElement<Button>("RibbonButton");
            }
        }

        public ComboBox RibbonTextComboBox
        {
            get
            {
                return this.GetElement<ComboBox>("RibbonTextComboBox");
            }
        }

        public ComboBox RibbonCheckComboBox
        {
            get
            {
                return this.GetElement<ComboBox>("RibbonCheckComboBox");
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
