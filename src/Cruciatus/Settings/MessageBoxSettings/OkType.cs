namespace Cruciatus.Settings.MessageBoxSettings
{
    #region using

    using System;

    #endregion

    public class OkType : ICloneable
    {
        public string Ok { get; set; }

        public object Clone()
        {
            return new OkType { Ok = Ok };
        }
    }
}
