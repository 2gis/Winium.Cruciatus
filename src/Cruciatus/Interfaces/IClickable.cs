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
    #region using

    using System.Drawing;
    using System.Windows.Forms;

    #endregion

    public interface IClickable
    {
        Point ClickablePoint { get; }

        bool Click();

        bool Click(MouseButtons mouseButton);
    }
}
