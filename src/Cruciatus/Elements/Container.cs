// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Container.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет контейнер для элементов.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus.Elements
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Automation;

    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    #endregion

    public class Container : CruciatusElement, IContainerElement
    {
        private readonly Dictionary<string, object> _childrenDictionary = new Dictionary<string, object>();

        /// <summary>
        /// Создает новый экземпляр класса <see cref="Container"/>.
        /// </summary>
        public Container()
        {
        }

        /// <summary>
        /// Создает и инициализирует новый экземпляр класса <see cref="Container"/>.
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
        public Container(CruciatusElement parent, string automationId)
        {
            Initialize(parent, automationId);
        }

        /// <summary>
        /// Возвращает координаты точки, внутри кликабельного элемента, которые можно использовать для нажатия.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Элемент не поддерживает данное свойство.
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
                return "Container";
            }
        }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.Custom;
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
            try
            {
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
    }
}
