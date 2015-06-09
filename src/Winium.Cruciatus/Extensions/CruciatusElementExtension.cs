namespace Winium.Cruciatus.Extensions
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Automation;

    using WindowsInput.Native;

    using Winium.Cruciatus.Core;
    using Winium.Cruciatus.Elements;
    using Winium.Cruciatus.Exceptions;
    using Winium.Cruciatus.Settings;

    #endregion

    /// <summary>
    /// Набор расширений для объектов CruciatusElement.
    /// </summary>
    public static class CruciatusElementExtension
    {
        #region Public Methods and Operators

        /// <summary>
        /// Кликнуть по элементу с зажатой кнопкой Control
        /// </summary>
        public static void ClickWithPressedCtrl(this CruciatusElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            var keyboardSimulatorExt =
                (KeyboardSimulatorExt)
                CruciatusFactory.GetSpecificKeyboard(KeyboardSimulatorType.BasedOnInputSimulatorLib);
            keyboardSimulatorExt.KeyDown(VirtualKeyCode.CONTROL);
            element.Click();
            keyboardSimulatorExt.KeyUp(VirtualKeyCode.CONTROL);
        }

        /// <summary>
        /// Клик по элементу с нажатыми кнопками (Ctrl, Shift, etc)
        /// </summary>
        /// <param name="element"></param>
        /// <param name="keys">Клавиши для "зажатия"</param>
        public static void ClickWithPressedKeys(this CruciatusElement element, List<VirtualKeyCode> keys)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            var keyboardSimulatorExt =
                (KeyboardSimulatorExt)
                CruciatusFactory.GetSpecificKeyboard(KeyboardSimulatorType.BasedOnInputSimulatorLib);

            keys.ForEach(key => keyboardSimulatorExt.KeyDown(key));
            element.Click();
            keys.ForEach(key => keyboardSimulatorExt.KeyUp(key));
        }

        /// <summary>
        /// Получает у элемента значение заданного свойства.
        /// </summary>
        /// <param name="cruciatusElement">
        /// Экземпляр элемента.
        /// </param>
        /// <param name="property">
        /// Целевое свойство.
        /// </param> 
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", 
            Justification = "First parameter in extension cannot be null.")]
        public static TOut GetAutomationPropertyValue<TOut>(
            this CruciatusElement cruciatusElement, 
            AutomationProperty property)
        {
            try
            {
                return cruciatusElement.Instance.GetPropertyValue<TOut>(property);
            }
            catch (NotSupportedException)
            {
                var msg = string.Format("Element '{0}' not support '{1}'", cruciatusElement, property.ProgrammaticName);
                CruciatusFactory.Logger.Error(msg);

                throw new CruciatusException("GET PROPERTY VALUE FAILED");
            }
            catch (InvalidCastException invalidCastException)
            {
                var msg = string.Format("Invalid cast from '{0}' to '{1}'.", invalidCastException.Message, typeof(TOut));
                CruciatusFactory.Logger.Error(msg);

                throw new CruciatusException("GET PROPERTY VALUE FAILED");
            }
        }

        /// <summary>
        /// Преобразовать элемент в CheckBox.
        /// </summary>
        public static CheckBox ToCheckBox(this CruciatusElement element)
        {
            return new CheckBox(element);
        }

        /// <summary>
        /// Преобразовать элемент в CheckBox.
        /// </summary>
        public static ComboBox ToComboBox(this CruciatusElement element)
        {
            return new ComboBox(element);
        }

        /// <summary>
        /// Преобразовать элемент в ListBox.
        /// </summary>
        public static ListBox ToListBox(this CruciatusElement element)
        {
            return new ListBox(element);
        }

        /// <summary>
        /// Преобразовать элемент в Menu.
        /// </summary>
        public static Menu ToMenu(this CruciatusElement element)
        {
            return new Menu(element);
        }

        #endregion
    }
}
