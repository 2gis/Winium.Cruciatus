namespace WpfTestApplication.Tests.Map
{
    #region using

    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    #endregion

    public class FirstRibbonTab : TabItem
    {
        public Button RibbonButton
        {
            get
            {
                return GetElement<Button>("RibbonButton");
            }
        }

        public ComboBox RibbonTextComboBox
        {
            get
            {
                return GetElement<ComboBox>("RibbonTextComboBox");
            }
        }

        public ComboBox RibbonCheckComboBox
        {
            get
            {
                return GetElement<ComboBox>("RibbonCheckComboBox");
            }
        }

        protected override T GetElement<T>(string automationId)
        {
            var element = base.GetElement<T>(automationId);
            Assert.IsNotNull(element, LastErrorMessage);
            return element;
        }
    }
}
