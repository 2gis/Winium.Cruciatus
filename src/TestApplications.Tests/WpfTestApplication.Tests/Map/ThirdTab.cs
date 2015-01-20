namespace WpfTestApplication.Tests.Map
{
    #region using

    using Cruciatus.Core;
    using Cruciatus.Elements;

    #endregion

    public class ThirdTab : TabItem
    {
        public ThirdTab(CruciatusElement parent, By getStrategy)
            : base(parent, getStrategy)
        {
        }

        public CruciatusElement OpenFileDialogButton
        {
            get
            {
                return GetByUid("OpenFileDialogButton");
            }
        }

        public CruciatusElement SaveFileDialogButton
        {
            get
            {
                return GetByUid("SaveFileDialogButton");
            }
        }
    }
}
