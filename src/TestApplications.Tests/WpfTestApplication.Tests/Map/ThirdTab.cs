namespace WpfTestApplication.Tests.Map
{
    #region using

    using Winium.Cruciatus.Core;
    using Winium.Cruciatus.Elements;

    #endregion

    public class ThirdTab : TabItem
    {
        #region Constructors and Destructors

        public ThirdTab(CruciatusElement parent, By getStrategy)
            : base(parent, getStrategy)
        {
        }

        #endregion

        #region Public Properties

        public CruciatusElement OpenFileDialogButton
        {
            get
            {
                return this.FindElementByUid("OpenFileDialogButton");
            }
        }

        public CruciatusElement SaveFileDialogButton
        {
            get
            {
                return this.FindElementByUid("SaveFileDialogButton");
            }
        }

        #endregion
    }
}
