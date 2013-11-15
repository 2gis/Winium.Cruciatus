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
        private string message = "Элемент не найден.\n";

        public ElementNotFoundException()
        {
        }

        public ElementNotFoundException(string element)
        {
            this.Element = element;
        }

        public ElementNotFoundException(string element, string message)
            : base(string.Empty)
        {
            this.Element = element;
            this.message = message;
        }

        public ElementNotFoundException(string element, Exception innerException)
            : base(string.Empty, innerException)
        {
            this.Element = element;
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
                    return this.message;
                }

                var str = this.message + string.Format("Подробности: {0}.\n", this.Element);
                return str;
            }
        }

        private string Element { get; set; }
    }
}
