namespace Winium.Cruciatus.Elements
{
    #region using

    using System.Windows.Automation;

    using Winium.Cruciatus.Core;
    using Winium.Cruciatus.Extensions;

    #endregion

    /// <summary>
    /// Представляет элемент управления чекбокс.
    /// </summary>
    public class CheckBox : CruciatusElement
    {
        #region Constructors and Destructors

        /// <summary>
        /// Создает экземпляр чекбокса.
        /// </summary>
        /// <param name="element">
        /// Исходный элемент.
        /// </param>
        public CheckBox(CruciatusElement element)
            : base(element)
        {
        }

        /// <summary>
        /// Создает экземпляр чекбокса. Поиск осуществится только при необходимости.
        /// </summary>
        /// <param name="parent">
        /// Родительский элемент.
        /// </param>
        /// <param name="getStrategy">
        /// Стратегия поиска элемента.
        /// </param>
        public CheckBox(CruciatusElement parent, By getStrategy)
            : base(parent, getStrategy)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Возвращает значение, указывающее, чекнут ли чекбокс.
        /// </summary>
        public bool IsToggleOn
        {
            get
            {
                return this.ToggleState == ToggleState.On;
            }
        }

        #endregion

        #region Properties

        internal ToggleState ToggleState
        {
            get
            {
                return this.GetAutomationPropertyValue<ToggleState>(TogglePattern.ToggleStateProperty);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Устанавливает чекбокс в состояние чекнут.
        /// </summary>
        public void Check()
        {
            if (this.IsToggleOn)
            {
                return;
            }

            this.Click();
        }

        /// <summary>
        /// Устанавливает чекбокс в состояние не чекнут.
        /// </summary>
        public void Uncheck()
        {
            if (!this.IsToggleOn)
            {
                return;
            }

            this.Click();
        }

        #endregion
    }
}
