// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyInvalidCastException.cs" company="2GIS">
//   UITestLibrary.Exceptions
// </copyright>
// <summary>
//   Определяет класс исключение PropertyInvalidCastException.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus.Exceptions
{
    #region using

    using System;
    using System.Runtime.Serialization;

    #endregion

    [Serializable]
    public class PropertyInvalidCastException : CruciatusException
    {
        public PropertyInvalidCastException()
        {
        }

        public PropertyInvalidCastException(string message)
            : base(message)
        {
        }

        public PropertyInvalidCastException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PropertyInvalidCastException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
