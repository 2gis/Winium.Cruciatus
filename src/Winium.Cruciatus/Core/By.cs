namespace Winium.Cruciatus.Core
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Automation;

    using Winium.Cruciatus.Exceptions;

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

    internal struct ElementFindInfo
    {
        #region Fields

        internal TreeScope TreeScope;

        private readonly List<Info> infoList;

        #endregion

        #region Constructors and Destructors

        internal ElementFindInfo(TreeScope scope, Info info)
        {
            this.TreeScope = scope;
            this.infoList = new List<Info> { info };
        }

        #endregion

        #region Properties

        internal Condition Condition
        {
            get
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
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Возвращает строковое представление стратегии поиска.
        /// </summary>
        public override string ToString()
        {
            var info = this.infoList[0];
            var str = GetPropertyName(info.Property) + ": " + info.Value;
            for (var i = 1; i < this.infoList.Count; ++i)
            {
                info = this.infoList[i];
                string condition;
                switch (info.ConditionType)
                {
                    case ConditionType.And:
                        condition = "and";
                        break;

                    case ConditionType.Or:
                        condition = "or";
                        break;

                    default:
                        throw new CruciatusException("ConditionType ERROR");
                }

                var propertyKeyValue = GetPropertyName(info.Property) + ": " + info.Value;
                str = string.Format("({0}) {1} {2}", str, condition, propertyKeyValue);
            }

            return str;
        }

        #endregion

        #region Methods

        internal void Add(Info info)
        {
            this.infoList.Add(info);
        }

        private static string GetPropertyName(AutomationIdentifier property)
        {
            var pattern = new Regex(@".*\.(?<name>.*)Property");
            return pattern.Match(property.ProgrammaticName).Groups["name"].Value;
        }

        #endregion
    }

    /// <summary>
    /// Класс-конструктор стратегии поиска элементов.
    /// </summary>
    public class By
    {
        #region Fields

        internal readonly List<ElementFindInfo> FindInfoList = new List<ElementFindInfo>();

        #endregion

        #region Constructors and Destructors

        internal By(TreeScope scope, Info info)
        {
            this.FindInfoList.Add(new ElementFindInfo(scope, info));
        }

        internal By(List<ElementFindInfo> findInfoList)
        {
            this.FindInfoList = findInfoList;
        }

        #endregion

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
        public static By AutomationProperty(AutomationProperty property, object value)
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
        public static By AutomationProperty(TreeScope scope, AutomationProperty property, object value)
        {
            return new By(scope, new Info(property, value, ConditionType.None));
        }

        /// <summary>
        /// Поиск по Name.
        /// </summary>
        /// <param name="value">
        /// Имя элемента.
        /// </param>
        public static By Name(string value)
        {
            return Name(TreeScope.Subtree, value);
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
        public static By Name(TreeScope scope, string value)
        {
            return AutomationProperty(scope, AutomationElement.NameProperty, value);
        }

        /// <summary>
        /// Поиск по заданному Path. Например: /#ElementUid//%ElementName, 
        /// где '/' - в детях, '//' - в глубину; # - по AutomationId, % - по Name.
        /// </summary>
        /// <param name="value">
        /// Строка в требуемом формате.
        /// </param>
        public static By Path(string value)
        {
            var findInfoList = new List<ElementFindInfo>();
            var flag = true;
            while (flag)
            {
                TreeScope scope;
                if (value.StartsWith("//"))
                {
                    scope = TreeScope.Subtree;
                    value = value.Remove(0, 2);
                }
                else if (value.StartsWith("/"))
                {
                    scope = TreeScope.Children;
                    value = value.Remove(0, 1);
                }
                else
                {
                    throw new CruciatusException("NOT '/' or '//'");
                }

                AutomationProperty property;
                if (value.StartsWith("#"))
                {
                    property = AutomationElement.AutomationIdProperty;
                }
                else if (value.StartsWith("%"))
                {
                    property = AutomationElement.NameProperty;
                }
                else
                {
                    throw new CruciatusException("NOT '#' or '%'");
                }

                value = value.Remove(0, 1);
                var index = value.IndexOf("/", StringComparison.Ordinal);
                string propertyValue;
                if (index == 0)
                {
                    throw new CruciatusException("NOT FOUND '/' AFTER '#' or '%'");
                }

                if (index == -1)
                {
                    propertyValue = value;
                    flag = false;
                }
                else
                {
                    propertyValue = value.Substring(0, index);
                    value = value.Substring(index);
                }

                findInfoList.Add(new ElementFindInfo(scope, new Info(property, propertyValue, ConditionType.None)));
            }

            return new By(findInfoList);
        }

        /// <summary>
        /// Поиск по  AutomationId.
        /// </summary>
        /// <param name="value">
        /// Уникальный идентификатор элемента.
        /// </param>
        public static By Uid(string value)
        {
            return Uid(TreeScope.Subtree, value);
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
        public static By Uid(TreeScope scope, string value)
        {
            return AutomationProperty(scope, AutomationElement.AutomationIdProperty, value);
        }

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
        public By And(AutomationProperty property, object value)
        {
            this.AddInfoToLast(property, value, ConditionType.And);
            return this;
        }

        /// <summary>
        /// Уточнить поиск по ControlType через логическое И.
        /// </summary>
        /// <param name="value">
        /// Тип элемента.
        /// </param>
        public By AndType(ControlType value)
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
        public By Or(AutomationProperty property, object value)
        {
            this.AddInfoToLast(property, value, ConditionType.Or);
            return this;
        }

        /// <summary>
        /// Уточнить поиск по Name элемента через логическое ИЛИ.
        /// </summary>
        /// <param name="value">
        /// Имя элемента.
        /// </param>
        public By OrName(string value)
        {
            this.Or(AutomationElement.NameProperty, value);
            return this;
        }

        #endregion

        #region Methods

        private void AddInfoToLast(AutomationProperty property, object value, ConditionType conditionType)
        {
            this.FindInfoList.Last().Add(new Info(property, value, conditionType));
        }

        #endregion
    }
}
