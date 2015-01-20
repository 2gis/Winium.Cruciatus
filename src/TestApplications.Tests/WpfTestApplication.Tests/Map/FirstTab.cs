namespace WpfTestApplication.Tests.Map
{
    #region using

    using Cruciatus.Core;
    using Cruciatus.Elements;
    using Cruciatus.Extensions;

    #endregion

    public class FirstTab : TabItem
    {
        public FirstTab(CruciatusElement parent, By getStrategy)
            : base(parent, getStrategy)
        {
        }

        public CruciatusElement SetTextButton
        {
            get
            {
                return GetByUid("SetTextButton");
            }
        }

        public CruciatusElement TextBox1
        {
            get
            {
                return GetByUid("TextBox1");
            }
        }

        public ComboBox TextComboBox
        {
            get
            {
                return GetByUid("TextComboBox").ToComboBox();
            }
        }

        public CheckBox CheckBox1
        {
            get
            {
                return GetByUid("CheckBox1").ToCheckBox();
            }
        }

        public ListBox TextListBox
        {
            get
            {
                return GetByUid("TextListBox").ToListBox();
            }
        }
    }
}
