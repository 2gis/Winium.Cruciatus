namespace Winium.Cruciatus.Elements
{
    #region using

    using System.Threading;
    using System.Windows.Automation;

    using Winium.Cruciatus.Core;
    using Winium.Cruciatus.Exceptions;
    using Winium.Cruciatus.Extensions;

    #endregion

    /// <summary>
    /// Представляет элемент управления выпадающий список.
    /// </summary>
    public class ComboBox : CruciatusElement
    {
        #region Constructors and Destructors

        /// <summary>
        /// Создает экземпляр выпадающего списка.
        /// </summary>
        /// <param name="element">
        /// Исходный элемент.
        /// </param>
        public ComboBox(CruciatusElement element)
            : base(element)
        {
        }

        /// <summary>
        /// Создает экземпляр выпадающего списка. Поиск осуществится только при необходимости.
        /// </summary>
        /// <param name="parent">
        /// Родительский элемент.
        /// </param>
        /// <param name="getStrategy">
        /// Стратегия поиска элемента.
        /// </param>
        public ComboBox(CruciatusElement parent, By getStrategy)
            : base(parent, getStrategy)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Возвращает значение, указывающее, раскрыт ли выпадающий список.
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return this.ExpandCollapseState == ExpandCollapseState.Expanded;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Возвращает состояние раскрытости выпадающего списка.
        /// </summary>
        internal ExpandCollapseState ExpandCollapseState
        {
            get
            {
                return
                    this.GetAutomationPropertyValue<ExpandCollapseState>(
                        ExpandCollapsePattern.ExpandCollapseStateProperty);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Сворачивает выпадающий список кликом.
        /// </summary>
        public void Collapse()
        {
            this.Collapse(ExpandStrategy.Click);
        }

        /// <summary>
        /// Сворачивает выпадающий список.
        /// </summary>
        /// <param name="strategy">
        /// Стратегия способа раскрытия.
        /// </param>
        public void Collapse(ExpandStrategy strategy)
        {
            if (this.ExpandCollapseState == ExpandCollapseState.Collapsed)
            {
                return;
            }

            switch (strategy)
            {
                case ExpandStrategy.Click:
                    this.Click();
                    break;
                case ExpandStrategy.ExpandCollapsePattern:
                    this.Instance.GetPattern<ExpandCollapsePattern>(ExpandCollapsePattern.Pattern).Collapse();
                    break;
                default:
                    Logger.Error("{0} is not valid or implemented collapse strategy.", strategy);
                    throw new CruciatusException("NOT COLLAPSE");
            }

            Thread.Sleep(250);
        }

        /// <summary>
        /// Раскрывает выпадающий список кликом.
        /// </summary>
        public void Expand()
        {
            this.Expand(ExpandStrategy.Click);
        }

        /// <summary>
        /// Раскрывает выпадающий список.
        /// </summary>
        public void Expand(ExpandStrategy strategy)
        {
            if (this.ExpandCollapseState == ExpandCollapseState.Expanded)
            {
                return;
            }

            switch (strategy)
            {
                case ExpandStrategy.Click:
                    this.Click();
                    break;
                case ExpandStrategy.ExpandCollapsePattern:
                    this.Instance.GetPattern<ExpandCollapsePattern>(ExpandCollapsePattern.Pattern).Expand();
                    break;
                default:
                    Logger.Error("{0} is not valid or implemented expand strategy.", strategy);
                    throw new CruciatusException("NOT EXPAND");
            }

            Thread.Sleep(250);
        }

        /// <summary>
        /// Прокручивает список до элемента, удовлетворяющего стратегии поиска. 
        /// Возвращает целевой элемент, либо null, если он не найден.
        /// </summary>
        /// <param name="getStrategy">
        /// Стратегия поиска целевого элемента.
        /// </param>
        public CruciatusElement ScrollTo(By getStrategy)
        {
            if (!this.Instance.Current.IsEnabled)
            {
                Logger.Error("Element '{0}' not enabled. Scroll failed.", this.ToString());
                CruciatusFactory.Screenshoter.AutomaticScreenshotCaptureIfNeeded();
                throw new ElementNotEnabledException("NOT SCROLL");
            }

            // Проверка, что выпадающий список раскрыт
            if (this.ExpandCollapseState != ExpandCollapseState.Expanded)
            {
                Logger.Error("Element {0} is not opened.", this);
                throw new CruciatusException("NOT SCROLL");
            }

            var scrollPattern = this.Instance.GetCurrentPattern(ScrollPattern.Pattern) as ScrollPattern;
            if (scrollPattern == null)
            {
                Logger.Error("{0} does not support ScrollPattern.", this);
                throw new CruciatusException("NOT SCROLL");
            }

            // Стартовый поиск элемента
            var element = CruciatusCommand.FindFirst(this, getStrategy, 1000);

            // Вертикальная прокрутка (при необходимости и возможности)
            if (element == null && scrollPattern.Current.VerticallyScrollable)
            {
                // Установка самого верхнего положения прокрутки
                while (scrollPattern.Current.VerticalScrollPercent > 0.1)
                {
                    scrollPattern.ScrollVertical(ScrollAmount.LargeDecrement);
                }

                // Установка самого левого положения прокрутки (при возможности)
                if (scrollPattern.Current.HorizontallyScrollable)
                {
                    while (scrollPattern.Current.HorizontalScrollPercent > 0.1)
                    {
                        scrollPattern.ScrollHorizontal(ScrollAmount.LargeDecrement);
                    }
                }

                // Основная вертикальная прокрутка
                while (element == null && scrollPattern.Current.VerticalScrollPercent < 99.9)
                {
                    scrollPattern.ScrollVertical(ScrollAmount.LargeIncrement);
                    element = CruciatusCommand.FindFirst(this, getStrategy, 1000);
                }
            }

            // Если прокрутив до конца элемент не найден, то его нет (кэп)
            if (element == null)
            {
                Logger.Debug("No elements matching {1} were found in {0}.", this, getStrategy);
                return null;
            }

            var strategy =
                By.AutomationProperty(TreeScope.Subtree, AutomationElement.ClassNameProperty, "Popup")
                    .And(AutomationElement.ProcessIdProperty, this.Instance.Current.ProcessId);
            var popupWindow = CruciatusFactory.Root.FindElement(strategy);
            if (popupWindow == null)
            {
                Logger.Error("Popup window of drop-down list was not found.");
                throw new CruciatusException("NOT SCROLL");
            }

            // Если точка клика элемента под границей списка - докручиваем по вертикали вниз
            var popupWindowInstance = popupWindow.Instance;
            while (element.Instance.ClickablePointUnder(popupWindowInstance, scrollPattern))
            {
                scrollPattern.ScrollVertical(ScrollAmount.LargeIncrement);
            }

            // Если точка клика элемента над границей списка - докручиваем по вертикали вверх
            while (element.Instance.ClickablePointOver(popupWindowInstance))
            {
                scrollPattern.ScrollVertical(ScrollAmount.SmallDecrement);
            }

            // Если точка клика элемента справа от границы списка - докручиваем по горизонтали вправо
            while (element.Instance.ClickablePointRight(popupWindowInstance, scrollPattern))
            {
                scrollPattern.ScrollHorizontal(ScrollAmount.LargeIncrement);
            }

            // Если точка клика элемента слева от границы списка - докручиваем по горизонтали влево
            while (element.Instance.ClickablePointLeft(popupWindowInstance))
            {
                scrollPattern.ScrollHorizontal(ScrollAmount.SmallDecrement);
            }

            return element;
        }

        /// <summary>
        /// Возвращает выбранный элемент.
        /// </summary>
        public CruciatusElement SelectedItem()
        {
            return
                this.FindElement(
                    By.AutomationProperty(TreeScope.Subtree, SelectionItemPattern.IsSelectedProperty, true));
        }

        #endregion
    }
}
