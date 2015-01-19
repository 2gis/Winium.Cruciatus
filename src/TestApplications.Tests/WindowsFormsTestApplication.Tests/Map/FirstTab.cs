namespace WindowsFormsTestApplication.Tests.Map
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
                return Get(By.Uid("SetTextButton"));
            }
        }

        public CruciatusElement TextBox1
        {
            get
            {
                return Get(By.Uid("TextBox1"));
            }
        }

        public ComboBox TextComboBox
        {
            get
            {
                return Get(By.Uid("TextComboBox")).ToComboBox();
            }
        }

        public CheckBox CheckBox1
        {
            get
            {
                return Get(By.Uid("CheckBox1")).ToCheckBox();
            }
        }

        public ListBox TextListBox
        {
            get
            {
                return Get(By.Uid("TextListBox")).ToListBox();
            }
        }
    }
}
