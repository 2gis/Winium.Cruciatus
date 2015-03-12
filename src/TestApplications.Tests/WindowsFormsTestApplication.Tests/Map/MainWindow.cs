namespace WindowsFormsTestApplication.Tests.Map
{
    #region using

    using Cruciatus.Core;
    using Cruciatus.Elements;

    #endregion

    public class MainWindow : CruciatusElement
    {
        #region Constructors and Destructors

        public MainWindow(CruciatusElement parent, By getStrategy)
            : base(parent, getStrategy)
        {
        }

        #endregion

        #region Public Properties

        public FirstTab TabItem1
        {
            get
            {
                return new FirstTab(this, By.Name("TabItem1"));
            }
        }

        public SecondTab TabItem2
        {
            get
            {
                return new SecondTab(this, By.Name("TabItem2"));
            }
        }

        #endregion
    }
}
