// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LazyInitializeException.cs" company="2GIS">
//   UITestLibrary.Exceptions
// </copyright>
// <summary>
//   Определяет класс исключение LazyInitializeException.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class LazyInitializeException : Exception
    {
        public LazyInitializeException()
        {
        }

        public LazyInitializeException(string message)
            : base(message)
        {
        }

        public LazyInitializeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected LazyInitializeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}