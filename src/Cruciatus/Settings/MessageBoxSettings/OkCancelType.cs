namespace Cruciatus.Settings.MessageBoxSettings
{
    #region using

    using System;

    #endregion

    public class OkCancelType : ICloneable
    {
        public string Ok { get; set; }

        public string Cancel { get; set; }

        public object Clone()
        {
            return new OkCancelType { Ok = Ok, Cancel = Cancel };
        }
    }
}
