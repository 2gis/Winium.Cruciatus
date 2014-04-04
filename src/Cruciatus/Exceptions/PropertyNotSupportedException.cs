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
    #region using

    using System;
    using System.Runtime.Serialization;

    #endregion

    [Serializable]
    public class PropertyNotSupportedException : CruciatusException
    {
        private string _fieldMessage = "Элемент не поддерживает желаемое свойство.\n";

        public PropertyNotSupportedException()
        {
        }

        public PropertyNotSupportedException(string message)
        {
            Initialize(message);
        }

        public PropertyNotSupportedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public PropertyNotSupportedException(string element, string property)
        {
            Initialize(element, property);
        }

        public PropertyNotSupportedException(string element, string property, string message)
            : this(element, property)
        {
            Initialize(message);
        }

        public PropertyNotSupportedException(string element, string property, Exception innerException)
            : base(string.Empty, innerException)
        {
            Initialize(element, property);
        }

        protected PropertyNotSupportedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override string Message
        {
            get
            {
                if (Element == null)
                {
                    return _fieldMessage;
                }

                var str = _fieldMessage + string.Format("Подробности: {0}, свойство {1}.\n", Element, Property);
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

            _fieldMessage = message;
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

            Element = element;
            Property = property;
        }
    }
}
