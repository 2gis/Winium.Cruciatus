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
    using System.Runtime.InteropServices;
    using System.Windows.Automation;

    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    /// <summary>
    /// Представляет элемент управления список.
    /// </summary>
    public class ListBox : BaseElement<ListBox>, ILazyInitialize
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
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            if (automationId == null)
            {
                throw new ArgumentNullException("automationId");
            }

            this.Parent = parent;
            this.AutomationId = automationId;
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

        /// <summary>
        /// Возвращает или задает уникальный идентификатор списка.
        /// </summary>
        internal override sealed string AutomationId { get; set; }

        /// <summary>
        /// Возвращает или задает элемент, который является родителем списка.
        /// </summary>
        internal AutomationElement Parent { get; set; }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.List;
            }
        }

        /// <summary>
        /// Возвращает инициализированный элемент списка.
        /// </summary>
        internal override AutomationElement Element
        {
            get
            {
                if (this.element == null)
                {
                    this.Find();
                }

                return this.element;
            }
        }

        /// <summary>
        /// Проверяет, если ли в списке элемент заданного типа с указанным номером.
        /// </summary>
        /// <param name="number">
        /// Номер элемента.
        /// </param>
        /// <typeparam name="T">
        /// Тип элемента.
        /// </typeparam>
        /// <returns>
        /// Искомый элемент, либо null, если найти не удалось.
        /// </returns>
        public bool Contains<T>(uint number) where T : BaseElement<T>, new()
        {
            return this.SearchElement(number, new T().GetType) != null;
        }

        /// <summary>
        /// Проверяет, если ли в списке элемент заданного типа с указанным именем.
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
        public bool Contains<T>(string name) where T : BaseElement<T>, new()
        {
            return this.SearchElement(name, new T().GetType) != null;
        }

        /// <summary>
        /// Прокручивает список до элемента заданного типа с указанным номером.
        /// </summary>
        /// <param name="number">
        /// Номер элемента.
        /// </param>
        /// <typeparam name="T">
        /// Тип элемента.
        /// </typeparam>
        /// <returns>
        /// Значение true если прокрутить удалось либо в этом нет необходимости; в противном случае значение - false.
        /// </returns>
        public bool ScrollTo<T>(uint number) where T : BaseElement<T>, new()
        {
            var searchElement = this.SearchElement(number, new T().GetType);
            if (searchElement == null)
            {
                this.LastErrorMessage = string.Format(
                    "В {0} количество элементов меньше заданного номера ({1}).",
                    this.ToString(),
                    number);
                return false;
            }

            return this.Scrolling(searchElement);
        }

        /// <summary>
        /// Прокручивает список до элемента заданного типа с указанным именем.
        /// </summary>
        /// <param name="name">
        /// Имя элемента.
        /// </param>
        /// <typeparam name="T">
        /// Тип элемента.
        /// </typeparam>
        /// <returns>
        /// Значение true если прокрутить удалось либо в этом нет необходимости; в противном случае значение - false.
        /// </returns>
        public bool ScrollTo<T>(string name) where T : BaseElement<T>, new()
        {
            var searchElement = this.SearchElement(name, new T().GetType);
            if (searchElement == null)
            {
                this.LastErrorMessage = string.Format("В {0} нет элемента с полем name = {1}.", this.ToString(), name);
                return false;
            }

            return this.Scrolling(searchElement);
        }

        /// <summary>
        /// Возвращает элемент заданного типа с указанным номером.
        /// </summary>
        /// <param name="number">
        /// Номер элемента.
        /// </param>
        /// <typeparam name="T">
        /// Тип элемента.
        /// </typeparam>
        /// <returns>
        /// Искомый элемент, либо null, если найти не удалось.
        /// </returns>
        public T Item<T>(uint number) where T : BaseElement<T>, new()
        {
            var item = new T();
            var searchElement = this.SearchElement(number, item.GetType);

            if (searchElement == null)
            {
                this.LastErrorMessage = string.Format(
                    "В {0} количество элементов меньше заданного номера ({1}).",
                    this.ToString(),
                    number);
                return null;
            }

            if (!this.Element.GeometricallyContains(searchElement))
            {
                this.LastErrorMessage = string.Format("В {0} элемент под номером {1} вне видимости.", this.ToString(), number);
                return null;
            }

            item.FromAutomationElement(searchElement);
            return item;
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
        public T Item<T>(string name) where T : BaseElement<T>, new()
        {
            var item = new T();
            var searchElement = this.SearchElement(name, item.GetType);

            if (searchElement == null)
            {
                this.LastErrorMessage = string.Format("В {0} нет элемента с полем name = {1}.", this.ToString(), name);
                return null;
            }

            if (!this.Element.GeometricallyContains(searchElement))
            {
                this.LastErrorMessage = string.Format("В {0} элемент с полем name = {1} вне видимости.", this.ToString(), name);
                return null;
            }

            item.FromAutomationElement(searchElement);
            return item;
        }

        public void LazyInitialize(AutomationElement parent, string automationId)
        {
            this.Parent = parent;
            this.AutomationId = automationId;
        }

        internal override ListBox FromAutomationElement(AutomationElement element)
        {
            this.element = element;
            return this;
        }

        /// <summary>
        /// Поиск списка в родительском элементе.
        /// </summary>
        private void Find()
        {
            // Ищем в нем первый встретившийся контрол с заданным automationId
            var condition = new PropertyCondition(AutomationElement.AutomationIdProperty, this.AutomationId);
            this.element = CruciatusFactory.WaitingValues(
                () => this.Parent.FindFirst(TreeScope.Subtree, condition),
                value => value == null);

            // Если не нашли, то загрузить выпадающий список не удалось
            if (this.element == null)
            {
                throw new ElementNotFoundException(this.ToString());
            }
        }

        /// <summary>
        /// Непосредственный поиск AutomationElement с заданными параметрами.
        /// </summary>
        /// <param name="number">
        /// Номер элемента.
        /// </param>
        /// <param name="type">
        /// Тип элемента.
        /// </param>
        /// <returns>
        /// Элемент если найден; в противном случае - null.
        /// </returns>
        private AutomationElement SearchElement(uint number, ControlType type)
        {
            var condition = new PropertyCondition(AutomationElement.ControlTypeProperty, type);

            var searchElement = this.Element.SearchSpecificElementConsideringScroll(
                elem => elem.FindAll(TreeScope.Subtree, condition),
                collection => collection.Count <= number,
                collection => collection.Count > number ? collection[(int)number] : null);

            if (searchElement == null && !type.Equals(ControlType.ListItem))
            {
                searchElement = this.SearchElement(number, ControlType.ListItem);
            }

            return searchElement;
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
            var condition = new AndCondition(
                    new PropertyCondition(AutomationElement.ControlTypeProperty, type),
                    new PropertyCondition(AutomationElement.NameProperty, name));

            var searchElement = this.Element.SearchSpecificElementConsideringScroll(
                elem => elem.FindFirst(TreeScope.Subtree, condition),
                elem => elem == null,
                elem => elem);

            if (searchElement == null && !type.Equals(ControlType.ListItem))
            {
                searchElement = this.SearchElement(name, ControlType.ListItem);
            }

            return searchElement;
        }

        /// <summary>
        /// Непосредственная прокрутка до заданного AutomationElement.
        /// </summary>
        /// <param name="innerElement">
        /// Элемент до которого надо прокрутить.
        /// </param>
        /// <returns>
        /// Значение true если прокрутить удалось либо в этом нет необходимости; в противном случае значение - false.
        /// </returns>
        private bool Scrolling(AutomationElement innerElement)
        {
            var scrollingResult = this.Element.Scrolling(innerElement);
            if (!scrollingResult)
            {
                this.LastErrorMessage = string.Format("Элемент {0} не поддерживает прокрутку содержимого.", this.ToString());
                return false;
            }

            return true;
        }
    }
}
