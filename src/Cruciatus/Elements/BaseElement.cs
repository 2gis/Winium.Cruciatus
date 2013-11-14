// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseElement.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет базу для элементов управления.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus.Elements
{
    using System.Windows.Automation;

    public abstract class BaseElement<T>
    {
        internal AutomationElement element;

        internal abstract string ClassName { get; }

        internal abstract string AutomationId { get; set; }

        internal new abstract ControlType GetType { get; }

        internal abstract AutomationElement Element { get; }

        public new string ToString()
        {
            return string.Format("{0} (uid: {1})", this.ClassName, this.AutomationId ?? "nonUid");
        }

        internal abstract T FromAutomationElement(AutomationElement element);
    }
}