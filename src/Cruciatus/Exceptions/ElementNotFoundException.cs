// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElementNotFoundException.cs" company="2GIS">
//   UITestLibrary.Exceptions
// </copyright>
// <summary>
//   Определяет класс исключение ElementNotFoundException.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ElementNotFoundException : CruciatusException
    {
        private readonly string fieldMessage = "Элемент не найден.\n";

        public ElementNotFoundException()
        {
        }

        public ElementNotFoundException(string element)
        {
            this.Initialize(element);
        }

        public ElementNotFoundException(string element, string message)
            : this(element)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            this.fieldMessage = message;
        }

        public ElementNotFoundException(string element, Exception innerException)
            : base(string.Empty, innerException)
        {
            this.Initialize(element);
        }

        protected ElementNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override string Message
        {
            get
            {
                if (this.Element == null)
                {
                    return this.fieldMessage;
                }

                var str = this.fieldMessage + string.Format("Подробности: {0}.\n", this.Element);
                return str;
            }
        }

        private string Element { get; set; }

        protected new virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        private void Initialize(string element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.Element = element;
        }
    }
}
