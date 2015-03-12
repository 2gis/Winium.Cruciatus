namespace WpfTestApplication.Tests.Map
{
    #region using

    using Cruciatus.Core;
    using Cruciatus.Elements;
    using Cruciatus.Extensions;

    #endregion

    public class SecondRibbonTab : TabItem
    {
        #region Constructors and Destructors

        public SecondRibbonTab(CruciatusElement parent, By getStrategy)
            : base(parent, getStrategy)
        {
        }

        #endregion

        #region Public Properties

        public CheckBox RibbonCheckBox
        {
            get
            {
                return this.FindElementByUid("RibbonCheckBox").ToCheckBox();
            }
        }

        public CheckBox RibbonToggleButton
        {
            get
            {
                return this.FindElementByUid("RibbonToggleButton").ToCheckBox();
            }
        }

        #endregion
    }
}
