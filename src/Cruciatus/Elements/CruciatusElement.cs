// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseElement.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет базу для элементов управления.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus.Elements
{
    using System;
    using System.Windows.Automation;

    using Cruciatus.Exceptions;

    public abstract class CruciatusElement
    {
        internal AutomationElement ElementInstance;

        internal string LastErrorMessageInstance;

        public string LastErrorMessage
        {
            get
            {
                return LastErrorMessageInstance;
            }

            internal set
            {
                LastErrorMessageInstance = value;
            }
        }

        internal abstract string ClassName { get; }

        internal new abstract ControlType GetType { get; }

        internal AutomationElement Parent { get; set; }

        internal string AutomationId { get; set; }

        internal virtual AutomationElement Element
        {
            get
            {
                if (this.ElementInstance == null)
                {
                    this.Find();
                }

                return this.ElementInstance;
            }
        }

        protected void Initialize(AutomationElement parent, string automationId)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            if (automationId == null)
            {
                throw new ArgumentNullException("automationId");
            }

            Parent = parent;
            AutomationId = automationId;
        }

        protected void Initialize(AutomationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            ElementInstance = element;
        }

        internal virtual void Find()
        {
            // Ищем в родителе первый встретившийся контрол с заданным automationId
            var condition = new PropertyCondition(AutomationElement.AutomationIdProperty, this.AutomationId);
            this.ElementInstance = CruciatusFactory.WaitingValues(
                () => this.Parent.FindFirst(TreeScope.Subtree, condition),
                value => value == null,
                CruciatusFactory.Settings.SearchTimeout);

            // Если не нашли, то загрузить элемент не удалось
            if (this.ElementInstance == null)
            {
                throw new ElementNotFoundException(this.ToString());
            }
        }

        public new string ToString()
        {
            return string.Format("{0} (uid: {1})", this.ClassName, this.AutomationId ?? "nonUid");
        }
    }
}
