namespace WpfTestApplication.Tests.Map
{
    #region using

    using Cruciatus.Core;
    using Cruciatus.Elements;
    using Cruciatus.Extensions;

    #endregion

    public class SecondRibbonTab : TabItem
    {
        public SecondRibbonTab(CruciatusElement parent, By selector)
            : base(parent, selector)
        {
        }

        public CheckBox RibbonCheckBox
        {
            get
            {
                return Get(By.Uid("RibbonCheckBox")).ToCheckBox();
            }
        }

        public CheckBox RibbonToggleButton
        {
            get
            {
                return Get(By.Uid("RibbonToggleButton")).ToCheckBox();
            }
        }
    }
}
