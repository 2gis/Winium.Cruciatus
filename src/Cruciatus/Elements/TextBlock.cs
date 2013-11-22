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
    using System.Windows.Automation;
    using System.Windows.Forms;

    using Cruciatus.Extensions;

    using Microsoft.VisualStudio.TestTools.UITesting;

    using ControlType = System.Windows.Automation.ControlType;

    public class TextBlock : BaseElement<TextBlock>
    {
        private const int MouseMoveSpeed = 2500;

        public System.Drawing.Point ClickablePoint
        {
            get
            {
                var windowsPoint = this.GetPropertyValue<TextBlock, System.Windows.Point>(AutomationElement.ClickablePointProperty);

                return new System.Drawing.Point((int)windowsPoint.X, (int)windowsPoint.Y);
            }
        }

        internal override string ClassName
        {
            get
            {
                return "TextBlock";
            }
        }

        internal override string AutomationId { get; set; }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.Text;
            }
        }

        internal override AutomationElement Element
        {
            get
            {
                return this.element;
            }
        }

        public bool Click(MouseButtons mouseButton = MouseButtons.Left)
        {
            try
            {
                Mouse.MouseMoveSpeed = MouseMoveSpeed;
                Mouse.Move(this.ClickablePoint);
                Mouse.Click(mouseButton);
            }
            catch (Exception exc)
            {
                this.LastErrorMessage = exc.Message;
                return false;
            }

            return true;
        }

        internal override TextBlock FromAutomationElement(AutomationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.element = element;
            return this;
        }
    }
}