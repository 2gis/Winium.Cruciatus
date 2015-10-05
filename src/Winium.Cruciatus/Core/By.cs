namespace Winium.Cruciatus.Core
{
    #region using

    using System.Collections.Generic;
    using System.Windows.Automation;

    #endregion

    /// <summary>
    /// Конструктор стратегии поиска элементов.
    /// </summary>
    public abstract class By
    {
        #region Public Methods and Operators

        /// <summary>
        /// Поиск по AutomationProperty. 
        /// Требуется подключить ссылку на UIAutomationClient.
        /// </summary>
        /// <param name="property">
        /// Целевое свойство.
        /// </param>
        /// <param name="value">
        /// Значение целевого свойства.
        /// </param>
        public static ByProperty AutomationProperty(AutomationProperty property, object value)
        {
            return AutomationProperty(TreeScope.Subtree, property, value);
        }

        /// <summary>
        /// Поиск по AutomationProperty с заданием глубины. 
        /// Требуется подключить ссылку на UIAutomationClient.
        /// </summary>
        /// <param name="scope">
        /// Глубина поиска.
        /// </param>
        /// <param name="property">
        /// Целевое свойство.
        /// </param>
        /// <param name="value">
        /// Значение целевого свойства.
        /// </param>
        public static ByProperty AutomationProperty(TreeScope scope, AutomationProperty property, object value)
        {
            return new ByProperty(scope, property, value);
        }

        /// <summary>
        /// Поиск по Name.
        /// </summary>
        /// <param name="value">
        /// Имя элемента.
        /// </param>
        public static ByProperty Name(string value)
        {
            return AutomationProperty(AutomationElement.NameProperty, value);
        }

        /// <summary>
        /// Поиск по Name с заданием глубины.
        /// </summary>
        /// <param name="scope">
        /// Глубина поиска.
        /// </param>
        /// <param name="value">
        /// Имя элемента.
        /// </param>
        public static ByProperty Name(TreeScope scope, string value)
        {
            return AutomationProperty(scope, AutomationElement.NameProperty, value);
        }

        /// <summary>
        /// Поиск по  AutomationId.
        /// </summary>
        /// <param name="value">
        /// Уникальный идентификатор элемента.
        /// </param>
        public static ByProperty Uid(string value)
        {
            return AutomationProperty(AutomationElement.AutomationIdProperty, value);
        }

        /// <summary>
        /// Поиск по AutomationId.
        /// </summary>
        /// <param name="scope">
        /// Глубина поиска.
        /// </param>
        /// <param name="value">
        /// Уникальный идентификатор элемента.
        /// </param>
        public static ByProperty Uid(TreeScope scope, string value)
        {
            return AutomationProperty(scope, AutomationElement.AutomationIdProperty, value);
        }

        /// <summary>
        /// Поиск по  XPath.
        /// </summary>
        /// <param name="value">
        /// Путь до файла в формате XPath.
        /// </param>
        public static ByXPath XPath(string value)
        {
            return new ByXPath(value);
        }

        /// <summary>
        /// Возвращает строковое представление стратегии поиска.
        /// </summary>
        public abstract override string ToString();

        #endregion

        #region Methods

        internal abstract IEnumerable<AutomationElement> FindAll(AutomationElement parent, int timeout);

        internal abstract AutomationElement FindFirst(AutomationElement parent, int timeout);

        #endregion
    }
}
