// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IContainerElement.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Интерфейс элементов, которые могут располагаться внутри элемента-контейнера.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus.Interfaces
{
    using System.Windows.Automation;

    public interface IContainerElement
    {
        void Initialize(AutomationElement parent, string automationId);
    }
}
