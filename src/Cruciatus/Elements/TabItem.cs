// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TabItem.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент управления вкладка.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus.Elements
{
    #region using

    using System;
    using System.Windows.Automation;

    using Cruciatus.Core;
    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;

    #endregion

    /// <summary>
    /// Представляет элемент управления вкладка.
    /// </summary>
    public class TabItem : CruciatusElement
    {
        public TabItem(CruciatusElement parent, By selector)
            : base(parent, selector)
        {
        }

        /// <summary>
        /// Возвращает значение, указывающее, выбрана ли вкладка.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Вкладка не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public bool IsSelection
        {
            get
            {
                return this.GetPropertyValue<bool>(SelectionItemPattern.IsSelectedProperty);
            }
        }

        /// <summary>
        /// Выбирает вкладку текущей.
        /// </summary>
        /// <returns>
        /// Значение true если удалось выбрать либо уже выбрана; в противном случае значение - false.
        /// </returns>
        public void Select()
        {
            if (IsSelection)
            {
                return;
            }

            Click();
        }
    }
}
