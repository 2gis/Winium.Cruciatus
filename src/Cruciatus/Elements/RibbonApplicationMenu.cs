// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonApplicationMenu.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент меню ленты.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus.Elements
{
    #region using

    using System.Windows.Automation;

    using Cruciatus.Core;

    #endregion

    /// <summary>
    /// Представляет элемент управления меню ленты.
    /// </summary>
    public class RibbonApplicationMenu : Menu
    {
        public RibbonApplicationMenu(CruciatusElement parent, By selector)
            : base(parent, selector)
        {
        }

        public override CruciatusElement Get(By selector)
        {
            Click();
            return base.Get(selector);
        }
    }
}
