namespace Winium.Cruciatus.Core
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Automation;

    using Condition = System.Windows.Automation.Condition;

    #endregion

    internal static class AutomationElementHelper
    {
        #region Constants

        private const int FindTimeout = 500;

        #endregion

        #region Methods

        internal static IEnumerable<AutomationElement> FindAll(
            AutomationElement parent, 
            TreeScope scope, 
            Condition condition)
        {
            return FindAll(parent, scope, condition, FindTimeout);
        }

        internal static IEnumerable<AutomationElement> FindAll(
            AutomationElement parent, 
            TreeScope scope, 
            Condition condition, 
            int timeout)
        {
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

        internal static AutomationElement FindFirst(AutomationElement parent, TreeScope scope, Condition condition)
        {
            return FindFirst(parent, scope, condition, FindTimeout);
        }

        internal static AutomationElement FindFirst(
            AutomationElement parent, 
            TreeScope scope, 
            Condition condition, 
            int timeout)
        {
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

        internal static bool TryGetBoundingRectangleCenter(AutomationElement element, out Point point)
        {
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

        internal static bool TryGetClickablePoint(AutomationElement element, out Point point)
        {
            return element.TryGetClickablePoint(out point);
        }

        #endregion
    }
}
