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
    using System;
    using System.Windows.Automation;

    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    /// <summary>
    /// Представляет элемент управления список.
    /// </summary>
    public class ListBox : CruciatusElement, IContainerElement
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ListBox"/>.
        /// </summary>
        public ListBox()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ListBox"/>.
        /// </summary>
        /// <param name="parent">
        /// Элемент, являющийся родителем для списка.
        /// </param>
        /// <param name="automationId">
        /// Уникальный идентификатор списка.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Входные параметры не должны быть нулевыми.
        /// </exception>
        public ListBox(AutomationElement parent, string automationId)
        {
            this.Initialize(parent, automationId);
        }

        /// <summary>
        /// Возвращает значение, указывающее, включена ли список.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Список не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public bool IsEnabled
        {
            get
            {
                return this.GetPropertyValue<bool>(AutomationElement.IsEnabledProperty);
            }
        }

        /// <summary>
        /// Возвращает текстовое представление имени класса.
        /// </summary>
        internal override string ClassName
        {
            get
            {
                return "ListBox";
            }
        }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.List;
            }
        }

        /// <summary>
        /// Прокручивает список до элемента с указанным типом и именем.
        /// </summary>
        /// <param name="name">
        /// Имя элемента.
        /// </param>
        /// <typeparam name="T">
        /// Тип элемента.
        /// </typeparam>
        /// <returns>
        /// Интересующий элемент, либо null, если элемент с заданными параметрами отсутствует.
        /// </returns>
        public T ScrollTo<T>(string name) where T : CruciatusElement, IListElement, new()
        {
            try
            {
                // Проверка на дурака
                if (string.IsNullOrEmpty(name))
                {
                    this.LastErrorMessage = "Параметр name не должен быть пустым.";
                    return null;
                }

                // Проверка, что таблица включена
                var isEnabled = CruciatusFactory.WaitingValues(
                    () => this.IsEnabled,
                    value => value != true);
                if (!isEnabled)
                {
                    this.LastErrorMessage = string.Format("{0} отключен.", this.ToString());
                    return null;
                }

                // Получение шаблона прокрутки у списка
                var scrollPattern = this.Element.GetCurrentPattern(ScrollPattern.Pattern) as ScrollPattern;
                if (scrollPattern == null)
                {
                    this.LastErrorMessage = string.Format("{0} не поддерживает шаблон прокрутки.", this.ToString());
                    return null;
                }

                var item = new T();
                var condition = new AndCondition(
                    new PropertyCondition(AutomationElement.ControlTypeProperty, item.GetType),
                    new PropertyCondition(AutomationElement.NameProperty, name));

                // Стартовый поиск элемента
                var element = this.Element.FindFirst(TreeScope.Subtree, condition);

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
                        element = this.Element.FindFirst(TreeScope.Subtree, condition);
                    }
                }

                // Если прокрутив до конца элемент не найден, то его нет (кэп)
                if (element == null)
                {
                    this.LastErrorMessage = string.Format("В {0} нет элемента с name = {1}.", this.ToString(), name);
                    return null;
                }

                // Если точка клика элемента под границей списка - докручиваем по вертикали вниз
                while (element.ClickablePointUnder(this.Element, scrollPattern))
                {
                    scrollPattern.ScrollVertical(ScrollAmount.SmallIncrement);
                }

                // Если точка клика элемента над границей списка - докручиваем по вертикали вверх
                while (element.ClickablePointOver(this.Element))
                {
                    scrollPattern.ScrollVertical(ScrollAmount.SmallDecrement);
                }

                // Если точка клика элемента справа от границы списка - докручиваем по горизонтали вправо
                while (element.ClickablePointRight(this.Element, scrollPattern))
                {
                    scrollPattern.ScrollHorizontal(ScrollAmount.SmallIncrement);
                }

                // Если точка клика элемента слева от границы списка - докручиваем по горизонтали влево
                while (element.ClickablePointLeft(this.Element))
                {
                    scrollPattern.ScrollHorizontal(ScrollAmount.SmallDecrement);
                }

                item.Initialize(element);
                return item;
            }
            catch (CruciatusException exc)
            {
                this.LastErrorMessage = exc.Message;
                return null;
            }
        }

        /// <summary>
        /// Возвращает элемент заданного типа с указанным именем.
        /// </summary>
        /// <param name="name">
        /// Имя элемента.
        /// </param>
        /// <typeparam name="T">
        /// Тип элемента.
        /// </typeparam>
        /// <returns>
        /// Искомый элемент, либо null, если найти не удалось.
        /// </returns>
        public T Item<T>(string name) where T : CruciatusElement, IListElement, new()
        {
            var isEnabled = CruciatusFactory.WaitingValues(
                    () => this.IsEnabled,
                    value => value != true);
            if (!isEnabled)
            {
                this.LastErrorMessage = string.Format("{0} отключен.", this.ToString());
                return null;
            }

            var item = new T();
            var condition = new AndCondition(
                    new PropertyCondition(AutomationElement.ControlTypeProperty, item.GetType),
                    new PropertyCondition(AutomationElement.NameProperty, name));

            var searchElement = this.Element.FindFirst(TreeScope.Subtree, condition);

            if (searchElement == null)
            {
                this.LastErrorMessage = string.Format("В {0} элемент с полем name = {1} не существует или вне видимости.", this.ToString(), name);
                return null;
            }

            if (!this.Element.ContainsClickablePoint(searchElement))
            {
                this.LastErrorMessage = string.Format("В {0} элемент с полем name = {1} вне видимости.", this.ToString(), name);
                return null;
            }

            item.Initialize(searchElement);
            return item;
        }

        void IContainerElement.Initialize(AutomationElement parent, string automationId)
        {
            this.Initialize(parent, automationId);
        }

        /// <summary>
        /// Непосредственный поиск AutomationElement с заданными параметрами.
        /// </summary>
        /// <param name="name">
        /// Имя элемента.
        /// </param>
        /// <param name="type">
        /// Тип элемента.
        /// </param>
        /// <returns>
        /// Элемент если найден; в противном случае - null.
        /// </returns>
        private AutomationElement SearchElement(string name, ControlType type)
        {
            // TODO: Это для WinForms надо, но стоит действовать иначе глобально (определяя что это WinForms)
            var condition = new AndCondition(
                    new PropertyCondition(AutomationElement.ControlTypeProperty, type),
                    new PropertyCondition(AutomationElement.NameProperty, name));

            var searchElement = this.Element.FindFirst(TreeScope.Subtree, condition);

            if (searchElement == null && !type.Equals(ControlType.ListItem))
            {
                searchElement = this.SearchElement(name, ControlType.ListItem);
            }

            return searchElement;
        }
    }
}
