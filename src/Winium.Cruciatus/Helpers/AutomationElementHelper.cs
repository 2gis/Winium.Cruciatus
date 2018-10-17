namespace Winium.Cruciatus.Helpers
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Automation;
    using System.Xml.XPath;
    //using NLog;

    using Winium.Cruciatus.Helpers.XPath;

    using Condition = System.Windows.Automation.Condition;

    #endregion

    internal static class AutomationElementHelper
    {
        public struct CursorPoint
        {
            public int X;
            public int Y;
        }
        public enum DeviceCap
        {
            LOGPIXELSX = 88,
            LOGPIXELSY = 90
        }
        #region Methods
        //static readonly Logger Logger = CruciatusFactory.Logger;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool GetPhysicalCursorPos(ref CursorPoint lpPoint);
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        static readonly int DEFAULTDPI = 96;

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

        static void getScreenScalingFactor(ref float ScreenScalingFactorX, ref float ScreenScalingFactorY)
        {
            System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            int Xdpi = GetDeviceCaps(desktop, (int)DeviceCap.LOGPIXELSX);
            int Ydpi = GetDeviceCaps(desktop, (int)DeviceCap.LOGPIXELSY);
            ScreenScalingFactorX = (float)Xdpi / (float)DEFAULTDPI;
            ScreenScalingFactorY = (float)Ydpi / (float)DEFAULTDPI;
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
            float ScreenScalingFactorX = 1;
            float ScreenScalingFactorY = 1;
            getScreenScalingFactor(ref ScreenScalingFactorX, ref ScreenScalingFactorY);
            point.X = rect.X / ScreenScalingFactorX;
            point.Y = rect.Y / ScreenScalingFactorY;
            CruciatusFactory.Logger.Debug("BoundingRectangle location before scaling X co-ordinate:" + rect.X + "Y co-ordinate: " + rect.Y);
            CruciatusFactory.Logger.Debug("BoundingRectangle Points after scaling X co-ordinate:" + point.X + "Y co-ordinate: " + point.Y);
            CruciatusFactory.Logger.Debug("BoundingRectangle width :" + rect.Width);
            CruciatusFactory.Logger.Debug("BoundingRectangle Height :" + rect.Height);
            point.Offset((rect.Width / ScreenScalingFactorX) / 2, (rect.Height / ScreenScalingFactorY) / 2);
            return true;
        }

        internal static bool TryGetClickablePoint(AutomationElement element, out Point point)
        {
            return element.TryGetClickablePoint(out point);
        }

        #endregion
    }
}
