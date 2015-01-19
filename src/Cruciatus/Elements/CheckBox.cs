// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckBox.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент управления чекбокс.
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
    /// Представляет элемент управления чекбокс.
    /// </summary>
    public class CheckBox : CruciatusElement
    {
        public CheckBox(CruciatusElement element)
            : base(element)
        {
        }

        public CheckBox(CruciatusElement parent, By getStrategy)
            : base(parent, getStrategy)
        {
        }

        /// <summary>
        /// Возвращает значение, указывающее, чекнут ли чекбокс.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Чекбокс не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public bool IsToggleOn
        {
            get
            {
                return ToggleState == ToggleState.On;
            }
        }

        internal ToggleState ToggleState
        {
            get
            {
                return this.GetPropertyValue<ToggleState>(TogglePattern.ToggleStateProperty);
            }
        }

        /// <summary>
        /// Устанавливает чекбокс в состояние чекнут.
        /// </summary>
        public void Check()
        {
            if (IsToggleOn)
            {
                return;
            }

            Click();
        }

        /// <summary>
        /// Устанавливает чекбокс в состояние не чекнут.
        /// </summary>
        public void Uncheck()
        {
            if (!IsToggleOn)
            {
                return;
            }

            Click();
        }
    }
}
