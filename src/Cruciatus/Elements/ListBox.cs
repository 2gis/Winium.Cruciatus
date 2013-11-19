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

    public class ListBox : BaseElement<ListBox>, ILazyInitialize
    {
        private AutomationElement parent;

        public ListBox()
        {
        }

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

            this.parent = parent;
            this.AutomationId = automationId;
        }

        internal override string ClassName
        {
            get
            {
                return "ListBox";
            }
        }

        internal override sealed string AutomationId { get; set; }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.List;
            }
        }

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

        public bool Contains<T>(uint number) where T : BaseElement<T>, new()
        {
            return this.SearchElement<T>(number) != null;
        }

        public bool Contains<T>(string name) where T : BaseElement<T>, new()
        {
            return this.SearchElement<T>(name) != null;
        }

        public bool ScrollTo<T>(uint number) where T : BaseElement<T>, new()
        {
            var searchElement = this.SearchElement<T>(number);
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

        public bool ScrollTo<T>(string name) where T : BaseElement<T>, new()
        {
            var searchElement = this.SearchElement<T>(name);
            if (searchElement == null)
            {
                this.LastErrorMessage = string.Format("В {0} нет элемента с полем name = {1}.", this.ToString(), name);
                return false;
            }

            return this.Scrolling(searchElement);
        }

        public T Item<T>(uint number) where T : BaseElement<T>, new()
        {
            var item = new T();
            var searchElement = this.SearchElement<T>(number);

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

        public T Item<T>(string name) where T : BaseElement<T>, new()
        {
            var item = new T();
            var searchElement = this.SearchElement<T>(name);

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
            this.parent = parent;
            this.AutomationId = automationId;
        }

        internal override ListBox FromAutomationElement(AutomationElement element)
        {
            this.element = element;
            return this;
        }

        private void Find()
        {
            // Ищем в нем первый встретившийся контрол с заданным automationId
            this.element = this.parent.FindFirst(
                TreeScope.Subtree,
                new PropertyCondition(AutomationElement.AutomationIdProperty, this.AutomationId));

            // Если не нашли, то загрузить выпадающий список не удалось
            if (this.element == null)
            {
                throw new ElementNotFoundException(this.ToString());
            }
        }

        private AutomationElement SearchElement<T>(uint number) where T : BaseElement<T>, new()
        {
            var item = new T();
            var condition = new PropertyCondition(AutomationElement.ControlTypeProperty, item.GetType);

            var searchElement = this.Element.SearchSpecificElementConsideringScroll(
                elem => elem.FindAll(TreeScope.Subtree, condition),
                collection => collection.Count <= number,
                collection => collection.Count > number ? collection[(int)number] : null);

            return searchElement;
        }

        private AutomationElement SearchElement<T>(string name) where T : BaseElement<T>, new()
        {
            var item = new T();
            var condition = new AndCondition(
                new PropertyCondition(AutomationElement.ControlTypeProperty, item.GetType),
                new PropertyCondition(AutomationElement.NameProperty, name));

            var searchElement = this.Element.SearchSpecificElementConsideringScroll(
                elem => elem.FindFirst(TreeScope.Subtree, condition),
                elem => elem == null,
                elem => elem);

            return searchElement;
        }

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
