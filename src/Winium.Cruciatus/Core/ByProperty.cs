namespace Winium.Cruciatus.Core
{
    #region using

    using System.Collections.Generic;
    using System.Windows.Automation;

    using Winium.Cruciatus.Exceptions;
    using Winium.Cruciatus.Helpers;

    #endregion

    internal enum ConditionType
    {
        None, 

        Or, 

        And
    }

    internal struct Info
    {
        #region Fields

        internal ConditionType ConditionType;

        internal AutomationProperty Property;

        internal object Value;

        #endregion

        #region Constructors and Destructors

        internal Info(AutomationProperty property, object value, ConditionType conditionType)
        {
            this.Property = property;
            this.Value = value;
            this.ConditionType = conditionType;
        }

        #endregion
    }

    /// <summary>
    /// Класс-конструктор стратегии поиска элементов по AutomationProperty.
    /// </summary>
    public class ByProperty : By
    {
        #region Fields

        private readonly List<Info> infoList;

        private readonly TreeScope scope;

        #endregion

        #region Constructors and Destructors

        internal ByProperty(TreeScope scope, AutomationProperty property, object value)
        {
            this.scope = scope;
            this.infoList = new List<Info> { new Info(property, value, ConditionType.None) };
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Уточнить поиск по AutomationProperty через логическое И. 
        /// Требуется подключить ссылку на UIAutomationClient.
        /// </summary>
        /// <param name="property">
        /// Целевое свойство.
        /// </param>
        /// <param name="value">
        /// Значение целевого свойства.
        /// </param>
        public ByProperty And(AutomationProperty property, object value)
        {
            this.infoList.Add(new Info(property, value, ConditionType.And));
            return this;
        }

        /// <summary>
        /// Уточнить поиск по ControlType через логическое И.
        /// </summary>
        /// <param name="value">
        /// Тип элемента.
        /// </param>
        public ByProperty AndType(ControlType value)
        {
            this.And(AutomationElement.ControlTypeProperty, value);
            return this;
        }

        /// <summary>
        /// Уточнить поиск по AutomationProperty через логическое ИЛИ. 
        /// Требуется подключить ссылку на UIAutomationClient.
        /// </summary>
        /// <param name="property">
        /// Целевое свойство.
        /// </param>
        /// <param name="value">
        /// Значение целевого свойства.
        /// </param>
        public ByProperty Or(AutomationProperty property, object value)
        {
            this.infoList.Add(new Info(property, value, ConditionType.Or));
            return this;
        }

        /// <summary>
        /// Уточнить поиск по Name элемента через логическое ИЛИ.
        /// </summary>
        /// <param name="value">
        /// Имя элемента.
        /// </param>
        public ByProperty OrName(string value)
        {
            this.Or(AutomationElement.NameProperty, value);
            return this;
        }

        /// <summary>
        /// Возвращает строковое представление стратегии поиска.
        /// </summary>
        public override string ToString()
        {
            var info = this.infoList[0];
            var str = AutomationPropertyHelper.GetPropertyName(info.Property) + ": " + info.Value;
            for (var i = 1; i < this.infoList.Count; ++i)
            {
                info = this.infoList[i];
                var condition = info.ConditionType.ToString().ToLower();
                var propertyName = AutomationPropertyHelper.GetPropertyName(info.Property);
                var propertyKeyValue = propertyName + ": " + info.Value;
                str = string.Format("({0}) {1} {2}", str, condition, propertyKeyValue);
            }

            return str;
        }

        #endregion

        #region Methods

        internal override IEnumerable<AutomationElement> FindAll(AutomationElement parent, int timeout)
        {
            return AutomationElementHelper.FindAll(parent, this.scope, this.GetCondition(), timeout);
        }

        internal override AutomationElement FindFirst(AutomationElement parent, int timeout)
        {
            return AutomationElementHelper.FindFirst(parent, this.scope, this.GetCondition(), timeout);
        }

        private Condition GetCondition()
        {
            var info = this.infoList[0];
            Condition result = new PropertyCondition(info.Property, info.Value);
            for (var i = 1; i < this.infoList.Count; ++i)
            {
                info = this.infoList[i];
                var condition = new PropertyCondition(info.Property, info.Value);
                switch (info.ConditionType)
                {
                    case ConditionType.And:
                        result = new AndCondition(result, condition);
                        break;

                    case ConditionType.Or:
                        result = new OrCondition(result, condition);
                        break;

                    default:
                        throw new CruciatusException("ConditionType ERROR");
                }
            }

            return result;
        }

        #endregion
    }
}
