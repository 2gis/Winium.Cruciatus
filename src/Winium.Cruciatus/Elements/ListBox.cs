namespace Winium.Cruciatus.Elements
{
    #region using

    using System.Windows.Automation;

    using Winium.Cruciatus.Core;
    using Winium.Cruciatus.Extensions;

    #endregion

    /// <summary>
    /// Элемент список.
    /// </summary>
    public class ListBox : CruciatusElement
    {
        #region Constructors and Destructors

        /// <summary>
        /// Создает экземпляр списка.
        /// </summary>
        /// <param name="element">
        /// Исходный элемент.
        /// </param>
        public ListBox(CruciatusElement element)
            : base(element)
        {
        }

        /// <summary>
        /// Создает экземпляр списка. Поиск осуществится только при необходимости.
        /// </summary>
        /// <param name="parent">
        /// Родительский элемент.
        /// </param>
        /// <param name="getStrategy">
        /// Стратегия поиска элемента.
        /// </param>
        public ListBox(CruciatusElement parent, By getStrategy)
            : base(parent, getStrategy)
        {
        }

        #endregion

        #region Public Methods and Operators

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

            var scrollPattern = this.Instance.GetCurrentPattern(ScrollPattern.Pattern) as ScrollPattern;
            if (scrollPattern == null)
            {
                Logger.Debug("{0} does not support ScrollPattern.", this);
                throw new ElementNotEnabledException("NOT SCROLL");
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

            if (element == null)
            {
                Logger.Debug("No elements matching {1} were found in {0}.", this, getStrategy);
                return null;
            }

            // Если точка клика элемента под границей списка - докручиваем по вертикали вниз
            while (element.Instance.ClickablePointUnder(this.Instance, scrollPattern))
            {
                scrollPattern.ScrollVertical(ScrollAmount.SmallIncrement);
            }

            // Если точка клика элемента над границей списка - докручиваем по вертикали вверх
            while (element.Instance.ClickablePointOver(this.Instance))
            {
                scrollPattern.ScrollVertical(ScrollAmount.SmallDecrement);
            }

            // Если точка клика элемента справа от границы списка - докручиваем по горизонтали вправо
            while (element.Instance.ClickablePointRight(this.Instance, scrollPattern))
            {
                scrollPattern.ScrollHorizontal(ScrollAmount.SmallIncrement);
            }

            // Если точка клика элемента слева от границы списка - докручиваем по горизонтали влево
            while (element.Instance.ClickablePointLeft(this.Instance))
            {
                scrollPattern.ScrollHorizontal(ScrollAmount.SmallDecrement);
            }

            return element;
        }

        #endregion
    }
}
