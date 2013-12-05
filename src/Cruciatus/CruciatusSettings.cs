// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CruciatusSettings.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет класс настроек Cruciatus.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus
{
    public class CruciatusSettings
    {
        private const int DefaultWaitTimeout = 7500;

        private const int DefaultWaitingPeriod = 25;

        private const int DefaultSearchTimeout = 10000;

        private const int DefaultWaitForExitTimeout = 10000;

        private const int DefaultWaitForReadyTimeout = 5000;

        private const int DefaultMouseMoveSpeed = 2500;

        private static CruciatusSettings instance;

        private CruciatusSettings()
        {
            this.ResetToDefault();
        }

        public int WaitTimeout { get; set; }

        public int WaitingPeriod { get; set; }

        public int SearchTimeout { get; set; }

        public int WaitForExitTimeout { get; set; }

        public int WaitForReadyTimeout { get; set; }

        public int MouseMoveSpeed { get; set; }

        internal static CruciatusSettings Instance
        {
            get
            {
                return instance ?? (instance = new CruciatusSettings());
            }
        }

        /// <summary>
        /// Сбрасывает значения настроек на исходные.
        /// </summary>
        public void ResetToDefault()
        {
            this.WaitTimeout = DefaultWaitTimeout;
            this.WaitingPeriod = DefaultWaitingPeriod;
            this.SearchTimeout = DefaultSearchTimeout;
            this.WaitForExitTimeout = DefaultWaitForExitTimeout;
            this.WaitForReadyTimeout = DefaultWaitForReadyTimeout;
            this.MouseMoveSpeed = DefaultMouseMoveSpeed;
        }
    }
}
