namespace Cruciatus.Core
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Windows.Automation;

    using Cruciatus.Exceptions;

    #endregion

    internal struct FindInfo
    {
        internal Condition Condition;

        internal TreeScope TreeScope;

        internal FindInfo(TreeScope scope, Condition condition)
        {
            TreeScope = scope;
            Condition = condition;
        }

        internal static FindInfo Create(TreeScope scope, Condition condition)
        {
            return new FindInfo(scope, condition);
        }
    }

    public class By
    {
        internal readonly List<FindInfo> FindInfoList;

        internal By()
        {
            FindInfoList = new List<FindInfo>();
        }

        internal By(TreeScope scope, Condition condition)
        {
            FindInfoList = new List<FindInfo> { FindInfo.Create(scope, condition) };
        }

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
            return new By(scope, new PropertyCondition(property, value));
        }

        public By AndType(ControlType value)
        {
            var condition = new PropertyCondition(AutomationElement.ControlTypeProperty, value);
            var n = FindInfoList.Count;
            if (n != 0)
            {
                var info = FindInfoList[n - 1];
                info.Condition = new AndCondition(info.Condition, condition);
                FindInfoList[n - 1] = info;
                return this;
            }

            FindInfoList.Add(FindInfo.Create(TreeScope.Subtree, condition));
            return this;
        }

        public By OrName(string value)
        {
            var condition = new PropertyCondition(AutomationElement.NameProperty, value);
            var n = FindInfoList.Count;
            if (n != 0)
            {
                var info = FindInfoList[n - 1];
                info.Condition = new OrCondition(info.Condition, condition);
                FindInfoList[n - 1] = info;
                return this;
            }

            FindInfoList.Add(FindInfo.Create(TreeScope.Subtree, condition));
            return this;
        }

        public static By Path(string value)
        {
            var selector = new By();
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
                    throw new CruciatusException("EMPTY AFTER '#' or '%'");
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

                selector.FindInfoList.Add(FindInfo.Create(scope, new PropertyCondition(property, propertyValue)));
            }

            return selector;
        }
    }
}
