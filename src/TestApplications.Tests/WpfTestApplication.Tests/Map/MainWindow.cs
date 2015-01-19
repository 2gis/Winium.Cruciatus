namespace WpfTestApplication.Tests.Map
{
    #region using

    using Cruciatus.Core;
    using Cruciatus.Elements;
    using Cruciatus.Extensions;

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
                return new FirstTab(this, By.Uid("TabItem1"));
            }
        }

        public SecondTab TabItem2
        {
            get
            {
                return new SecondTab(this, By.Uid("TabItem2"));
            }
        }

        public ThirdTab TabItem3
        {
            get
            {
                return new ThirdTab(this, By.Uid("TabItem3"));
            }
        }

        public FirstRibbonTab RibbonTabItem1
        {
            get
            {
                return new FirstRibbonTab(this, By.Uid("RibbonTabItem1"));
            }
        }

        public SecondRibbonTab RibbonTabItem2
        {
            get
            {
                return new SecondRibbonTab(this, By.Uid("RibbonTabItem2"));
            }
        }

        public RibbonApplicationMenu RibbonMenu
        {
            get
            {
                return new RibbonApplicationMenu(this, By.Uid("RibbonMenu"));
            }
        }

        public Menu SimpleMenu
        {
            get
            {
                return Get(By.Uid("SimpleMenu")).ToMenu();
            }
        }

        public Menu SetTextButtonContextMenu
        {
            get
            {
                return new Menu(this, By.Uid("SetTextButtonContextMenu"));
            }
        }

        public OpenFileDialog OpenFileDialog
        {
            get
            {
                return new OpenFileDialog(this, By.Name("Открытие"));
            }
        }

        public SaveFileDialog SaveFileDialog
        {
            get
            {
                return new SaveFileDialog(this, By.Name("Сохранение"));
            }
        }
    }
}
