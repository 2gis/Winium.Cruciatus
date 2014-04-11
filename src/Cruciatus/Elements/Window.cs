// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент окно.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus.Elements
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Windows.Automation;

    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    #endregion

    /// <summary>
    /// Представляет элемент окно.
    /// </summary>
    public class Window : CruciatusElement, IContainerElement
    {
        private readonly Dictionary<string, object> _childrenDictionary = new Dictionary<string, object>();

        /// <summary>
        /// Создает новый экземпляр класса <see cref="Window"/>.
        /// </summary>
        public Window()
        {
        }

        /// <summary>
        /// Создает и инициализирует новый экземпляр класса <see cref="Window"/>.
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
        public Window(CruciatusElement parent, string automationId)
        {
            Initialize(parent, automationId);
        }

        /// <summary>
        /// Возвращает текстовое представление имени класса.
        /// </summary>
        internal override string ClassName
        {
            get
            {
                return "Window";
            }
        }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.Window;
            }
        }

        /// <summary>
        /// Делает пометку, что окно закрыто (удаляет ссылки на дочерние элементы окна).
        /// </summary>
        public void Closed()
        {
            ElementInstance = null;
            _childrenDictionary.Clear();
        }

        public bool WaitForReady()
        {
            return Element.WaitForElementReady();
        }

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
