namespace WpfTestApplication.Tests.Map
{
    #region using

    using Cruciatus.Core;
    using Cruciatus.Elements;
    using Cruciatus.Extensions;

    #endregion

    public class FirstTab : TabItem
    {
        #region Constructors and Destructors

        public FirstTab(CruciatusElement parent, By getStrategy)
            : base(parent, getStrategy)
        {
        }

        #endregion

        #region Public Properties

        public CheckBox CheckBox1
        {
            get
            {
                return this.FindElementByUid("CheckBox1").ToCheckBox();
            }
        }

        public CruciatusElement SetTextButton
        {
            get
            {
                return this.FindElementByUid("SetTextButton");
            }
        }

        public CruciatusElement TextBox1
        {
            get
            {
                return this.FindElementByUid("TextBox1");
            }
        }

        public ComboBox TextComboBox
        {
            get
            {
                return this.FindElementByUid("TextComboBox").ToComboBox();
            }
        }

        public ListBox TextListBox
        {
            get
            {
                return this.FindElementByUid("TextListBox").ToListBox();
            }
        }

        #endregion
    }
}
