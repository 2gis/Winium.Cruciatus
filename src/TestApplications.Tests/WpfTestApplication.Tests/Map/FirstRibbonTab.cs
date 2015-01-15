namespace WpfTestApplication.Tests.Map
{
    #region using

    using Cruciatus.Core;
    using Cruciatus.Elements;
    using Cruciatus.Extensions;

    #endregion

    public class FirstRibbonTab : TabItem
    {
        public FirstRibbonTab(CruciatusElement parent, By selector)
            : base(parent, selector)
        {
        }

        public CruciatusElement RibbonButton
        {
            get
            {
                return Get(By.Uid("RibbonButton"));
            }
        }

        public ComboBox RibbonTextComboBox
        {
            get
            {
                return Get(By.Uid("RibbonTextComboBox")).ToComboBox();
            }
        }

        public ComboBox RibbonCheckComboBox
        {
            get
            {
                return Get(By.Uid("RibbonCheckComboBox")).ToComboBox();
            }
        }
    }
}
