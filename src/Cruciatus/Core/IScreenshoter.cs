namespace Cruciatus.Core
{
    #region using

    using System.Diagnostics.CodeAnalysis;

    #endregion

    public interface IScreenshoter
    {
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        Screenshot GetScreenshot();
    }
}
