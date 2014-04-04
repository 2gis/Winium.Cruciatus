namespace Cruciatus.Settings.MessageBoxSettings
{
    #region using

    using System;

    #endregion

    public class MessageBoxButtonUid : ICloneable
    {
        public string CloseButton { get; set; }

        public OkType OkType { get; set; }

        public OkCancelType OkCancelType { get; set; }

        public YesNoType YesNoType { get; set; }

        public YesNoCancelType YesNoCancelType { get; set; }

        public object Clone()
        {
            return new MessageBoxButtonUid
            {
                CloseButton = CloseButton, 
                OkType = (OkType)OkType.Clone(), 
                OkCancelType = (OkCancelType)OkCancelType.Clone(), 
                YesNoType = (YesNoType)YesNoType.Clone(), 
                YesNoCancelType = (YesNoCancelType)YesNoCancelType.Clone()
            };
        }
    }
}
