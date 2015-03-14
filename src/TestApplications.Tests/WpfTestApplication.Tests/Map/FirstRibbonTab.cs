namespace WpfTestApplication.Tests.Map
{
    #region using

    using Winium.Cruciatus.Core;
    using Winium.Cruciatus.Elements;
    using Winium.Cruciatus.Extensions;

    #endregion

    public class FirstRibbonTab : TabItem
    {
        #region Constructors and Destructors

        public FirstRibbonTab(CruciatusElement parent, By getStrategy)
            : base(parent, getStrategy)
        {
        }

        #endregion

        #region Public Properties

        public CruciatusElement RibbonButton
        {
            get
            {
                return this.FindElementByUid("RibbonButton");
            }
        }

        public ComboBox RibbonCheckComboBox
        {
            get
            {
                return this.FindElementByUid("RibbonCheckComboBox").ToComboBox();
            }
        }

        public ComboBox RibbonTextComboBox
        {
            get
            {
                return this.FindElementByUid("RibbonTextComboBox").ToComboBox();
            }
        }

        #endregion
    }
}
