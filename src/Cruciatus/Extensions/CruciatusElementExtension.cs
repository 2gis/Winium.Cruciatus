// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CruciatusElementExtension.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет расширения для элементов, наследующихся от CruciatusElement.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus.Extensions
{
    #region using

    using System;
    using System.Windows.Automation;

    using Cruciatus.Elements;
    using Cruciatus.Exceptions;

    #endregion

    public static class CruciatusElementExtension
    {
        /// <summary>
        /// Возвращает значение заданного свойства, приведенное к указанному типу.
        /// </summary>
        /// <param name="cruciatusElement">
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
        /// <exception cref="PropertyNotSupportedException">
        /// Элемент не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// Нельзя привести значение свойства к указанному типу.
        /// </exception>
        internal static TOut GetPropertyValue<TOut>(this CruciatusElement cruciatusElement, AutomationProperty property)
        {
            try
            {
                return cruciatusElement.Element.GetPropertyValue<TOut>(property);
            }
            catch (NotSupportedException)
            {
                throw new PropertyNotSupportedException(cruciatusElement.ToString(), property.ProgrammaticName);
            }
            catch (InvalidCastException exc)
            {
                var err = string.Format(
                    "При получении значения свойства {0} у элемента {1} произошла ошибка. ", 
                    property, 
                    cruciatusElement.ToString());
                err += exc.Message;

                throw new PropertyInvalidCastException(err);
            }
        }
    }
}
