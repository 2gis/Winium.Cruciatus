namespace Winium.Cruciatus.Helpers
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Automation;
    using System.Xml.XPath;

    using Winium.Cruciatus.Helpers.XPath;

    using Condition = System.Windows.Automation.Condition;

    #endregion

    internal static class AutomationElementHelper
    {
        #region Methods

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

        internal static IEnumerable<AutomationElement> FindAll(AutomationElement parent, string xpath, int timeout)
        {
            var navigator = new DesktopTreeXPathNavigator(parent);
            var dtn = DateTime.Now.AddMilliseconds(timeout);

            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while (DateTime.Now <= dtn)
            {
                var obj = navigator.Evaluate(xpath);
                var nodeIterator = obj as XPathNodeIterator;
                if (nodeIterator == null)
                {
                    CruciatusFactory.Logger.Warn("XPath expression '{0}' not searching nodes", xpath);
                    break;
                }

                var nodes = nodeIterator.Cast<DesktopTreeXPathNavigator>();
                var elementNodes = nodes.Where(item => item.NodeType == XPathNodeType.Element).ToList();
                if (elementNodes.Any())
                {
                    return elementNodes.Select(item => (AutomationElement)item.TypedValue);
                }
            }

            return Enumerable.Empty<AutomationElement>();
        }

        internal static AutomationElement FindFirst(AutomationElement parent, TreeScope scope, Condition condition)
        {
            return FindFirst(parent, scope, condition, CruciatusFactory.Settings.SearchTimeout);
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
