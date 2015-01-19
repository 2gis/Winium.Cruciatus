namespace WpfTestApplication.Tests.Map
{
    #region using

    using Cruciatus.Core;
    using Cruciatus.Elements;
    using Cruciatus.Extensions;

    #endregion

    public class SecondTab : TabItem
    {
        public SecondTab(CruciatusElement parent, By getStrategy)
            : base(parent, getStrategy)
        {
        }

        public CruciatusElement ChangeEnabledButton
        {
            get
            {
                return Get(By.Uid("ChangeEnabledButton"));
            }
        }

        public CruciatusElement TextBox2
        {
            get
            {
                return Get(By.Uid("TextBox2"));
            }
        }

        public ComboBox CheckComboBox
        {
            get
            {
                return Get(By.Uid("CheckComboBox")).ToComboBox();
            }
        }

        public CheckBox CheckBox2
        {
            get
            {
                return Get(By.Uid("CheckBox2")).ToCheckBox();
            }
        }

        public ListBox CheckListBox
        {
            get
            {
                return Get(By.Uid("CheckListBox")).ToListBox();
            }
        }
    }
}
