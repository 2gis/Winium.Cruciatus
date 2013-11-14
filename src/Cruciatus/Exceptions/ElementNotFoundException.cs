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
    public class ElementNotFoundException : Exception
    {
        public ElementNotFoundException()
        {
        }

        public ElementNotFoundException(string element)
        {
            this.Element = element;
        }

        public ElementNotFoundException(string element, string message)
            : base(message)
        {
            this.Element = element;
        }

        public ElementNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
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
                    return "Элемент не найден.\n";
                }

                return string.Format("Элемент {0} не найден.\n", this.Element);
            }
        }

        private string Element { get; set; }
    }
}
