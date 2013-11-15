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

        public T Item<T>(uint number) where T : BaseElement<T>, new()
        {
            var item = new T();
            var condition = new PropertyCondition(AutomationElement.ControlTypeProperty, item.GetType);

            var searchElement = this.Element.SearchSpecificElementConsideringScroll(
                elem => elem.FindAll(TreeScope.Subtree, condition),
                collection => collection.Count <= number,
                collection => collection.Count > number ? collection[(int)number] : null);

            if (searchElement == null)
            {
                // TODO: Исключение вида - не найдено элемента в списке, удовлетворяющему заданным условиям
                throw new Exception("не нашлось элемента в списке");
            }

            item.FromAutomationElement(searchElement);
            return item;
        }

        public T Item<T>(string name) where T : BaseElement<T>, new()
        {
            var item = new T();
            var condition = new AndCondition(
                new PropertyCondition(AutomationElement.ControlTypeProperty, item.GetType),
                new PropertyCondition(AutomationElement.NameProperty, name));
            
            var searchElement = this.Element.SearchSpecificElementConsideringScroll(
                elem => elem.FindFirst(TreeScope.Subtree, condition),
                elem => elem == null,
                elem => elem);

            if (searchElement == null)
            {
                // TODO: Исключение вида - не найдено элемента в списке, удовлетворяющему заданным условиям
                throw new Exception("не нашлось элемента в списке");
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
    }
}
