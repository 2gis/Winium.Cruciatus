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
    using System.Configuration;
    using System.Linq;
    using System.Windows.Automation;

    using Cruciatus.Elements;

    public static class CruciatusBaseElementExtension
    {
        internal static TOut GetPropertyValue<T, TOut>(this BaseElement<T> baseElement, AutomationProperty property)
        {
            var obj = baseElement.Element.GetCurrentPropertyValue(property, true);
            if (obj == AutomationElement.NotSupported)
            {
                // TODO: Исключение вида - элемент не поддерживает желаемое свойство
                throw new Exception(string.Format("Элемент {0} не поддерживает свойство {1}.\n", baseElement.ToString(), property.ProgrammaticName));
            }

            if ((obj is TOut) == false)
            {
                var err = string.Format("При получении значения свойства {0} у элемента {1} произошла ошибка: ", property, baseElement.ToString());
                err += string.Format("тип значения не соответствует ожидаемому.\n");
                throw new Exception(err);
            }

            return (TOut)obj;
        }
    }
}