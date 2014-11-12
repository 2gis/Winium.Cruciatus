namespace WpfTestApplication.Tests.Map
{
    #region using

    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    #endregion

    public class MainWindow : Window
    {
        public FirstTab TabItem1
        {
            get
            {
                return GetElement<FirstTab>("TabItem1");
            }
        }

        public SecondTab TabItem2
        {
            get
            {
                return GetElement<SecondTab>("TabItem2");
            }
        }

        public FirstRibbonTab RibbonTabItem1
        {
            get
            {
                return GetElement<FirstRibbonTab>("RibbonTabItem1");
            }
        }

        public SecondRibbonTab RibbonTabItem2
        {
            get
            {
                return GetElement<SecondRibbonTab>("RibbonTabItem2");
            }
        }

        public RibbonApplicationMenu RibbonMenu
        {
            get
            {
                return GetElement<RibbonApplicationMenu>("RibbonMenu");
            }
        }

        public Menu SimpleMenu
        {
            get
            {
                return GetElement<Menu>("SimpleMenu");
            }
        }

        public ContextMenu SetTextButtonContextMenu
        {
            get
            {
                return GetElement<ContextMenu>("SetTextButtonContextMenu");
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
