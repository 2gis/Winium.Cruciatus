// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CruciatusCommand.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет команды фреймворка Cruciatus.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;

    using Cruciatus.Core;
    using Cruciatus.Elements;

    using NLog;

    using Keyboard = Cruciatus.Core.Keyboard;
    using Mouse = Cruciatus.Core.Mouse;
    using Point = System.Windows.Point;

    #endregion

    public static class CruciatusCommand
    {
        private static readonly Logger Logger = CruciatusFactory.Logger;

        public static readonly Mouse Mouse = new Mouse();

        public static readonly Keyboard Keyboard = new Keyboard();

        public static bool TryClickOnClickablePoint(MouseButtons button, CruciatusElement element, bool doubleClick)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            Point point;
            if (!AutomationElementHelper.TryGetClickablePoint(element.Instanse, out point))
            {
                Logger.Debug("Element '{0}' not have ClickablePoint", element);
                return false;
            }

            if (doubleClick)
            {
                Mouse.DoubleClick(button, point.X, point.Y);
            }
            else
            {
                Mouse.Click(button, point.X, point.Y);
            }

            Logger.Info("{0} on '{1}' element at ({2}, {3}) ClickablePoint",
                        doubleClick ? "DoubleClick" : "Click", element, point.X, point.Y);
            return true;
        }

        public static bool TryClickOnBoundingRectangleCenter(MouseButtons button, CruciatusElement element, bool doubleClick)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            Point point;
            if (!AutomationElementHelper.TryGetBoundingRectangleCenter(element.Instanse, out point))
            {
                Logger.Debug("Element '{0}' have empty BoundingRectangle", element);
                return false;
            }

            if (doubleClick)
            {
                Mouse.DoubleClick(button, point.X, point.Y);
            }
            else
            {
                Mouse.Click(button, point.X, point.Y);
            }

            Logger.Info("{0} on '{1}' element at ({2}, {3}) BoundingRectangle center",
                        doubleClick ? "DoubleClick" : "Click", element, point.X, point.Y);
            return true;
        }

        public static bool TryClickUsingInvokePattern(CruciatusElement element, bool doubleClick)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            object basePattern;
            if (element.Instanse.TryGetCurrentPattern(InvokePattern.Pattern, out basePattern))
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

        public static bool TryGetTextUsingTextPattern(CruciatusElement element, out string text)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            object pattern;
            if (element.Instanse.TryGetCurrentPattern(TextPattern.Pattern, out pattern))
            {
                var textPattern = pattern as TextPattern;
                if (textPattern != null)
                {
                    text = textPattern.DocumentRange.GetText(-1);
                    Logger.Info("Element '{0}' return text using TextPattern", element);
                    return true;
                }
            }

            Logger.Debug("Element ({0}) not support TextPattern", element);
            text = null;
            return false;
        }

        public static bool TryGetTextUsingValuePattern(CruciatusElement element, out string text)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            object pattern;
            if (element.Instanse.TryGetCurrentPattern(ValuePattern.Pattern, out pattern))
            {
                var valuePattern = pattern as ValuePattern;
                if (valuePattern != null)
                {
                    Logger.Info("Element '{0}' return text with use ValuePattern", element);
                    text = valuePattern.Current.Value;
                    return true;
                }
            }

            Logger.Debug("Element ({0}) not support ValuePattern", element);
            text = null;
            return false;
        }

        public static CruciatusElement FindFirst(CruciatusElement parent, By selector)
        {
            return FindFirst(parent, selector, CruciatusFactory.Settings.SearchTimeout);
        }

        public static CruciatusElement FindFirst(CruciatusElement parent, By selector, int timeout)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            var element = FindFirst(parent.Instanse, selector, timeout);
            if (element == null)
            {
                return null;
            }

            return new CruciatusElement(parent.Instanse, element, selector);
        }

        public static IEnumerable<CruciatusElement> FindAll(CruciatusElement parent, By selector)
        {
            return FindAll(parent, selector, CruciatusFactory.Settings.SearchTimeout);
        }

        public static IEnumerable<CruciatusElement> FindAll(CruciatusElement parent, By selector, int timeout)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            var element = parent.Instanse;
            var info = selector.FindInfoList;
            for (var i = 0; i < info.Count - 1; ++i)
            {
                element = AutomationElementHelper.FindFirst(element, info[i].TreeScope, info[i].Condition, timeout);
                if (element == null)
                {
                    return Enumerable.Empty<CruciatusElement>();
                }
            }

            var lastIinfo = selector.FindInfoList.Last();
            var result = AutomationElementHelper.FindAll(element, lastIinfo.TreeScope, lastIinfo.Condition);
            return result.Select(e => new CruciatusElement(parent.Instanse, e, selector));
        }

        internal static AutomationElement FindFirst(AutomationElement parent, By selector)
        {
            return FindFirst(parent, selector, CruciatusFactory.Settings.SearchTimeout);
        }

        internal static AutomationElement FindFirst(AutomationElement parent, By selector, int timeout)
        {
            var element = parent;
            foreach (var info in selector.FindInfoList)
            {
                element = AutomationElementHelper.FindFirst(element, info.TreeScope, info.Condition, timeout);
                if (element == null)
                {
                    return null;
                }
            }

            return element;
        }
    }
}
