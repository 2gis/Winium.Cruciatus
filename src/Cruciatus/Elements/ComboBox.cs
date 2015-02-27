// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComboBox.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент управления выпадающий список.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus.Elements
{
    #region using

    using System;
    using System.Threading;
    using System.Windows.Automation;

    using Cruciatus.Core;
    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;

    #endregion

    /// <summary>
    /// Представляет элемент управления выпадающий список.
    /// </summary>
    public class ComboBox : CruciatusElement
    {
        public ComboBox(CruciatusElement element)
            : base(element)
        {
        }

        public ComboBox(CruciatusElement parent, By getStrategy)
            : base(parent, getStrategy)
        {
        }

        /// <summary>
        /// Возвращает значение, указывающее, раскрыт ли выпадающий список.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Выпадающий список не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public bool IsExpanded
        {
            get
            {
                return ExpandCollapseState == ExpandCollapseState.Expanded;
            }
        }

        /// <summary>
        /// Возвращает состояние раскрытости выпадающего списка.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Выпадающий список не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        internal ExpandCollapseState ExpandCollapseState
        {
            get
            {
                return this.GetPropertyValue<ExpandCollapseState>(ExpandCollapsePattern.ExpandCollapseStateProperty);
            }
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
            if (ExpandCollapseState == ExpandCollapseState.Expanded)
            {
                return;
            }

            switch (strategy)
            {
                case ExpandStrategy.Click:
                    this.Click();
                    break;
                case ExpandStrategy.ExpandCollapsePattern:
                    this.Instanse.GetPattern<ExpandCollapsePattern>(ExpandCollapsePattern.Pattern).Expand();
                    break;
                default:
                    Logger.Error("{0} is not valid or implemented expand strategy.", strategy);
                    throw new CruciatusException("NOT EXPAND");
            }

            Thread.Sleep(250);
        }

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
        public void Collapse(ExpandStrategy strategy)
        {
            if (ExpandCollapseState == ExpandCollapseState.Collapsed)
            {
                return;
            }

            switch (strategy)
            {
                case ExpandStrategy.Click:
                    this.Click();
                    break;
                case ExpandStrategy.ExpandCollapsePattern:
                    this.Instanse.GetPattern<ExpandCollapsePattern>(ExpandCollapsePattern.Pattern).Collapse();
                    break;
                default:
                    Logger.Error("{0} is not valid or implemented collapse strategy.", strategy);
                    throw new CruciatusException("NOT COLLAPSE");
            }

            Thread.Sleep(250);
        }

        public CruciatusElement ScrollTo(By getStrategy)
        {
            if (!Instanse.Current.IsEnabled)
            {
                Logger.Error("Element '{0}' not enabled. Scroll failed.", ToString());
                CruciatusFactory.Screenshoter.AutomaticScreenshotCaptureIfNeeded();
                throw new ElementNotEnabledException("NOT SCROLL");
            }

            // Проверка, что выпадающий список раскрыт
            if (ExpandCollapseState != ExpandCollapseState.Expanded)
            {
                Logger.Error(string.Format("Элемент {0} не развернут.", ToString()));
                throw new CruciatusException("NOT SCROLL");
            }

            // Получение шаблона прокрутки у списка
            var scrollPattern = Instanse.GetCurrentPattern(ScrollPattern.Pattern) as ScrollPattern;
            if (scrollPattern == null)
            {
                Logger.Error(string.Format("{0} не поддерживает ScrollPattern.", ToString()));
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
                Logger.Debug("В {0} нет элемента '{1}'.", ToString(), getStrategy);
                return null;
            }

            var strategy = By.AutomationProperty(TreeScope.Subtree, AutomationElement.ClassNameProperty, "Popup")
                .And(AutomationElement.ProcessIdProperty, Instanse.Current.ProcessId);
            var popupWindow = CruciatusFactory.Root.Get(strategy);
            if (popupWindow == null)
            {
                Logger.Error("Не найдено popup окно выпадающего списка.");
                throw new CruciatusException("NOT SCROLL");
            }

            // Если точка клика элемента под границей списка - докручиваем по вертикали вниз
            var popupWindowInstance = popupWindow.Instanse;
            while (element.Instanse.ClickablePointUnder(popupWindowInstance, scrollPattern))
            {
                scrollPattern.ScrollVertical(ScrollAmount.LargeIncrement);
            }

            // Если точка клика элемента над границей списка - докручиваем по вертикали вверх
            while (element.Instanse.ClickablePointOver(popupWindowInstance))
            {
                scrollPattern.ScrollVertical(ScrollAmount.SmallDecrement);
            }

            // Если точка клика элемента справа от границы списка - докручиваем по горизонтали вправо
            while (element.Instanse.ClickablePointRight(popupWindowInstance, scrollPattern))
            {
                scrollPattern.ScrollHorizontal(ScrollAmount.LargeIncrement);
            }

            // Если точка клика элемента слева от границы списка - докручиваем по горизонтали влево
            while (element.Instanse.ClickablePointLeft(popupWindowInstance))
            {
                scrollPattern.ScrollHorizontal(ScrollAmount.SmallDecrement);
            }

            return element;
        }

        public CruciatusElement SelectedItem()
        {
            return Get(By.AutomationProperty(TreeScope.Subtree, SelectionItemPattern.IsSelectedProperty, true));
        }
    }
}
