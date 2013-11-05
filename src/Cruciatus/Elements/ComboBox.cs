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
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Forms;

    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    using Microsoft.VisualStudio.TestTools.UITesting;

    using ControlType = System.Windows.Automation.ControlType;
    using Point = System.Drawing.Point;
    using Size = System.Drawing.Size;

    public class ComboBox : BaseElement<ComboBox>, ILazyInitialize
    {
        private const int MouseMoveSpeed = 2500;

        private string automationId;

        private AutomationElement parent;

        public ComboBox()
        {
        }

        public ComboBox(AutomationElement parent, string automationId)
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

        /// <summary>
        /// Возвращает значение, указывающее, включен ли данный элемент управления.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return (bool)this.Element.GetCurrentPropertyValue(AutomationElement.IsEnabledProperty);
            }
        }

        /// <summary>
        /// Возвращает координаты прямоугольника, который полностью охватывает элемент.
        /// </summary>
        public Rectangle BoundingRectangle
        {
            get
            {
                var rect = (Rect)this.Element.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);

                // Усечение дабла дает немного меньший прямоугольник, но он внутри изначального
                return new Rectangle(new Point((int)rect.X, (int)rect.Y), new Size((int)rect.Width, (int)rect.Height));
            }
        }

        public ExpandCollapseState State
        {
            get
            {
                return (ExpandCollapseState)this.Element.GetCurrentPropertyValue(ExpandCollapsePattern.ExpandCollapseStateProperty);
            }
        }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.ComboBox;
            }
        }

        /// <summary>
        /// Возвращает инициализированный элемент.
        /// </summary>
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

        public void LazyInitialize(AutomationElement parent, string automationId)
        {
            this.parent = parent;
            this.automationId = automationId;
        }

        public void Expend()
        {
            if (this.State == ExpandCollapseState.Collapsed)
            {
                this.Click();
            }
        }

        public void Collapsed()
        {
            if (this.State != ExpandCollapseState.Collapsed)
            {
                this.Click();
            }
        }

        public T Item<T>(uint number) where T : BaseElement<T>, new()
        {
            this.Expend();

            var item = new T();
            var condition = new PropertyCondition(AutomationElement.ControlTypeProperty, item.GetType);

            var items = this.Element.FindAll(TreeScope.Subtree, condition);
            if (items.Count <= number)
            {
                throw new ArgumentOutOfRangeException("number");
            }

            item.FromAutomationElement(items[(int)number]);
            return item;
        }

        public T Item<T>(string name) where T : BaseElement<T>, new()
        {
            this.Expend();

            var item = new T();
            var condition = new AndCondition(
                new PropertyCondition(AutomationElement.ControlTypeProperty, item.GetType),
                new PropertyCondition(AutomationElement.NameProperty, name));
            
            var elem = this.Element.FindFirst(TreeScope.Subtree, condition);
            if (elem == null)
            {
                // TODO: Исключение вида - не найдено элемента в списке, удовлетворяющему заданным условиям
                throw new Exception("не нашлось элемента в списке комбобокса");
            }

            item.FromAutomationElement(elem);
            return item;
        }

        internal override ComboBox FromAutomationElement(AutomationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.element = element;
            this.CheckingOfProperties();

            return this;
        }

        protected override void CheckingOfProperties()
        {
            if (!this.Element.GetSupportedProperties().Contains(AutomationElement.IsEnabledProperty))
            {
                // TODO: Исключение вида - контрол не поддерживает свойство Enabled
                throw new Exception("выпадающий список не поддерживает свойство Enabled");
            }

            if (!this.Element.GetSupportedProperties().Contains(AutomationElement.BoundingRectangleProperty))
            {
                // TODO: Исключение вида - контрол не поддерживает свойство BoundingRectangle
                throw new Exception("выпадающий список не поддерживает свойство BoundingRectangle");
            }

            if (!this.Element.GetSupportedProperties().Contains(ExpandCollapsePattern.ExpandCollapseStateProperty))
            {
                // TODO: Исключение вида - контрол не поддерживает свойство ExpandCollapseState
                throw new Exception("выпадающий список не поддерживает свойство ExpandCollapseState");
            }
        }

        private void Click(MouseButtons mouseButton = MouseButtons.Left)
        {
            if (!this.IsEnabled)
            {
                throw new ElementNotEnabledException("Выпадающий список отключен, нельзя выполнить нажатие.");
            }

            var controlBoundingRect = this.BoundingRectangle;

            // TODO Вынести это действие как расширения для типа Rectangle
            var clickablePoint = Point.Add(controlBoundingRect.Location, new Size(controlBoundingRect.Width / 2, controlBoundingRect.Height / 2));

            Mouse.MouseMoveSpeed = MouseMoveSpeed;
            Mouse.Move(clickablePoint);
            Mouse.Click(mouseButton);

            if (!this.Element.WaitForElementReady())
            {
                // TODO: Стопить ли все исключением, если время ожидания готовности истекло
                throw new Exception("Время ожидания готовности выпадающего списка истекло!");
            }
        }

        /// <summary>
        /// Поиск текущего элемента в родительском
        /// </summary>
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
                throw new Exception("выпадающий список не найден");
            }

            this.CheckingOfProperties();
        }
    }
}