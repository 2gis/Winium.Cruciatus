namespace WpfTestApplication.Tests.Map
{
    #region using

    using Winium.Cruciatus.Core;
    using Winium.Cruciatus.Elements;
    using Winium.Cruciatus.Extensions;

    #endregion

    public class SecondTab : TabItem
    {
        #region Constructors and Destructors

        public SecondTab(CruciatusElement parent, By getStrategy)
            : base(parent, getStrategy)
        {
        }

        #endregion

        #region Public Properties

        public CruciatusElement ChangeEnabledButton
        {
            get
            {
                return this.FindElementByUid("ChangeEnabledButton");
            }
        }

        public CheckBox CheckBox2
        {
            get
            {
                return this.FindElementByUid("CheckBox2").ToCheckBox();
            }
        }

        public ComboBox CheckComboBox
        {
            get
            {
                return this.FindElementByUid("CheckComboBox").ToComboBox();
            }
        }

        public ListBox CheckListBox
        {
            get
            {
                return this.FindElementByUid("CheckListBox").ToListBox();
            }
        }

        public CruciatusElement TextBox2
        {
            get
            {
                return this.FindElementByUid("TextBox2");
            }
        }

        #endregion
    }
}
