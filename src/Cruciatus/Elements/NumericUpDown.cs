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

    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    public class NumericUpDown : CruciatusElement, IContainerElement
    {
        public NumericUpDown()
        {
        }

        public NumericUpDown(AutomationElement parent, string automationId)
        {
            this.Initialize(parent, automationId);
        }

        public int Value
        {
            get
            {
                return Convert.ToInt32(this.GetPropertyValue<double>(RangeValuePattern.ValueProperty));
            }
        }

        internal override string ClassName
        {
            get
            {
                return "NumericUpDown";
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

        void IContainerElement.Initialize(AutomationElement parent, string automationId)
        {
            this.Initialize(parent, automationId);
        }
    }
}
