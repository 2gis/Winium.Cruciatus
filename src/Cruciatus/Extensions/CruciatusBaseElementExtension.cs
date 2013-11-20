// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CruciatusBaseElementExtension.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет расширения для элементов, наследующихся от BaseElement.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus.Extensions
{
    using System;
    using System.Windows.Automation;

    using Cruciatus.Elements;
    using Cruciatus.Exceptions;

    public static class CruciatusBaseElementExtension
    {
        /// <summary>
        /// Возвращает значение заданного свойства, приведенное к указанному типу.
        /// </summary>
        /// <param name="baseElement">
        /// Текущий элемент, свойство которого необходимо получить.
        /// </param>
        /// <param name="property">
        /// Свойство, которое необходимо получить.
        /// </param>
        /// <typeparam name="T">
        /// Тип элемента Круциатуса.
        /// </typeparam>
        /// <typeparam name="TOut">
        /// Тип значения получаемого свойства.
        /// </typeparam>
        /// <returns>
        /// Значение заданного свойства, приведенное к указанному типу.
        /// </returns>
        /// <exception cref="PropertyNotSupportedException">
        /// Элемент не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// Нельзя привести значение свойства к указанному типу.
        /// </exception>
        internal static TOut GetPropertyValue<T, TOut>(this BaseElement<T> baseElement, AutomationProperty property)
        {
            try
            {
                return baseElement.Element.GetPropertyValue<TOut>(property);
            }
            catch (NotSupportedException)
            {
                throw new PropertyNotSupportedException(baseElement.ToString(), property.ProgrammaticName);
            }
            catch (InvalidCastException exc)
            {
                var err = string.Format(
                    "При получении значения свойства {0} у элемента {1} произошла ошибка. ",
                    property,
                    baseElement.ToString());
                err += exc.Message;

                throw new InvalidCastException(err);
            }
        }
    }
}
