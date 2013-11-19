// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumericUpDown.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет какой-то элемент управления.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus.Elements
{
    using System;
    using System.Windows.Automation;

    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    public class NumericUpDown : BaseElement<NumericUpDown>, ILazyInitialize
    {
        private AutomationElement parent;

        public NumericUpDown()
        {
        }

        public NumericUpDown(AutomationElement parent, string automationId)
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

        public int Value
        {
            get
            {
                return Convert.ToInt32(this.GetPropertyValue<NumericUpDown, double>(RangeValuePattern.ValueProperty));
            }
        }

        internal override string ClassName
        {
            get
            {
                return "NumericUpDown";
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

        public void LazyInitialize(AutomationElement parent, string automationId)
        {
            this.parent = parent;
            this.AutomationId = automationId;
        }

        internal override NumericUpDown FromAutomationElement(AutomationElement element)
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