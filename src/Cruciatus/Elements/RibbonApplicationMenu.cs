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

    using Cruciatus.Core;

    #endregion

    /// <summary>
    /// Представляет элемент управления меню ленты.
    /// </summary>
    public class RibbonApplicationMenu : Menu
    {
        /// <summary>
        /// Создает экземпляр меню приложения ленты.
        /// </summary>
        /// <param name="parent">
        /// Родительский элемент.
        /// </param>
        /// <param name="getStrategy">
        /// Стратегия получения элемента.
        /// </param>
        public RibbonApplicationMenu(CruciatusElement parent, By getStrategy)
            : base(parent, getStrategy)
        {
        }

        public override CruciatusElement Get(By strategy)
        {
            Click();
            return base.Get(strategy);
        }
    }
}
