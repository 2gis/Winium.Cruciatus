namespace Winium.Cruciatus.Extensions
{
    #region using

    using System;
    using System.Windows;
    using System.Windows.Automation;

    using Winium.Cruciatus.Exceptions;
    using Winium.Cruciatus.Helpers;

    #endregion

    /// <summary>
    /// Набор расширений для объектов AutomationElement.
    /// </summary>
    internal static class AutomationElementExtension
    {
        #region Constants

        private const string OperationCanceledExceptionText = "Could not determine location of item relative to point\n";

        #endregion

        #region Methods

        internal static bool ClickablePointLeft(this AutomationElement currentElement, AutomationElement rectElement)
        {
            try
            {
                Point point;
                if (!AutomationElementHelper.TryGetClickablePoint(currentElement, out point))
                {
                    if (!AutomationElementHelper.TryGetBoundingRectangleCenter(currentElement, out point))
                    {
                        throw new OperationCanceledException(OperationCanceledExceptionText);
                    }
                }

                var rect = rectElement.GetPropertyValue<Rect>(AutomationElement.BoundingRectangleProperty);

                return point.X < rect.Left;
            }
            catch (Exception exc)
            {
                throw new OperationCanceledException(OperationCanceledExceptionText, exc);
            }
        }

        internal static bool ClickablePointOver(this AutomationElement currentElement, AutomationElement rectElement)
        {
            try
            {
                Point point;
                if (!AutomationElementHelper.TryGetClickablePoint(currentElement, out point))
                {
                    if (!AutomationElementHelper.TryGetBoundingRectangleCenter(currentElement, out point))
                    {
                        throw new OperationCanceledException(OperationCanceledExceptionText);
                    }
                }

                var rect = rectElement.GetPropertyValue<Rect>(AutomationElement.BoundingRectangleProperty);

                return point.Y < rect.Top;
            }
            catch (Exception exc)
            {
                throw new OperationCanceledException(OperationCanceledExceptionText, exc);
            }
        }

        internal static bool ClickablePointRight(
            this AutomationElement currentElement, 
            AutomationElement rectElement, 
            ScrollPattern scrollPattern)
        {
            try
            {
                Point point;
                if (!AutomationElementHelper.TryGetClickablePoint(currentElement, out point))
                {
                    if (!AutomationElementHelper.TryGetBoundingRectangleCenter(currentElement, out point))
                    {
                        throw new OperationCanceledException(OperationCanceledExceptionText);
                    }
                }

                var rect = rectElement.GetPropertyValue<Rect>(AutomationElement.BoundingRectangleProperty);

                if (scrollPattern == null || scrollPattern.Current.HorizontalScrollPercent < 0)
                {
                    return point.X > rect.Right;
                }

                return point.X > rect.Right - CruciatusFactory.Settings.ScrollBarWidth;
            }
            catch (Exception exc)
            {
                throw new OperationCanceledException(OperationCanceledExceptionText, exc);
            }
        }

        internal static bool ClickablePointUnder(
            this AutomationElement currentElement, 
            AutomationElement rectElement, 
            ScrollPattern scrollPattern)
        {
            try
            {
                Point point;
                if (!AutomationElementHelper.TryGetClickablePoint(currentElement, out point))
                {
                    if (!AutomationElementHelper.TryGetBoundingRectangleCenter(currentElement, out point))
                    {
                        throw new OperationCanceledException(OperationCanceledExceptionText);
                    }
                }

                var rect = rectElement.GetPropertyValue<Rect>(AutomationElement.BoundingRectangleProperty);

                if (scrollPattern == null || scrollPattern.Current.HorizontalScrollPercent < 0)
                {
                    return point.Y > rect.Bottom;
                }

                return point.Y > rect.Bottom - CruciatusFactory.Settings.ScrollBarHeight;
            }
            catch (Exception exc)
            {
                throw new OperationCanceledException(OperationCanceledExceptionText, exc);
            }
        }

        internal static bool ContainsClickablePoint(
            this AutomationElement externalElement, 
            AutomationElement internalElement)
        {
            try
            {
                Point point;
                if (!AutomationElementHelper.TryGetClickablePoint(internalElement, out point))
                {
                    if (!AutomationElementHelper.TryGetBoundingRectangleCenter(internalElement, out point))
                    {
                        throw new OperationCanceledException(OperationCanceledExceptionText);
                    }
                }

                var externalRect = externalElement.GetPropertyValue<Rect>(AutomationElement.BoundingRectangleProperty);

                return externalRect.Contains(point);
            }
            catch (Exception exc)
            {
                throw new OperationCanceledException(
                    "Could not determine if element is contained by another element\n", 
                    exc);
            }
        }

        internal static T GetPattern<T>(this AutomationElement element, AutomationPattern pattern) where T : class
        {
            object foundPattern;
            if (element.TryGetCurrentPattern(pattern, out foundPattern))
            {
                return (T)foundPattern;
            }

            var msg = string.Format("Element does not support {0}", typeof(T).Name);
            throw new CruciatusException(msg);
        }

        internal static TOut GetPropertyValue<TOut>(this AutomationElement element, AutomationProperty property)
        {
            var obj = element.GetCurrentPropertyValue(property, true);
            if (obj == AutomationElement.NotSupported)
            {
                throw new NotSupportedException();
            }

            if (obj is TOut)
            {
                return (TOut)obj;
            }

            throw new InvalidCastException(obj.GetType().ToString());
        }

        #endregion
    }
}
