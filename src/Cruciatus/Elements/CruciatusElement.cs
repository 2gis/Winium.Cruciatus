// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CruciatusElement.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет базу для элементов управления.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus.Elements
{
    #region using

    using System;
    using System.Windows.Automation;

    using Cruciatus.Exceptions;

    #endregion

    /// <summary>
    /// Базовый класс для элементов.
    /// </summary>
    public abstract class CruciatusElement
    {
        /// <summary>
        /// Текст последней ошибки.
        /// </summary>
        public string LastErrorMessage { get; internal set; }

        internal AutomationElement ElementInstance { get; set; }

        internal AutomationElement Parent { get; set; }

        internal string AutomationId { get; set; }

        internal abstract string ClassName { get; }

        internal new abstract ControlType GetType { get; }

        internal AutomationElement Element
        {
            get
            {
                if (ElementInstance == null)
                {
                    if (Parent == null || AutomationId == null)
                    {
                        LastErrorMessage = "Элемент нельзя использовать, пока он не инициализирован";
                        return null;
                    }

                    Find();
                }

                return ElementInstance;
            }
        }

        public new string ToString()
        {
            return string.Format("{0} (uid: {1})", ClassName, AutomationId ?? "nonUid");
        }

        internal virtual void Find()
        {
            ElementInstance = CruciatusFactory.Find(Parent, AutomationId, TreeScope.Subtree);

            if (ElementInstance == null)
            {
                throw new ElementNotFoundException(ToString());
            }
        }

        public void Initialize(CruciatusElement parent, string automationId)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            if (automationId == null)
            {
                throw new ArgumentNullException("automationId");
            }

            if (parent.Element == null)
            {
                throw new ElementNotFoundException(parent.ToString());
            }

            Parent = parent.Element;
            AutomationId = automationId;
        }
    }
}
