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
        private const int DefaultWaitingPeriod = 25;

        private const int DefaultSearchTimeout = 10000;

        private const int DefaultWaitForExitTimeout = 10000;

        private const int DefaultWaitForReadyTimeout = 5000;

        private const int DefaultWaitForGetValueTimeout = 7500;
        
        private const int DefaultMouseMoveSpeed = 2500;

        private static CruciatusSettings instance;

        private CruciatusSettings()
        {
            this.ResetToDefault();
        }

        /// <summary>
        /// Возвращает или задает период между запросами во время ожидания в миллисекундах.
        /// </summary>
        public int WaitingPeriod { get; set; }

        /// <summary>
        /// Возвращает или задает время поиска элемента в миллисекундках.
        /// </summary>
        public int SearchTimeout { get; set; }

        /// <summary>
        /// Возвращает или задает время ожидания завершения приложения в миллисекундках.
        /// </summary>
        public int WaitForExitTimeout { get; set; }

        /// <summary>
        /// Возвращает или задает время ожидания готовности элемента в миллисекундках.
        /// </summary>
        public int WaitForReadyTimeout { get; set; }

        /// <summary>
        /// Возвращает или задает время ожидания получения свойств элемента в миллисекундках.
        /// </summary>
        public int WaitForGetValueTimeout { get; set; }

        /// <summary>
        /// Возвращает или задает скорость передвижения мыши.
        /// </summary>
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
            this.WaitingPeriod = DefaultWaitingPeriod;
            this.SearchTimeout = DefaultSearchTimeout;
            this.WaitForExitTimeout = DefaultWaitForExitTimeout;
            this.WaitForReadyTimeout = DefaultWaitForReadyTimeout;
            this.WaitForGetValueTimeout = DefaultWaitForGetValueTimeout;
            this.MouseMoveSpeed = DefaultMouseMoveSpeed;
        }
    }
}
