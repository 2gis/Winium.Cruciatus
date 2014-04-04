// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IListElement.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Интерфейс элементов, которые могут располагаться в списках.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus.Interfaces
{
    #region using

    using System.Windows.Automation;

    #endregion

    public interface IListElement
    {
        void Initialize(AutomationElement element);
    }
}
