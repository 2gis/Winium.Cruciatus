// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TabItem.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент управления вкладка.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus.Elements
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Automation;
    using System.Windows.Forms;

    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    using Microsoft.VisualStudio.TestTools.UITesting;

    using ControlType = System.Windows.Automation.ControlType;

    /// <summary>
    /// Представляет элемент управления вкладка.
    /// </summary>
    public abstract class TabItem : CruciatusElement, IContainerElement
    {
        private readonly Dictionary<string, object> objects = new Dictionary<string, object>();

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TabItem"/>.
        /// </summary>
        protected TabItem()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TabItem"/>.
        /// </summary>
        /// <param name="parent">
        /// Элемент, являющийся родителем для вкладки.
        /// </param>
        /// <param name="automationId">
        /// Уникальный идентификатор вкладки.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Входные параметры не должны быть нулевыми.
        /// </exception>
        protected TabItem(AutomationElement parent, string automationId)
        {
            Initialize(parent, automationId);
        }

        /// <summary>
        /// Возвращает значение, указывающее, включена ли вкладка.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Вкладка не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public bool IsEnabled
        {
            get
            {
                return this.GetPropertyValue<bool>(AutomationElement.IsEnabledProperty);
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее, выбрана ли вкладка.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Вкладка не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public bool IsSelection
        {
            get
            {
                return this.GetPropertyValue<bool>(SelectionItemPattern.IsSelectedProperty);
            }
        }

        /// <summary>
        /// Возвращает координаты точки, внутри вкладки, которые можно использовать для нажатия.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Вкладка не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public System.Drawing.Point ClickablePoint
        {
            get
            {
                var windowsPoint = this.GetPropertyValue<System.Windows.Point>(AutomationElement.ClickablePointProperty);

                return new System.Drawing.Point((int)windowsPoint.X, (int)windowsPoint.Y);
            }
        }

        /// <summary>
        /// Возвращает текстовое представление имени класса.
        /// </summary>
        internal override string ClassName
        {
            get
            {
                return "TabItem";
            }
        }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.TabItem;
            }
        }

        /// <summary>
        /// Выбирает вкладку текущей.
        /// </summary>
        /// <returns>
        /// Значение true если удалось выбрать либо уже выбрана; в противном случае значение - false.
        /// </returns>
        public bool Select()
        {
            try
            {
                if (!this.IsSelection)
                {
                    if (!this.Click())
                    {
                        return false;
                    }

                    if (!this.Element.WaitForElementReady())
                    {
                        this.LastErrorMessage = string.Format(
                            "Время ожидания готовности вкладки {0} истекло.",
                            this.ToString());
                        return false;
                    }
                }

                return true;
            }
            catch (Exception exc)
            {
                this.LastErrorMessage = exc.Message;
                return false;
            }
        }

        /// <summary>
        /// Возвращает элемент заданного типа с указанным уникальным идентификатором.
        /// </summary>
        /// <param name="automationId">
        /// Уникальный идентификатор элемента.
        /// </param>
        /// <typeparam name="T">
        /// Тип элемента.
        /// </typeparam>
        /// <returns>
        /// Искомый элемент, либо null, если найти не удалось.
        /// </returns>
        protected virtual T GetElement<T>(string automationId) where T : CruciatusElement, IContainerElement, new()
        {
            try
            {
                if (!this.IsSelection)
                {
                    this.LastErrorMessage = string.Format("Вкладка {0} не выбрана.", this.ToString());
                    return null;
                }

                if (!this.objects.ContainsKey(automationId))
                {
                    var item = new T();
                    item.Initialize(this.Element, automationId);
                    this.objects.Add(automationId, item);
                }

                return (T)this.objects[automationId];
            }
            catch (Exception exc)
            {
                this.LastErrorMessage = exc.Message;
                return null;
            }
        }

        /// <summary>
        /// Выполняет нажатие по вкладке.
        /// </summary>
        /// <param name="mouseButton">
        /// Задает кнопку мыши, которой будет произведено нажатие; либо кнопка по умолчанию.
        /// </param>
        private bool Click(MouseButtons mouseButton = MouseButtons.Left)
        {
            
            try
            {
                var isEnabled = CruciatusFactory.WaitingValues(
                    () => this.IsEnabled,
                    value => value != true);

                if (!isEnabled)
                {
                    this.LastErrorMessage = string.Format(
                        "Вкладка {0} отключена, нельзя выполнить переход.",
                        this.ToString());
                    return false;
                }

                Mouse.MouseMoveSpeed = CruciatusFactory.Settings.MouseMoveSpeed;
                Mouse.Move(this.ClickablePoint);

                // Костыльное дело, но без этой строки не работает на "чистой" Telerek-вкладке 
                Mouse.Move(new System.Drawing.Point(Mouse.Location.X + 1, Mouse.Location.Y));

                Mouse.Click(mouseButton);
            }
            catch (Exception exc)
            {
                this.LastErrorMessage = exc.Message;
                return false;
            }

            return true;
        }

        void IContainerElement.Initialize(AutomationElement parent, string automationId)
        {
            Initialize(parent, automationId);
        }
    }
}
