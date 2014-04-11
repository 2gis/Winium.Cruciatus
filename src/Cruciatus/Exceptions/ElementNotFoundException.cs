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
    #region using

    using System;
    using System.Runtime.Serialization;

    #endregion

    [Serializable]
    public class ElementNotFoundException : CruciatusException
    {
        private readonly string _fieldMessage = "Элемент не найден.\n";

        public ElementNotFoundException()
        {
        }

        public ElementNotFoundException(string element)
        {
            Initialize(element);
        }

        public ElementNotFoundException(string element, string message)
            : this(element)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            _fieldMessage = message;
        }

        public ElementNotFoundException(string element, Exception innerException)
            : base(string.Empty, innerException)
        {
            Initialize(element);
        }

        protected ElementNotFoundException(SerializationInfo info, StreamingContext context)
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

                var str = _fieldMessage + string.Format(" Подробности: {0}.\n", Element);
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

            Element = element;
        }
    }
}
