namespace Cruciatus
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Automation;

    using Cruciatus.Core;
    using Cruciatus.Elements;
    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;

    using NLog;

    #endregion

    internal static class CruciatusCommand
    {
        #region Static Fields

        private static readonly Logger Logger = CruciatusFactory.Logger;

        #endregion

        #region Methods

        internal static IEnumerable<CruciatusElement> FindAll(CruciatusElement parent, By getStrategy)
        {
            return FindAll(parent, getStrategy, CruciatusFactory.Settings.SearchTimeout);
        }

        internal static IEnumerable<CruciatusElement> FindAll(CruciatusElement parent, By getStrategy, int timeout)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            var element = parent.Instance;
            var info = getStrategy.FindInfoList;
            for (var i = 0; i < info.Count - 1; ++i)
            {
                element = AutomationElementHelper.FindFirst(element, info[i].TreeScope, info[i].Condition, timeout);
                if (element == null)
                {
                    Logger.Error("Element '{0}' not found", info);
                    CruciatusFactory.Screenshoter.AutomaticScreenshotCaptureIfNeeded();
                    throw new CruciatusException("ELEMENT NOT FOUND");
                }
            }

            var lastIinfo = getStrategy.FindInfoList.Last();
            var result = AutomationElementHelper.FindAll(element, lastIinfo.TreeScope, lastIinfo.Condition);
            return result.Select(e => new CruciatusElement(parent, e, getStrategy));
        }

        internal static CruciatusElement FindFirst(CruciatusElement parent, By getStrategy)
        {
            return FindFirst(parent, getStrategy, CruciatusFactory.Settings.SearchTimeout);
        }

        internal static CruciatusElement FindFirst(CruciatusElement parent, By getStrategy, int timeout)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            var element = FindFirst(parent.Instance, getStrategy, timeout);
            return element == null ? null : new CruciatusElement(parent, element, getStrategy);
        }

        internal static AutomationElement FindFirst(AutomationElement parent, By getStrategy)
        {
            return FindFirst(parent, getStrategy, CruciatusFactory.Settings.SearchTimeout);
        }

        internal static AutomationElement FindFirst(AutomationElement parent, By getStrategy, int timeout)
        {
            var element = parent;
            foreach (var info in getStrategy.FindInfoList)
            {
                element = AutomationElementHelper.FindFirst(element, info.TreeScope, info.Condition, timeout);
                if (element == null)
                {
                    Logger.Info("Element '{0}' not found", info);
                    CruciatusFactory.Screenshoter.AutomaticScreenshotCaptureIfNeeded();
                    return null;
                }
            }

            return element;
        }

        internal static bool TryClickOnBoundingRectangleCenter(
            MouseButton button, 
            CruciatusElement element, 
            bool doubleClick)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            Point point;
            if (!AutomationElementHelper.TryGetBoundingRectangleCenter(element.Instance, out point))
            {
                Logger.Debug("Element '{0}' have empty BoundingRectangle", element);
                return false;
            }

            if (doubleClick)
            {
                CruciatusFactory.Mouse.DoubleClick(button, point.X, point.Y);
            }
            else
            {
                CruciatusFactory.Mouse.Click(button, point.X, point.Y);
            }

            Logger.Info(
                "{0} on '{1}' element at ({2}, {3}) BoundingRectangle center", 
                doubleClick ? "DoubleClick" : "Click", 
                element, 
                point.X, 
                point.Y);
            return true;
        }

        internal static bool TryClickOnClickablePoint(MouseButton button, CruciatusElement element, bool doubleClick)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            var point = element.Properties.ClickablePoint;
            if (!point.HasValue)
            {
                Logger.Debug("Element '{0}' not have ClickablePoint", element);
                return false;
            }

            var x = point.Value.X;
            var y = point.Value.Y;
            if (doubleClick)
            {
                CruciatusFactory.Mouse.DoubleClick(button, x, y);
            }
            else
            {
                CruciatusFactory.Mouse.Click(button, x, y);
            }

            Logger.Info(
                "{0} on '{1}' element at ({2}, {3}) ClickablePoint", 
                doubleClick ? "DoubleClick" : "Click", 
                element, 
                x, 
                y);
            return true;
        }

        internal static bool TryClickUsingInvokePattern(CruciatusElement element, bool doubleClick)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            object basePattern;
            if (element.Instance.TryGetCurrentPattern(InvokePattern.Pattern, out basePattern))
            {
                string cmd;
                var invokePattern = (InvokePattern)basePattern;
                if (doubleClick)
                {
                    invokePattern.Invoke();
                    invokePattern.Invoke();
                    cmd = "DoubleClick";
                }
                else
                {
                    invokePattern.Invoke();
                    cmd = "Click";
                }

                Logger.Info("{0} emulation on '{1}' element with use invoke pattern", cmd, element);
                return true;
            }

            Logger.Debug("Element '{0}' not support InvokePattern", element);
            return false;
        }

        internal static bool TryGetTextUsingTextPattern(CruciatusElement element, out string text)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            object pattern;
            if (element.Instance.TryGetCurrentPattern(TextPattern.Pattern, out pattern))
            {
                var textPattern = pattern as TextPattern;
                if (textPattern != null)
                {
                    text = textPattern.DocumentRange.GetText(-1);
                    Logger.Info("Element '{0}' return text using TextPattern", element);
                    return true;
                }
            }

            Logger.Debug("Element '{0}' not support TextPattern", element);
            text = null;
            return false;
        }

        internal static bool TryGetTextUsingValuePattern(CruciatusElement element, out string text)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            object pattern;
            if (element.Instance.TryGetCurrentPattern(ValuePattern.Pattern, out pattern))
            {
                var valuePattern = pattern as ValuePattern;
                if (valuePattern != null)
                {
                    Logger.Info("Element '{0}' return text with use ValuePattern", element);
                    text = valuePattern.Current.Value;
                    return true;
                }
            }

            Logger.Debug("Element '{0}' not support ValuePattern", element);
            text = null;
            return false;
        }

        #endregion
    }
}
