namespace Cruciatus.Settings.MessageBoxSettings
{
    #region using

    using System;

    #endregion

    public class YesNoCancelType : ICloneable
    {
        public string Yes { get; set; }

        public string No { get; set; }

        public string Cancel { get; set; }

        public object Clone()
        {
            return new YesNoCancelType { Yes = Yes, No = No, Cancel = Cancel };
        }
    }
}
