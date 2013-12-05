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
    using System.Windows.Automation;
    using System.Windows.Forms;

    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    using Microsoft.VisualStudio.TestTools.UITesting;

    using ControlType = System.Windows.Automation.ControlType;

    public class DateTimePicker : BaseElement<DateTimePicker>, ILazyInitialize
    {
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
            this.AutomationId = automationId;
        }

        public System.Drawing.Point ClickablePoint
        {
            get
            {
                var windowsPoint = this.GetPropertyValue<DateTimePicker, System.Windows.Point>(AutomationElement.ClickablePointProperty);

                return new System.Drawing.Point((int)windowsPoint.X, (int)windowsPoint.Y);
            }
        }

        internal override string ClassName
        {
            get
            {
                return "DateTimePicker";
            }
        }

        internal override sealed string AutomationId { get; set; }

        internal override ControlType GetType
        {
            get
            {
                // TODO: а какой он??
                return ControlType.Custom;
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

        public void SetDateTime(string value)
        {
            Mouse.MouseMoveSpeed = CruciatusFactory.Settings.MouseMoveSpeed;
            Mouse.Move(this.ClickablePoint);
            Mouse.Click(MouseButtons.Left);
            Keyboard.SendKeys("^a");
            Keyboard.SendKeys(value);
            Keyboard.SendKeys("{Enter}");
        }

        public void LazyInitialize(AutomationElement parent, string automationId)
        {
            this.parent = parent;
            this.AutomationId = automationId;
        }

        internal override DateTimePicker FromAutomationElement(AutomationElement element)
        {
            this.element = element;
            return this;
        }

        private void Find()
        {
            this.element = this.parent.FindFirst(
                TreeScope.Subtree,
                new PropertyCondition(AutomationElement.AutomationIdProperty, this.AutomationId));

            // Если не нашли, то загрузить элемент не удалось
            if (this.element == null)
            {
                throw new ElementNotFoundException(this.ToString());
            }
        }
    }
}
