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
    #region using

    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Automation;
    using System.Windows.Forms;

    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    using Microsoft.VisualStudio.TestTools.UITesting;

    #endregion

    /// <summary>
    /// Представляет элемент управления вкладка.
    /// </summary>
    public class TabItem : CruciatusElement, IContainerElement
    {
        private readonly Dictionary<string, object> _childrenDictionary = new Dictionary<string, object>();

        /// <summary>
        /// Создает новый экземпляр класса <see cref="TabItem"/>.
        /// </summary>
        public TabItem()
        {
        }

        /// <summary>
        /// Создает и инициализирует новый экземпляр класса <see cref="TabItem"/>.
        /// </summary>
        /// <param name="parent">
        /// Родительский элемент.
        /// </param>
        /// <param name="automationId">
        /// Уникальный идентификатор в рамках родительского элемента.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Входные параметры не должны быть нулевыми.
        /// </exception>
        public TabItem(CruciatusElement parent, string automationId)
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
        public Point ClickablePoint
        {
            get
            {
                var windowsPoint = this.GetPropertyValue<System.Windows.Point>(AutomationElement.ClickablePointProperty);

                return new Point((int)windowsPoint.X, (int)windowsPoint.Y);
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
                if (IsSelection)
                {
                    return true;
                }

                if (!Click())
                {
                    return false;
                }

                if (Element.WaitForElementReady())
                {
                    return true;
                }

                LastErrorMessage = string.Format("Время ожидания готовности вкладки {0} истекло.", ToString());
                return false;
            }
            catch (CruciatusException exc)
            {
                LastErrorMessage = exc.Message;
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
        public virtual T GetElement<T>(string automationId) where T : CruciatusElement, IContainerElement, new()
        {
            if (automationId == null)
            {
                throw new ArgumentNullException("automationId");
            }

            try
            {
                if (!IsSelection)
                {
                    LastErrorMessage = string.Format("Вкладка {0} не выбрана.", ToString());
                    return null;
                }

                if (!_childrenDictionary.ContainsKey(automationId))
                {
                    var item = new T();
                    item.Initialize(this, automationId);
                    _childrenDictionary.Add(automationId, item);
                }

                return (T)_childrenDictionary[automationId];
            }
            catch (CruciatusException exc)
            {
                LastErrorMessage = exc.Message;
                return null;
            }
        }

        /// <summary>
        /// Выполняет нажатие по вкладке.
        /// </summary>
        /// <param name="mouseButton">
        /// Задает кнопку мыши, которой будет произведено нажатие; либо кнопка по умолчанию.
        /// </param>
        /// <returns>
        /// Значение true если нажать на элемент удалось; в противном случае значение - false.
        /// </returns>
        private bool Click(MouseButtons mouseButton = MouseButtons.Left)
        {
            try
            {
                var isEnabled = CruciatusFactory.WaitingValues(() => IsEnabled, value => value != true);

                if (!isEnabled)
                {
                    var str = string.Format("Вкладка {0} отключена, нельзя выполнить переход.", ToString());
                    LastErrorMessage = str;
                    return false;
                }

                Mouse.MouseMoveSpeed = CruciatusFactory.Settings.MouseMoveSpeed;
                Mouse.Move(ClickablePoint);

                // Костыльное дело, но без этой строки не работает на "чистой" Telerek-вкладке 
                Mouse.Move(new Point(Mouse.Location.X + 1, Mouse.Location.Y));

                Mouse.Click(mouseButton);
            }
            catch (CruciatusException exc)
            {
                LastErrorMessage = exc.Message;
                return false;
            }

            return true;
        }
    }
}
