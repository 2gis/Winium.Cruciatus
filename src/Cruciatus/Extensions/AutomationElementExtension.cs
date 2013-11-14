// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutomationElementExtension.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет расширения для AutomationElement.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus.Extensions
{
    using System;
    using System.Diagnostics;
    using System.Windows.Automation;

    using Microsoft.VisualStudio.TestTools.UITesting;

    public static class AutomationElementExtension
    {
        private const int MouseMoveSpeed = 2500;

        private const int WaitForReadyTimeout = 5000;

        public static bool WaitForElementReady(this AutomationElement element)
        {
            var walker = new TreeWalker(Condition.TrueCondition);
            AutomationElement parent = element;
            WindowPattern windowPattern = null;
            while (parent != null)
            {
                object pattern;
                if (parent.TryGetCurrentPattern(WindowPattern.Pattern, out pattern))
                {
                    windowPattern = (WindowPattern)pattern;
                    break;
                }

                parent = walker.GetParent(parent);
            }

            if (windowPattern == null)
            {
                // Теоретически такой ситуации не может быть
                // но если что, то считаем, что все ок
                return true;
            }

            // результат от WaitForInputIdle желательно проверить самостоятельно
            // ошибка при возврате false точно встречается
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var timeIsNotUp = windowPattern.WaitForInputIdle(WaitForReadyTimeout);
            stopwatch.Stop();

            // Если результат true и время таймаута не вышло
            if (timeIsNotUp && stopwatch.ElapsedMilliseconds < WaitForReadyTimeout)
            {
                return true;
            }

            // Если результат false и время таймаута вышло
            if (!timeIsNotUp && stopwatch.ElapsedMilliseconds > WaitForReadyTimeout)
            {
                return false;
            }

            // Иначе используем UITesting
            var control = UITestControlFactory.FromNativeElement(element, "UIA");
            return control.WaitForControlReady(WaitForReadyTimeout);
        }

        public static bool GeometricallyContains(this AutomationElement externalElement, AutomationElement internalElement)
        {
            try
            {
                var externalRect = externalElement.GetPropertyValue<System.Windows.Rect>(AutomationElement.BoundingRectangleProperty);
                var internaleRect = internalElement.GetPropertyValue<System.Windows.Rect>(AutomationElement.BoundingRectangleProperty);

                return externalRect.Contains(internaleRect);
            }
            catch (Exception exc)
            {
                throw new OperationCanceledException("Не удалось определить вхождение одного элемента в другой.\n", exc);
            }
        }

        public static void MoveMouseToCenter(this AutomationElement element)
        {
            try
            {
                var windowsPoint = element.GetPropertyValue<System.Windows.Point>(AutomationElement.ClickablePointProperty);
                var clickablePoint = new System.Drawing.Point((int)windowsPoint.X, (int)windowsPoint.Y);

                Mouse.MouseMoveSpeed = MouseMoveSpeed;
                Mouse.Move(clickablePoint);
            }
            catch (Exception exc)
            {
                throw new OperationCanceledException("Перемещение курсора мыши не удалось.\n", exc);
            }
        }

        public static AutomationElement SearchSpecificElementConsideringScroll<T>(
            this AutomationElement element,
            Func<AutomationElement, T> findFunc,
            Func<T, bool> compareFunc,
            Func<T, AutomationElement> getAutomationElementFunc)
            where T : class
        {
            T searchElement;
            var scrollPattern = element.GetCurrentPattern(ScrollPattern.Pattern) as ScrollPattern;
            if (scrollPattern != null)
            {
                element.MoveMouseToCenter();

                scrollPattern.SetScrollPercent(scrollPattern.Current.HorizontalScrollPercent, 0);

                searchElement = findFunc(element);
                while (compareFunc(searchElement) && scrollPattern.Current.VerticalScrollPercent < 100)
                {
                    scrollPattern.ScrollVertical(ScrollAmount.LargeIncrement);

                    // TODO: Делать что-нибудь если false?
                    element.WaitForElementReady();

                    searchElement = findFunc(element);
                }

                if (!compareFunc(searchElement))
                {
                    while (!element.GeometricallyContains(getAutomationElementFunc(searchElement)))
                    {
                        scrollPattern.ScrollVertical(ScrollAmount.SmallIncrement);
                    }
                }
            }
            else
            {
                searchElement = findFunc(element);
            }

            return getAutomationElementFunc(searchElement);
        }

        public static TOut GetPropertyValue<TOut>(this AutomationElement element, AutomationProperty property)
        {
            var obj = element.GetCurrentPropertyValue(property, true);
            if (obj == AutomationElement.NotSupported)
            {
                var err = string.Format("Элемент не поддерживает свойство {0}.\n", property.ProgrammaticName);
                throw new NotSupportedException(err);
            }

            if ((obj is TOut) == false)
            {
                var err = string.Format("Преобразование {0} к {1} не поддерживается.\n", obj.GetType(), typeof(TOut));
                throw new InvalidCastException(err);
            }

            return (TOut)obj;
        }
    }
}
