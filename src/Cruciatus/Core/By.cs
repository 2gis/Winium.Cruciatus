namespace Cruciatus.Core
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Automation;

    using Cruciatus.Exceptions;

    #endregion

    internal enum ConditionType
    {
        None, 

        Or, 

        And
    }

    internal struct Info
    {
        internal ConditionType ConditionType;

        internal AutomationProperty Property;

        internal object Value;

        internal Info(AutomationProperty property, object value, ConditionType conditionType)
        {
            Property = property;
            Value = value;
            ConditionType = conditionType;
        }
    }

    internal struct ElementFindInfo
    {
        private readonly List<Info> _infoList;

        internal TreeScope TreeScope;

        internal ElementFindInfo(TreeScope scope, Info info)
        {
            TreeScope = scope;
            _infoList = new List<Info> { info };
        }

        internal Condition Condition
        {
            get
            {
                var info = _infoList[0];
                Condition result = new PropertyCondition(info.Property, info.Value);
                for (var i = 1; i < _infoList.Count; ++i)
                {
                    info = _infoList[i];
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

        internal void Add(Info info)
        {
            _infoList.Add(info);
        }

        public override string ToString()
        {
            var info = _infoList[0];
            var str = GetPropertyName(info.Property) + ": " + info.Value;
            for (var i = 1; i < _infoList.Count; ++i)
            {
                info = _infoList[i];
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

        private static string GetPropertyName(AutomationIdentifier property)
        {
            var pattern = new Regex(@".*\.(?<name>.*)Property");
            return pattern.Match(property.ProgrammaticName).Groups["name"].Value;
        }
    }

    public class By
    {
        internal readonly List<ElementFindInfo> FindInfoList = new List<ElementFindInfo>();

        internal By(TreeScope scope, Info info)
        {
            FindInfoList.Add(new ElementFindInfo(scope, info));
        }

        internal By(List<ElementFindInfo> findInfoList)
        {
            FindInfoList = findInfoList;
        }

        #region Create By (static)

        public static By Uid(string value)
        {
            return Uid(TreeScope.Subtree, value);
        }

        public static By Uid(TreeScope scope, string value)
        {
            return AutomationProperty(scope, AutomationElement.AutomationIdProperty, value);
        }

        public static By Name(string value)
        {
            return Name(TreeScope.Subtree, value);
        }

        public static By Name(TreeScope scope, string value)
        {
            return AutomationProperty(scope, AutomationElement.NameProperty, value);
        }

        public static By AutomationProperty(TreeScope scope, AutomationProperty property, object value)
        {
            return new By(scope, new Info(property, value, ConditionType.None));
        }

        #endregion

        public By AndType(ControlType value)
        {
            And(AutomationElement.ControlTypeProperty, value);
            return this;
        }

        public By OrName(string value)
        {
            Or(AutomationElement.NameProperty, value);
            return this;
        }

        public By And(AutomationProperty property, object value)
        {
            AddInfoToLast(property, value, ConditionType.And);
            return this;
        }

        public By Or(AutomationProperty property, object value)
        {
            AddInfoToLast(property, value, ConditionType.Or);
            return this;
        }

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

        private void AddInfoToLast(AutomationProperty property, object value, ConditionType conditionType)
        {
            FindInfoList.Last().Add(new Info(property, value, conditionType));
        }
    }
}
