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
    #region using

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Automation;

    using Cruciatus.Core;

    #endregion

    public static class AutomationElementExtension
    {
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
        public static bool ContainsClickablePoint(this AutomationElement externalElement, 
                                                  AutomationElement internalElement)
        {
            try
            {
                Point point;
                if (!AutomationElementHelper.TryGetClickablePoint(internalElement, out point))
                {
                    if (!AutomationElementHelper.TryGetBoundingRectangleCenter(internalElement, out point))
                    {
                        throw new OperationCanceledException(
                            "Не удалось определить расположение элемента относительно точки.\n");
                    }
                }

                var externalRect = externalElement.GetPropertyValue<Rect>(AutomationElement.BoundingRectangleProperty);
                
                return externalRect.Contains(point);
            }
            catch (Exception exc)
            {
                throw new OperationCanceledException("Не удалось определить вхождение одного элемента в другой.\n", exc);
            }
        }

        public static bool ClickablePointUnder(this AutomationElement currentElement, AutomationElement rectElement)
        {
            return ClickablePointUnder(currentElement, rectElement, null);
        }

        public static bool ClickablePointUnder(this AutomationElement currentElement, AutomationElement rectElement, 
                                               ScrollPattern scrollPattern)
        {
            try
            {
                Point point;
                if (!AutomationElementHelper.TryGetClickablePoint(currentElement, out point))
                {
                    if (!AutomationElementHelper.TryGetBoundingRectangleCenter(currentElement, out point))
                    {
                        throw new OperationCanceledException(
                            "Не удалось определить расположение элемента относительно точки.\n");
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
                throw new OperationCanceledException(
                    "Не удалось определить расположение элемента относительно точки.\n", exc);
            }
        }

        public static bool ClickablePointOver(this AutomationElement currentElement, AutomationElement rectElement)
        {
            try
            {
                Point point;
                if (!AutomationElementHelper.TryGetClickablePoint(currentElement, out point))
                {
                    if (!AutomationElementHelper.TryGetBoundingRectangleCenter(currentElement, out point))
                    {
                        throw new OperationCanceledException(
                            "Не удалось определить расположение элемента относительно точки.\n");
                    }
                }

                var rect = rectElement.GetPropertyValue<Rect>(AutomationElement.BoundingRectangleProperty);

                return point.Y < rect.Top;
            }
            catch (Exception exc)
            {
                throw new OperationCanceledException(
                    "Не удалось определить расположение элемента относительно точки.\n", exc);
            }
        }

        public static bool ClickablePointRight(this AutomationElement currentElement, AutomationElement rectElement)
        {
            return ClickablePointRight(currentElement, rectElement, null);
        }

        public static bool ClickablePointRight(this AutomationElement currentElement, AutomationElement rectElement, 
                                               ScrollPattern scrollPattern)
        {
            try
            {
                Point point;
                if (!AutomationElementHelper.TryGetClickablePoint(currentElement, out point))
                {
                    if (!AutomationElementHelper.TryGetBoundingRectangleCenter(currentElement, out point))
                    {
                        throw new OperationCanceledException(
                            "Не удалось определить расположение элемента относительно точки.\n");
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
                throw new OperationCanceledException(
                    "Не удалось определить расположение элемента относительно точки.\n", exc);
            }
        }

        public static bool ClickablePointLeft(this AutomationElement currentElement, AutomationElement rectElement)
        {
            try
            {
                Point point;
                if (!AutomationElementHelper.TryGetClickablePoint(currentElement, out point))
                {
                    if (!AutomationElementHelper.TryGetBoundingRectangleCenter(currentElement, out point))
                    {
                        throw new OperationCanceledException(
                            "Не удалось определить расположение элемента относительно точки.\n");
                    }
                }

                var rect = rectElement.GetPropertyValue<Rect>(AutomationElement.BoundingRectangleProperty);

                return point.X < rect.Left;
            }
            catch (Exception exc)
            {
                throw new OperationCanceledException(
                    "Не удалось определить расположение элемента относительно точки.\n", exc);
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
        [SuppressMessage("Microsoft.Design", 
            "CA1062:Validate arguments of public methods", 
            Justification = "First parameter in extension cannot be null.")]
        public static TOut GetPropertyValue<TOut>(this AutomationElement element, AutomationProperty property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

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
