// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePicker.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент управления поле с датой.
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

    using Cruciatus.Interfaces;

    using Microsoft.VisualStudio.TestTools.UITesting;

    using ControlType = System.Windows.Automation.ControlType;
    using Point = System.Drawing.Point;
    using Size = System.Drawing.Size;

    public class DateTimePicker : BaseElement<DateTimePicker>, ILazyInitialize
    {
        private const int MouseMoveSpeed = 2500;

        private string automationId;

        private AutomationElement parent;

        public DateTimePicker()
        {
        }

        public DateTimePicker(AutomationElement parent, string automationId)
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

        public Rectangle BoundingRectangle
        {
            get
            {
                var rect = (Rect)this.Element.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);

                return new Rectangle(new Point((int)rect.X, (int)rect.Y), new Size((int)rect.Width, (int)rect.Height));
            }
        }

        internal override ControlType GetType
        {
            get
            {
                // TODO: а какой он??
                return ControlType.Custom;
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

        public void SetDateTime(string value)
        {
            var controlBoundingRect = this.BoundingRectangle;

            // TODO Вынести это действие как расширения для типа Rectangle
            var clickablePoint = Point.Add(controlBoundingRect.Location, new Size(controlBoundingRect.Width / 2, controlBoundingRect.Height / 2));

            Mouse.MouseMoveSpeed = MouseMoveSpeed;
            Mouse.Move(clickablePoint);
            Mouse.Click(MouseButtons.Left);
            Keyboard.SendKeys("^a");
            Keyboard.SendKeys(value);
            Keyboard.SendKeys("{Enter}");
        }

        public void LazyInitialize(AutomationElement parent, string automationId)
        {
            this.parent = parent;
            this.automationId = automationId;
        }

        internal override DateTimePicker FromAutomationElement(AutomationElement element)
        {
            this.element = element;
            this.CheckingOfProperties();

            return this;
        }

        protected override void CheckingOfProperties()
        {
            if (!this.Element.GetSupportedProperties().Contains(AutomationElement.BoundingRectangleProperty))
            {
                // TODO Исключение вида - контрол не поддерживает свойство BoundingRectangle
                throw new Exception("текстовое поле не поддерживает свойство BoundingRectangle");
            }
        }

        private void Find()
        {
            this.element = this.parent.FindFirst(
                TreeScope.Subtree,
                new PropertyCondition(AutomationElement.AutomationIdProperty, this.automationId));

            // Если не нашли, то загрузить элемент не удалось
            if (this.element == null)
            {
                // TODO: Исключение вида - не найдено контрола с заданным AutomationId
                throw new Exception("текстовое поле не найдено");
            }

            this.CheckingOfProperties();
        }
    }
}
