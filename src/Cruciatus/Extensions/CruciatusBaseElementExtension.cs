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
