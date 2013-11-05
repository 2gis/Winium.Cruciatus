// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBlock.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент текстовый блок.
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

    using Microsoft.VisualStudio.TestTools.UITesting;

    using ControlType = System.Windows.Automation.ControlType;
    using Point = System.Drawing.Point;
    using Size = System.Drawing.Size;

    public class TextBlock : BaseElement<TextBlock>
    {
        private const int MouseMoveSpeed = 2500;

        public Rectangle BoundingRectangle
        {
            get
            {
                var rect = (Rect)this.Element.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);

                // Усечение дабла дает немного меньший прямоугольник, но он внутри изначального
                return new Rectangle(new Point((int)rect.X, (int)rect.Y), new Size((int)rect.Width, (int)rect.Height));
            }
        }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.Text;
            }
        }

        protected override AutomationElement Element
        {
            get
            {
                return this.element;
            }
        }

        public void Click(MouseButtons mouseButton = MouseButtons.Left)
        {
            var controlBoundingRect = this.BoundingRectangle;

            // TODO Вынести это действие как расширения для типа Rectangle
            var clickablePoint = Point.Add(controlBoundingRect.Location, new Size(controlBoundingRect.Width / 2, controlBoundingRect.Height / 2));

            Mouse.MouseMoveSpeed = MouseMoveSpeed;
            Mouse.Move(clickablePoint);
            Mouse.Click(mouseButton);
        }

        internal override TextBlock FromAutomationElement(AutomationElement element)
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
            if (!this.Element.GetSupportedProperties().Contains(AutomationElement.BoundingRectangleProperty))
            {
                // TODO: Исключение вида - контрол не поддерживает свойство BoundingRectangle
                throw new Exception("текстовый блок не поддерживает свойство BoundingRectangle");
            }
        }
    }
}