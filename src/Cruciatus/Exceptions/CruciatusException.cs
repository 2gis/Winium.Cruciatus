// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CruciatusException.cs" company="2GIS">
//   UITestLibrary.Exceptions
// </copyright>
// <summary>
//   Определяет базовый класс исключений CruciatusException.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus.Exceptions
{
    #region using

    using System;
    using System.Runtime.Serialization;

    #endregion

    [Serializable]
    public class CruciatusException : Exception
    {
        public CruciatusException()
        {
        }

        public CruciatusException(string message)
            : base(message)
        {
        }

        public CruciatusException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CruciatusException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
