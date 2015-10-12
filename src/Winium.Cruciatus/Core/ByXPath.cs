namespace Winium.Cruciatus.Core
{
    #region using

    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;

    using Winium.Cruciatus.Helpers;

    #endregion

    /// <summary>
    /// Класс-конструктор стратегии поиска элементов по XPath.
    /// </summary>
    public class ByXPath : By
    {
        #region Fields

        private readonly string xpath;

        #endregion

        #region Constructors and Destructors

        internal ByXPath(string xpath)
        {
            this.xpath = xpath;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Возвращает строковое представление стратегии поиска.
        /// </summary>
        public override string ToString()
        {
            return this.xpath;
        }

        #endregion

        #region Methods

        internal override IEnumerable<AutomationElement> FindAll(AutomationElement parent, int timeout)
        {
            return AutomationElementHelper.FindAll(parent, this.xpath, timeout);
        }

        internal override AutomationElement FindFirst(AutomationElement parent, int timeout)
        {
            return AutomationElementHelper.FindAll(parent, this.xpath, timeout).FirstOrDefault();
        }

        #endregion
    }
}
