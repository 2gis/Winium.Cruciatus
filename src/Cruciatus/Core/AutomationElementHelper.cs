namespace Cruciatus.Core
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Automation;

    using Condition = System.Windows.Automation.Condition;

    #endregion

    public static class AutomationElementHelper
    {
        private const int FindTimeout = 500;

        public static AutomationElement FindFirst(AutomationElement parent, TreeScope scope, Condition condition)
        {
            return FindFirst(parent, scope, condition, FindTimeout);
        }

        public static AutomationElement FindFirst(AutomationElement parent, TreeScope scope, Condition condition,
                                                  int timeout)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            var dtn = DateTime.Now.AddMilliseconds(timeout);

            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while (DateTime.Now <= dtn)
            {
                var element = parent.FindFirst(scope, condition);
                if (element != null)
                {
                    return element;
                }
            }

            return null;
        }

        public static IEnumerable<AutomationElement> FindAll(AutomationElement parent, TreeScope scope,
                                                             Condition condition)
        {
            return FindAll(parent, scope, condition, FindTimeout);
        }

        public static IEnumerable<AutomationElement> FindAll(AutomationElement parent, TreeScope scope,
                                                             Condition condition, int timeout)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            var dtn = DateTime.Now.AddMilliseconds(timeout);

            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while (DateTime.Now <= dtn)
            {
                var elements = parent.FindAll(scope, condition);
                if (elements.Count > 0)
                {
                    return elements.Cast<AutomationElement>();
                }
            }

            return Enumerable.Empty<AutomationElement>();
        }

        public static bool TryGetClickablePoint(AutomationElement element, out Point point)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return element.TryGetClickablePoint(out point);
        }

        public static bool TryGetBoundingRectangleCenter(AutomationElement element, out Point point)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            var rect = element.Current.BoundingRectangle;
            if (rect.IsEmpty)
            {
                point = new Point();
                return false;
            }

            point = rect.Location;
            point.Offset(rect.Width / 2, rect.Height / 2);
            return true;
        }
    }
}
