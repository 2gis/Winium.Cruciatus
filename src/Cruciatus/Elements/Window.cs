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

    using System.Collections.Generic;
    using System.Windows.Automation;

    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    #endregion

    /// <summary>
    /// Представляет элемент окно.
    /// </summary>
    public abstract class Window : CruciatusElement, IContainerElement
    {
        private readonly Dictionary<string, object> _childrenDictionary = new Dictionary<string, object>();

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
        /// Возвращает инициализированный элемент окна.
        /// </summary>
        internal override AutomationElement Element
        {
            get
            {
                if (ElementInstance == null)
                {
                    Find();

                    // TODO: Нужны приложения с окнами для улучшения этих костыльных строчек
                    ////object objectPattern;
                    ////if (this.element.TryGetCurrentPattern(WindowPattern.Pattern, out objectPattern))
                    ////{
                    ////((WindowPattern)objectPattern).WaitForInputIdle(1500);
                    ////}
                    ////else
                    ////{
                    ////Thread.Sleep(500);
                    ////}
                }

                return ElementInstance;
            }
        }

        void IContainerElement.Initialize(AutomationElement parent, string automationId)
        {
            Initialize(parent, automationId);
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

        protected virtual T GetElement<T>(string automationId) where T : CruciatusElement, IContainerElement, new()
        {
            try
            {
                if (!_childrenDictionary.ContainsKey(automationId))
                {
                    var item = new T();
                    item.Initialize(Element, automationId);
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
