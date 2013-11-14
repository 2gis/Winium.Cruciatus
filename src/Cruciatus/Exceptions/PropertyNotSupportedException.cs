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
    public class PropertyNotSupportedException : Exception
    {
        public PropertyNotSupportedException()
        {
        }

        public PropertyNotSupportedException(string message)
            : base(message)
        {
        }

        public PropertyNotSupportedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PropertyNotSupportedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}