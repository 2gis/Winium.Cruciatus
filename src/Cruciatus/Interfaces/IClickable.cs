// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IClickable.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Интерфейс элементов, по которым можно кликнуть.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus.Interfaces
{
    using System.Windows.Forms;

    public interface IClickable
    {
        System.Drawing.Point ClickablePoint { get; }

        bool Click(MouseButtons mouseButton = MouseButtons.Left);
    }
}
