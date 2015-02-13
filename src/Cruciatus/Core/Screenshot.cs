namespace Cruciatus.Core
{
    #region using

    using System;
    using System.IO;

    #endregion

    public class Screenshot
    {
        private readonly string _base64String = string.Empty;

        private readonly byte[] _byteArray;

        public Screenshot(byte[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException("Cannot be null or empty", "array");
            }

            _byteArray = array;
            _base64String = Convert.ToBase64String(_byteArray);
        }

        public Screenshot(string base64)
        {
            if (string.IsNullOrEmpty(base64))
            {
                throw new ArgumentException("Cannot be null or empty", "base64");
            }

            _base64String = base64;
            _byteArray = Convert.FromBase64String(_base64String);
        }

        public string AsBase64String()
        {
            return _base64String;
        }

        public byte[] AsByteArray()
        {
            return _byteArray;
        }

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
                stream.Write(_byteArray, 0, _byteArray.Length);
            }
        }

        public override string ToString()
        {
            return _base64String;
        }
    }
}
