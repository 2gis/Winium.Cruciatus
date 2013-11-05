// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILazyInitialize.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Интерфейс элементов поддерживающих ленивую инициализацию по родителю и ИД.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus.Interfaces
{
    using System.Windows.Automation;

    public interface ILazyInitialize
    {
        void LazyInitialize(AutomationElement parent, string automationId);
    }
}