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
    using System;
    using System.Windows;
    using System.Windows.Automation;

    using Cruciatus.Elements;
    using Cruciatus.Exceptions;

    using MessageBox = Cruciatus.MessageBox;

    public static class CruciatusElementExtension
    {
        /// <summary>
        /// Закрывает диалоговое окно MessageBox.
        /// </summary>
        /// <param name="parent">
        /// Элемент, являющийся родительским для диалогового окна.
        /// </param>
        /// <param name="buttonsType">
        /// Тип набора кнопок в диалоговом окне.
        /// </param>
        /// <param name="button">
        /// Кнопка, которой будет производиться закрытие диалогового окна.
        /// </param>
        /// <returns>
        /// Значение true если закрыть удалось; в противном случае значение - false.
        /// </returns>
        public static bool CloseMessageBox(this CruciatusElement parent, MessageBoxButton buttonsType, MessageBoxResult button)
        {
            return MessageBox.ClickButton(parent, buttonsType, button);
        }

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
