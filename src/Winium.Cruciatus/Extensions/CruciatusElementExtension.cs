namespace Winium.Cruciatus.Extensions
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Automation;

    using WindowsInput.Native;

    using Winium.Cruciatus.Elements;
    using Winium.Cruciatus.Exceptions;

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
            ClickWithPressedKeys(element, new List<VirtualKeyCode> { VirtualKeyCode.CONTROL });
        }

        /// <summary>
        /// Клик по элементу с нажатыми кнопками (Ctrl, Shift, etc)
        /// </summary>
        /// <param name="element">
        /// Экземпляр элемента.
        /// </param>
        /// <param name="keys">
        /// Клавиши для "зажатия"
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", 
            Justification = "First parameter in extension cannot be null.")]
        public static void ClickWithPressedKeys(this CruciatusElement element, List<VirtualKeyCode> keys)
        {
            keys.ForEach(key => CruciatusFactory.Keyboard.KeyDown(key));
            element.Click();
            keys.ForEach(key => CruciatusFactory.Keyboard.KeyUp(key));
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
        /// Возвращает требуемый шаблон автоматизации.
        /// </summary>
        /// <param name="element">
        /// Экземпляр элемента.
        /// </param>
        /// <param name="pattern">
        /// Требуемый шаблон (например ExpandCollapsePattern.Pattern).
        /// </param>
        /// <typeparam name="T">
        /// Тип требуемого шаблона.
        /// </typeparam>
        /// <returns></returns>
        public static T GetPattern<T>(this CruciatusElement element, AutomationPattern pattern) where T : class
        {
            return element.Instance.GetPattern<T>(pattern);
        }

        /// <summary>
        /// Преобразовать элемент в CheckBox.
        /// </summary>
        public static CheckBox ToCheckBox(this CruciatusElement element)
        {
            return new CheckBox(element);
        }

        /// <summary>
        /// Преобразовать элемент в ComboBox.
        /// </summary>
        public static ComboBox ToComboBox(this CruciatusElement element)
        {
            return new ComboBox(element);
        }

        /// <summary>
        /// Преобразовать элемент в DataGrid.
        /// </summary>
        public static DataGrid ToDataGrid(this CruciatusElement element)
        {
            return new DataGrid(element);
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
