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
                return GetByUid("ChangeEnabledButton");
            }
        }

        public CruciatusElement TextBox2
        {
            get
            {
                return GetByUid("TextBox2");
            }
        }

        public ComboBox CheckComboBox
        {
            get
            {
                return GetByUid("CheckComboBox").ToComboBox();
            }
        }

        public CheckBox CheckBox2
        {
            get
            {
                return GetByUid("CheckBox2").ToCheckBox();
            }
        }

        public ListBox CheckListBox
        {
            get
            {
                return GetByUid("CheckListBox").ToListBox();
            }
        }
    }
}
