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
        /// Раскрывает выпадающий список.
        /// </summary>
        /// <returns>
        /// Значение true если удалось раскрыть либо если уже раскрыт; в противном случае значение - false.
        /// </returns>
        public void Expand()
        {
            if (ExpandCollapseState == ExpandCollapseState.Expanded)
            {
                return;
            }

            Click();
        }

        /// <summary>
        /// Сворачивает выпадающий список.
        /// </summary>
        /// <returns>
        /// Значение true если удалось свернуть либо если уже свернут; в противном случае значение - false.
        /// </returns>
        public void Collapse()
        {
            if (ExpandCollapseState != ExpandCollapseState.Collapsed)
            {
                Click();
            }
        }

        public CruciatusElement ScrollTo(By getStrategy)
        {
            if (!Instanse.Current.IsEnabled)
            {
                Logger.Error("Element '{0}' not enabled. Scroll failed.", ToString());
                throw new ElementNotEnabledException("NOT SCROLL");
            }

            // Проверка, что выпадающий список раскрыт
            if (ExpandCollapseState != ExpandCollapseState.Expanded)
            {
                Logger.Error(string.Format("Элемент {0} не развернут.", ToString()));
                throw new ElementNotEnabledException("NOT SCROLL");
            }

            // Получение шаблона прокрутки у списка
            var scrollPattern = Instanse.GetCurrentPattern(ScrollPattern.Pattern) as ScrollPattern;
            if (scrollPattern == null)
            {
                Logger.Debug(string.Format("{0} не поддерживает шаблон прокрутки.", ToString()));
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

            // Если прокрутив до конца элемент не найден, то его нет (кэп)
            if (element == null)
            {
                Logger.Debug("В {0} нет элемента '{1}'.", ToString(), getStrategy);
                return null;
            }

            // Если точка клика элемента под границей списка - докручиваем по вертикали вниз
            while (element.Instanse.ClickablePointUnder(Instanse, scrollPattern))
            {
                scrollPattern.ScrollVertical(ScrollAmount.SmallIncrement);
            }

            // Если точка клика элемента над границей списка - докручиваем по вертикали вверх
            while (element.Instanse.ClickablePointOver(Instanse))
            {
                scrollPattern.ScrollVertical(ScrollAmount.SmallDecrement);
            }

            // Если точка клика элемента справа от границы списка - докручиваем по горизонтали вправо
            while (element.Instanse.ClickablePointRight(Instanse, scrollPattern))
            {
                scrollPattern.ScrollHorizontal(ScrollAmount.SmallIncrement);
            }

            // Если точка клика элемента слева от границы списка - докручиваем по горизонтали влево
            while (element.Instanse.ClickablePointLeft(Instanse))
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
