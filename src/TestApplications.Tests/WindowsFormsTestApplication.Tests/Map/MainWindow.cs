namespace WindowsFormsTestApplication.Tests.Map
{
    #region using

    using Cruciatus.Core;
    using Cruciatus.Elements;

    #endregion

    public class MainWindow : CruciatusElement
    {
        public MainWindow(CruciatusElement parent, By getStrategy)
            : base(parent, getStrategy)
        {
        }

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

        /*#region Временно, пока проблемы с вкладками.

        public CruciatusElement SetTextButton
        {
            get
            {
                return new CruciatusElement(this, By.Uid("SetTextButton"));
            }
        }

        public CruciatusElement TextBox1
        {
            get
            {
                return new CruciatusElement(this, By.Uid("TextBox1"));
            }
        }

        public ComboBox TextComboBox
        {
            get
            {
                return new ComboBox(this, By.Uid("TextComboBox"));
            }
        }

        public CheckBox CheckBox1
        {
            get
            {
                return new CheckBox(this, By.Uid("CheckBox1"));
            }
        }

        public ListBox TextListBox
        {
            get
            {
                return new ListBox(this, By.Uid("TextListBox"));
            }
        }

        #endregion*/
    }
}
