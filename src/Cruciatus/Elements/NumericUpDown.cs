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
    using System.Linq;
    using System.Windows.Automation;

    using Cruciatus.Interfaces;

    public class NumericUpDown : BaseElement<NumericUpDown>, ILazyInitialize
    {
        private string automationId;

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
            this.automationId = automationId;
        }

        public int Value
        {
            get
            {
                return Convert.ToInt32(this.Element.GetCurrentPropertyValue(RangeValuePattern.ValueProperty));
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

        public void LazyInitialize(AutomationElement parent, string automationId)
        {
            this.parent = parent;
            this.automationId = automationId;
        }

        internal override NumericUpDown FromAutomationElement(AutomationElement element)
        {
            this.element = element;
            this.CheckingOfProperties();

            return this;
        }

        protected override void CheckingOfProperties()
        {
            if (!this.Element.GetSupportedProperties().Contains(RangeValuePattern.ValueProperty))
            {
                // TODO Исключение вида - контрол не поддерживает свойство Value
                throw new Exception("текстовое поле не поддерживает свойство Value");
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