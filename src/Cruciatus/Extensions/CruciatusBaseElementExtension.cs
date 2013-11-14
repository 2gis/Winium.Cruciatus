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

    public static class CruciatusBaseElementExtension
    {
        internal static TOut GetPropertyValue<T, TOut>(this BaseElement<T> baseElement, AutomationProperty property)
        {
            try
            {
                return baseElement.Element.GetPropertyValue<TOut>(property);
            }
            catch (NotSupportedException exc)
            {
                // TODO: Исключение вида - элемент не поддерживает желаемое свойство
                var err = string.Format(
                        "Элемент {0} не поддерживает свойство {1}.\n",
                        baseElement.ToString(),
                        property.ProgrammaticName);

                throw new Exception(err);
            }
            catch (InvalidCastException exc)
            {
                var err = string.Format(
                    "При получении значения свойства {0} у элемента {1} произошла ошибка.\n",
                    property,
                    baseElement.ToString());

                throw new Exception(err, exc);
            }
        }
    }
}