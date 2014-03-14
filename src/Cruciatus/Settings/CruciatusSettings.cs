// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CruciatusSettings.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет класс настроек Cruciatus.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus.Settings
{
    using System.Windows.Forms;

    using Cruciatus.Settings.MessageBoxSettings;

    public class CruciatusSettings
    {
        private const int DefaultWaitingPeriod = 25;

        private const int DefaultSearchTimeout = 10000;

        private const int DefaultWaitForExitTimeout = 10000;

        private const int DefaultWaitForReadyTimeout = 5000;

        private const int DefaultWaitForGetValueTimeout = 7500;

        private const int DefaultMouseMoveSpeed = 2500;

        private const int DefaultScrollBarWidth = 18;

        private const int DefaultScrollBarHeight = 18;

        private const MouseButtons DefaultClickButton = MouseButtons.Left;

        private static CruciatusSettings instance;

        private readonly MessageBoxButtonUid defaultMessageBoxButtonUid = new MessageBoxButtonUid();

        private CruciatusSettings()
        {
            this.defaultMessageBoxButtonUid.CloseButton = "Close";
            this.defaultMessageBoxButtonUid.OkType = new OkType { Ok = "2" };
            this.defaultMessageBoxButtonUid.OkCancelType = new OkCancelType { Ok = "1", Cancel = "2" };
            this.defaultMessageBoxButtonUid.YesNoType = new YesNoType { Yes = "6", No = "7" };
            this.defaultMessageBoxButtonUid.YesNoCancelType = new YesNoCancelType { Yes = "6", No = "7", Cancel = "2" };

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

        /// <summary>
        /// Возвращает или задает ширину полосы прокрутки.
        /// </summary>
        public int ScrollBarWidth { get; set; }

        /// <summary>
        /// Возвращает или задает высоту полосы прокрутки.
        /// </summary>
        public int ScrollBarHeight { get; set; }

        /// <summary>
        /// Возвращает или задает кнопку мыши, которой производится нажатие (click).
        /// </summary>
        public MouseButtons ClickButton { get; set; }

        /// <summary>
        /// Возвращает или задает информацию о уникальных идентификаторах кнопок в MessageBox.
        /// </summary>
        public MessageBoxButtonUid MessageBoxButtonUid { get; set; }

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
            this.ScrollBarWidth = DefaultScrollBarWidth;
            this.ScrollBarHeight = DefaultScrollBarHeight;
            this.ClickButton = DefaultClickButton;

            this.MessageBoxButtonUid = (MessageBoxButtonUid)this.defaultMessageBoxButtonUid.Clone();
        }
    }
}
