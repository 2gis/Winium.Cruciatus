namespace Cruciatus.Settings.MessageBoxSettings
{
    #region using

    using System;

    #endregion

    public class YesNoType : ICloneable
    {
        public string Yes { get; set; }

        public string No { get; set; }

        public object Clone()
        {
            return new YesNoType { Yes = Yes, No = No };
        }
    }
}
