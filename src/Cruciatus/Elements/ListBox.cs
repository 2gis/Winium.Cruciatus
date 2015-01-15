// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListBox.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент управления список.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus.Elements
{
    #region using

    using System.Windows.Automation;

    using Cruciatus.Core;
    using Cruciatus.Extensions;

    #endregion

    /// <summary>
    /// Представляет элемент управления список.
    /// </summary>
    public class ListBox : CruciatusElement
    {
        public ListBox(CruciatusElement parent)
            : base(parent)
        {
        }

        public ListBox(CruciatusElement parent, By selector)
            : base(parent, selector)
        {
        }

        public CruciatusElement ScrollTo(By selector)
        {
            if (!Instanse.Current.IsEnabled)
            {
                Logger.Error("Element '{0}' not enabled. Scroll failed.", ToString());
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
            var element = CruciatusCommand.FindFirst(this, selector, 1000);

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
                    element = CruciatusCommand.FindFirst(this, selector, 1000);
                }
            }

            // Если прокрутив до конца элемент не найден, то его нет (кэп)
            if (element == null)
            {
                Logger.Debug("В {0} нет элемента '{1}'.", ToString(), selector);
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
    }
}
