
namespace WpfTestApplication.Map
{
    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class MainWindow : Window
    {
        public FirstTab TabItem1
        {
            get
            {
                return this.GetElement<FirstTab>("TabItem1");
            }
        }

        public SecondTab TabItem2
        {
            get
            {
                return this.GetElement<SecondTab>("TabItem2");
            }
        }

        public FirstRibbonTab RibbonTabItem1
        {
            get
            {
                return this.GetElement<FirstRibbonTab>("RibbonTabItem1");
            }
        }

        public SecondRibbonTab RibbonTabItem2
        {
            get
            {
                return this.GetElement<SecondRibbonTab>("RibbonTabItem2");
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