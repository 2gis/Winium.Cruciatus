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

        /// <summary>
        /// Ожидать готовности элемента заданное время или время по умолчанию.
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
        public static bool WaitForElementReady(this AutomationElement element, int milliseconds = WaitForReadyTimeout)
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
        /// Проверка предположения, что заданный элемент геометрически внутри текущего.
        /// </summary>
        /// <param name="externalElement">
        /// Текущий элемент.
        /// </param>
        /// <param name="internalElement">
        /// Проверяемый элемент.
        /// </param>
        /// <returns>
        /// Значение true если предположение верно; в противном случае значение - false.
        /// </returns>
        /// <exception cref="OperationCanceledException">
        /// Операция прервана из-за ошибки.
        /// </exception>
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
                // Запоминаем начальное состояние прокрутки
                var startVerticalScrollPercent = scrollPattern.Current.VerticalScrollPercent;

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

                // Возвращаем начальное состояние прокрутки
                scrollPattern.SetScrollPercent(scrollPattern.Current.HorizontalScrollPercent, startVerticalScrollPercent);
            }
            else
            {
                searchElement = findFunc(element);
            }

            return getAutomationElementFunc(searchElement);
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
            if (externalElement.GeometricallyContains(internalElement))
            {
                return true;
            }

            var scrollPattern = externalElement.GetCurrentPattern(ScrollPattern.Pattern) as ScrollPattern;
            if (scrollPattern == null)
            {
                return false;
            }

            do
            {
                scrollPattern.ScrollVertical(ScrollAmount.SmallIncrement);
            }
            while (!externalElement.GeometricallyContains(internalElement));

            return true;
        }
    }
}
