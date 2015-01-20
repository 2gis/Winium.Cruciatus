namespace WpfTestApplication.Tests.Map
{
    #region using

    using Cruciatus.Core;
    using Cruciatus.Elements;
    using Cruciatus.Extensions;

    #endregion

    public class FirstRibbonTab : TabItem
    {
        public FirstRibbonTab(CruciatusElement parent, By getStrategy)
            : base(parent, getStrategy)
        {
        }

        public CruciatusElement RibbonButton
        {
            get
            {
                return GetByUid("RibbonButton");
            }
        }

        public ComboBox RibbonTextComboBox
        {
            get
            {
                return GetByUid("RibbonTextComboBox").ToComboBox();
            }
        }

        public ComboBox RibbonCheckComboBox
        {
            get
            {
                return GetByUid("RibbonCheckComboBox").ToComboBox();
            }
        }
    }
}
