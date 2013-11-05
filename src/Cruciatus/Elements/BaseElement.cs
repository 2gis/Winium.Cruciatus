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
        protected AutomationElement element;

        internal new abstract ControlType GetType { get; }

        protected abstract AutomationElement Element { get; }

        internal abstract T FromAutomationElement(AutomationElement element);

        protected abstract void CheckingOfProperties();
    }
}