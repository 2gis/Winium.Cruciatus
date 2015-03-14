namespace Winium.Cruciatus.Elements
{
    #region using

    using System.Windows.Automation;

    using Winium.Cruciatus.Core;
    using Winium.Cruciatus.Extensions;

    #endregion

    /// <summary>
    /// Элемент вкладка. Требуется поддержка интерфейса SelectionItemPattern.
    /// </summary>
    public class TabItem : CruciatusElement
    {
        #region Constructors and Destructors

        /// <summary>
        /// Создает экземпляр вкладки. Поиск осуществится только при необходимости.
        /// </summary>
        /// <param name="parent">
        /// Родительский элемент.
        /// </param>
        /// <param name="getStrategy">
        /// Стратегия поиска элемента.
        /// </param>
        public TabItem(CruciatusElement parent, By getStrategy)
            : base(parent, getStrategy)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Возвращает значение, указывающее, выбрана ли вкладка.
        /// </summary>
        public bool IsSelection
        {
            get
            {
                return this.GetAutomationPropertyValue<bool>(SelectionItemPattern.IsSelectedProperty);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Выбирает вкладку текущей.
        /// </summary>
        public void Select()
        {
            if (this.IsSelection)
            {
                return;
            }

            this.Click();
        }

        #endregion
    }
}
