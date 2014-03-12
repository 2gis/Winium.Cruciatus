// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyNotSupportedException.cs" company="2GIS">
//   UITestLibrary.Exceptions
// </copyright>
// <summary>
//   Определяет класс исключение PropertyNotSupportedException.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class PropertyNotSupportedException : CruciatusException
    {
        private string fieldMessage = "Элемент не поддерживает желаемое свойство.\n";

        public PropertyNotSupportedException()
        {
        }

        public PropertyNotSupportedException(string message)
        {
            this.Initialize(message);
        }

        public PropertyNotSupportedException(string element, string property)
        {
            this.Initialize(element, property);
        }

        public PropertyNotSupportedException(string element, string property, string message)
            : this(element, property)
        {
            this.Initialize(message);
        }

        public PropertyNotSupportedException(string element, string property, Exception innerException)
            : base(string.Empty, innerException)
        {
            this.Initialize(element, property);
        }

        protected PropertyNotSupportedException(SerializationInfo info, StreamingContext context)
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

                var str = this.fieldMessage + string.Format("Подробности: {0}, свойство {1}.\n", this.Element, this.Property);
                return str;
            }
        }

        private string Element { get; set; }

        private string Property { get; set; }

        protected new virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        private void Initialize(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            this.fieldMessage = message;
        }

        private void Initialize(string element, string property)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            this.Element = element;
            this.Property = property;
        }
    }
}
