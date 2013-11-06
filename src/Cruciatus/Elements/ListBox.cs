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

    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    public class ListBox : BaseElement<ListBox>, ILazyInitialize
    {
        private string automationId;

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
            this.automationId = automationId;
        }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.List;
            }
        }

        protected override AutomationElement Element
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

            AutomationElementCollection items;
            var scrollPattern = (ScrollPattern)this.Element.GetCurrentPattern(ScrollPattern.Pattern);
            if (scrollPattern != null)
            {
                this.Element.MoveMouseToCenter();

                scrollPattern.SetScrollPercent(scrollPattern.Current.HorizontalScrollPercent, 0);

                items = this.Element.FindAll(TreeScope.Subtree, condition);
                while (items.Count <= number && scrollPattern.Current.VerticalScrollPercent < 100)
                {
                    scrollPattern.ScrollVertical(ScrollAmount.LargeIncrement);

                    // TODO: Делать что-нибудь если false?
                    this.Element.WaitForElementReady();

                    items = this.Element.FindAll(TreeScope.Subtree, condition);
                }

                if (items.Count > number)
                {
                    while (!this.Element.GeometricallyContains(items[(int)number]))
                    {
                        scrollPattern.ScrollVertical(ScrollAmount.SmallIncrement);
                    }
                }
            }
            else
            {
                items = this.Element.FindAll(TreeScope.Subtree, condition);
            }

            if (items.Count <= number)
            {
                throw new ArgumentOutOfRangeException("number");
            }

            item.FromAutomationElement(items[(int)number]);
            return item;
        }

        public T Item<T>(string name) where T : BaseElement<T>, new()
        {
            var item = new T();
            var condition = new AndCondition(
                new PropertyCondition(AutomationElement.ControlTypeProperty, item.GetType),
                new PropertyCondition(AutomationElement.NameProperty, name));

            AutomationElement searchElement;
            var scrollPattern = (ScrollPattern)this.Element.GetCurrentPattern(ScrollPattern.Pattern);
            if (scrollPattern != null)
            {
                this.Element.MoveMouseToCenter();

                scrollPattern.SetScrollPercent(scrollPattern.Current.HorizontalScrollPercent, 0);

                searchElement = this.Element.FindFirst(TreeScope.Subtree, condition);
                while (searchElement == null && scrollPattern.Current.VerticalScrollPercent < 100)
                {
                    scrollPattern.ScrollVertical(ScrollAmount.LargeIncrement);

                    // TODO: Делать что-нибудь если false?
                    this.Element.WaitForElementReady();

                    searchElement = this.Element.FindFirst(TreeScope.Subtree, condition);
                }

                if (searchElement != null)
                {
                    while (!this.Element.GeometricallyContains(searchElement))
                    {
                        scrollPattern.ScrollVertical(ScrollAmount.SmallIncrement);
                    }
                }
            }
            else
            {
                searchElement = this.Element.FindFirst(TreeScope.Subtree, condition);
            }

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
            this.automationId = automationId;
        }

        internal override ListBox FromAutomationElement(AutomationElement element)
        {
            this.element = element;
            this.CheckingOfProperties();

            return this;
        }

        protected override void CheckingOfProperties()
        {
            // TODO: Какие-то проверки явно должны быть
        }

        private void Find()
        {
            // Ищем в нем первый встретившийся контрол с заданным automationId
            this.element = this.parent.FindFirst(
                TreeScope.Subtree,
                new PropertyCondition(AutomationElement.AutomationIdProperty, this.automationId));

            // Если не нашли, то загрузить выпадающий список не удалось
            if (this.element == null)
            {
                // TODO: Исключение вида - не найдено контрола с заданным AutomationId
                throw new Exception("список не найден");
            }

            this.CheckingOfProperties();
        }
    }
}
