namespace WpfTestApplication.Tests.Map
{
    #region using

    using Cruciatus.Core;
    using Cruciatus.Elements;

    #endregion

    public class ThirdTab : TabItem
    {
        public ThirdTab(CruciatusElement parent, By selector)
            : base(parent, selector)
        {
        }

        public CruciatusElement OpenFileDialogButton
        {
            get
            {
                return Get(By.Uid("OpenFileDialogButton"));
            }
        }

        public CruciatusElement SaveFileDialogButton
        {
            get
            {
                return Get(By.Uid("SaveFileDialogButton"));
            }
        }
    }
}
