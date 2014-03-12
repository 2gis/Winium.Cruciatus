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
        /// <summary>
        /// Ожидать готовности элемента в течении времени по умолчанию.
        /// </summary>
        /// <param name="element">
        /// Текущий элемент, у которого ожидаем готовность.
        /// </param>
        /// <returns>
        /// Значение true если элемент оказался готов до истечения времени ожидания; в противном случае значение - false.
        /// </returns>
        public static bool WaitForElementReady(this AutomationElement element)
        {
            return element.WaitForElementReady(CruciatusFactory.Settings.WaitForReadyTimeout);
        }

        /// <summary>
        /// Ожидать готовности элемента заданное время.
        /// </summary>
        /// <param name="element">
        /// Текущий элемент, у которого ожидаем готовность.
        /// </param>
        /// <param name="milliseconds">
        /// Сколько времени ждать.
        /// </param>
        /// <returns>
        /// Значение true если элемент оказался готов до истечения времени ожидания; в противном случае значение - false.
        /// </returns>
        public static bool WaitForElementReady(this AutomationElement element, int milliseconds)
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
            var timeIsNotUp = windowPattern.WaitForInputIdle(milliseconds);
            stopwatch.Stop();

            // Если результат true и время таймаута не вышло
            if (timeIsNotUp && stopwatch.ElapsedMilliseconds < milliseconds)
            {
                return true;
            }

            // Если результат false и время таймаута вышло
            if (!timeIsNotUp && stopwatch.ElapsedMilliseconds > milliseconds)
            {
                return false;
            }

            // Иначе используем UITesting
            var control = UITestControlFactory.FromNativeElement(element, "UIA");
            return control.WaitForControlReady(milliseconds);
        }

        /// <summary>
        /// Определяет, включает ли элемент точку клика заданного элемента.
        /// </summary>
        /// <param name="externalElement">
        /// Текущий элемент.
        /// </param>
        /// <param name="internalElement">
        /// Проверяемый элемент.
        /// </param>
        /// <returns>
        /// Значение true если элемент включает точку клика заданного элемента; в противном случае значение - false.
        /// </returns>
        /// <exception cref="OperationCanceledException">
        /// Операция прервана из-за ошибки.
        /// </exception>
        public static bool ContainsClickablePoint(this AutomationElement externalElement, AutomationElement internalElement)
        {
            try
            {
                var externalRect = externalElement.GetPropertyValue<System.Windows.Rect>(AutomationElement.BoundingRectangleProperty);
                var internaleRect = internalElement.GetPropertyValue<System.Windows.Point>(AutomationElement.ClickablePointProperty);

                return externalRect.Contains(internaleRect);
            }
            catch (Exception exc)
            {
                throw new OperationCanceledException("Не удалось определить вхождение одного элемента в другой.\n", exc);
            }
        }

        public static bool ClickablePointUnder(this AutomationElement currentElement, AutomationElement rectElement, ScrollPattern scrollPattern = null)
        {
            try
            {
                var point = currentElement.GetPropertyValue<System.Windows.Point>(AutomationElement.ClickablePointProperty);
                var rect = rectElement.GetPropertyValue<System.Windows.Rect>(AutomationElement.BoundingRectangleProperty);

                if (scrollPattern == null || scrollPattern.Current.HorizontalScrollPercent < 0)
                {
                    return point.Y > rect.Bottom;
                }

                return point.Y > rect.Bottom - CruciatusFactory.Settings.ScrollBarHeight;
            }
            catch (Exception exc)
            {
                throw new OperationCanceledException("Не удалось определить расположение элемента относительно точки.\n", exc);
            }
        }

        public static bool ClickablePointOver(this AutomationElement currentElement, AutomationElement rectElement)
        {
            try
            {
                var point = currentElement.GetPropertyValue<System.Windows.Point>(AutomationElement.ClickablePointProperty);
                var rect = rectElement.GetPropertyValue<System.Windows.Rect>(AutomationElement.BoundingRectangleProperty);

                return point.Y < rect.Top;
            }
            catch (Exception exc)
            {
                throw new OperationCanceledException("Не удалось определить расположение элемента относительно точки.\n", exc);
            }
        }

        public static bool ClickablePointRight(this AutomationElement currentElement, AutomationElement rectElement, ScrollPattern scrollPattern = null)
        {
            try
            {
                var point = currentElement.GetPropertyValue<System.Windows.Point>(AutomationElement.ClickablePointProperty);
                var rect = rectElement.GetPropertyValue<System.Windows.Rect>(AutomationElement.BoundingRectangleProperty);

                if (scrollPattern == null || scrollPattern.Current.HorizontalScrollPercent < 0)
                {
                    return point.X > rect.Right;
                }

                return point.X > rect.Right - CruciatusFactory.Settings.ScrollBarWidth;
            }
            catch (Exception exc)
            {
                throw new OperationCanceledException("Не удалось определить расположение элемента относительно точки.\n", exc);
            }
        }

        public static bool ClickablePointLeft(this AutomationElement currentElement, AutomationElement rectElement)
        {
            try
            {
                var point = currentElement.GetPropertyValue<System.Windows.Point>(AutomationElement.ClickablePointProperty);
                var rect = rectElement.GetPropertyValue<System.Windows.Rect>(AutomationElement.BoundingRectangleProperty);

                return point.X < rect.Left;
            }
            catch (Exception exc)
            {
                throw new OperationCanceledException("Не удалось определить расположение элемента относительно точки.\n", exc);
            }
        }

        /// <summary>
        /// Перемещение курсора мыши в центр элемента.
        /// </summary>
        /// <param name="element">
        /// Элемент, в центр которого перемещается курсор мыши.
        /// </param>
        /// <exception cref="OperationCanceledException">
        /// Операция прервана из-за ошибки.
        /// </exception>
        public static void MoveMouseToCenter(this AutomationElement element)
        {
            try
            {
                var windowsPoint = element.GetPropertyValue<System.Windows.Point>(AutomationElement.ClickablePointProperty);
                var clickablePoint = new System.Drawing.Point((int)windowsPoint.X, (int)windowsPoint.Y);

                Mouse.MouseMoveSpeed = CruciatusFactory.Settings.MouseMoveSpeed;
                Mouse.Move(clickablePoint);
            }
            catch (Exception exc)
            {
                throw new OperationCanceledException("Перемещение курсора мыши не удалось.\n", exc);
            }
        }

        /// <summary>
        /// Возвращает значение заданного свойства, приведенное к указанному типу.
        /// </summary>
        /// <param name="element">
        /// Текущий элемент, свойство которого необходимо получить.
        /// </param>
        /// <param name="property">
        /// Свойство, которое необходимо получить.
        /// </param>
        /// <typeparam name="TOut">
        /// Тип значения получаемого свойства.
        /// </typeparam>
        /// <returns>
        /// Значение заданного свойства, приведенное к указанному типу.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Элемент не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// Нельзя привести значение свойства к указанному типу.
        /// </exception>
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

        /// <summary>
        /// Прокручивает содержимое до заданного элемента.
        /// </summary>
        /// <param name="externalElement">
        /// Текущий элемент, содержимое которого будет прокручивать.
        /// </param>
        /// <param name="internalElement">
        /// Элемент до которого прокурчиваем.
        /// </param>
        /// <returns>
        /// Значение true если прокрутили либо в этом не было необходимости; иначе прокрутка не поддерживается, значение - false.
        /// </returns>
        public static bool Scrolling(this AutomationElement externalElement, AutomationElement internalElement)
        {
            if (externalElement.ContainsClickablePoint(internalElement))
            {
                return true;
            }

            var scrollPattern = externalElement.GetCurrentPattern(ScrollPattern.Pattern) as ScrollPattern;
            if (scrollPattern == null)
            {
                return false;
            }

            while (scrollPattern.Current.VerticalScrollPercent > 0)
            {
                scrollPattern.ScrollVertical(ScrollAmount.LargeDecrement);
            }

            do
            {
                scrollPattern.ScrollVertical(ScrollAmount.SmallIncrement);
            }
            while (!externalElement.ContainsClickablePoint(internalElement));

            return true;
        }

        /// <summary>
        /// Прокручивает содержимое до заданного элемента.
        /// </summary>
        /// <param name="externalElement">
        /// Текущий элемент, содержимое которого будет прокручивать.
        /// </param>
        /// <param name="internalElement">
        /// Элемент до которого прокурчиваем.
        /// </param>
        /// <param name="popupWindow">
        /// sf
        /// </param>
        /// <returns>
        /// Значение true если прокрутили либо в этом не было необходимости; иначе прокрутка не поддерживается, значение - false.
        /// </returns>
        public static bool ScrollingForComboBox(this AutomationElement externalElement, AutomationElement internalElement, AutomationElement popupWindow)
        {
            if (popupWindow.ContainsClickablePoint(internalElement))
            {
                return true;
            }

            var scrollPattern = externalElement.GetCurrentPattern(ScrollPattern.Pattern) as ScrollPattern;
            if (scrollPattern == null)
            {
                return false;
            }

            while (scrollPattern.Current.VerticalScrollPercent > 0)
            {
                scrollPattern.ScrollVertical(ScrollAmount.LargeDecrement);
            }

            do
            {
                scrollPattern.ScrollVertical(ScrollAmount.SmallIncrement);
            }
            while (!popupWindow.ContainsClickablePoint(internalElement));

            return true;
        }
    }
}
