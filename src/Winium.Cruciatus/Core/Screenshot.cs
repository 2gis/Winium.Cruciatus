namespace Winium.Cruciatus.Core
{
    #region using

    using System;
    using System.IO;

    #endregion

    /// <summary>
    /// Класс описывающий скриншот.
    /// </summary>
    public class Screenshot
    {
        #region Fields

        private readonly string base64String = string.Empty;

        private readonly byte[] byteArray;

        #endregion

        #region Constructors and Destructors

        internal Screenshot(byte[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException("Cannot be null or empty", "array");
            }

            this.byteArray = array;
            this.base64String = Convert.ToBase64String(this.byteArray);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Возвращает скриншот в виде base64 строки.
        /// </summary>
        public string AsBase64String()
        {
            return this.base64String;
        }

        /// <summary>
        /// Возвращает скриншот в виде массива байт.
        /// </summary>
        public byte[] AsByteArray()
        {
            return this.byteArray;
        }

        /// <summary>
        /// Сохраняет скриншот в заданный файл.
        /// </summary>
        /// <param name="filePath">
        /// Путь до файла.
        /// </param>
        public void SaveAsFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Cannot be null or empty", "filePath");
            }

            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using (var stream = File.OpenWrite(filePath))
            {
                stream.Write(this.byteArray, 0, this.byteArray.Length);
            }
        }

        /// <summary>
        /// Возвращает скриншот в виде base64 строки.
        /// </summary>
        public override string ToString()
        {
            return this.base64String;
        }

        #endregion
    }
}
