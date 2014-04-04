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
    #region using

    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Forms;

    using Cruciatus.Settings.MessageBoxSettings;

    #endregion

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

        private static readonly MessageBoxButtonUid DefaultMessageBoxButtonUid = new MessageBoxButtonUid();

        private static CruciatusSettings _instance;

        private CruciatusSettings()
        {
            DefaultMessageBoxButtonUid.CloseButton = "Close";
            DefaultMessageBoxButtonUid.OkType = new OkType { Ok = "2" };
            DefaultMessageBoxButtonUid.OkCancelType = new OkCancelType { Ok = "1", Cancel = "2" };
            DefaultMessageBoxButtonUid.YesNoType = new YesNoType { Yes = "6", No = "7" };
            DefaultMessageBoxButtonUid.YesNoCancelType = new YesNoCancelType { Yes = "6", No = "7", Cancel = "2" };

            ResetToDefault();
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
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", Justification = "Reviewed.")]
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
                return _instance ?? (_instance = new CruciatusSettings());
            }
        }

        /// <summary>
        /// Сбрасывает значения настроек на исходные.
        /// </summary>
        public void ResetToDefault()
        {
            WaitingPeriod = DefaultWaitingPeriod;
            SearchTimeout = DefaultSearchTimeout;
            WaitForExitTimeout = DefaultWaitForExitTimeout;
            WaitForReadyTimeout = DefaultWaitForReadyTimeout;
            WaitForGetValueTimeout = DefaultWaitForGetValueTimeout;
            MouseMoveSpeed = DefaultMouseMoveSpeed;
            ScrollBarWidth = DefaultScrollBarWidth;
            ScrollBarHeight = DefaultScrollBarHeight;
            ClickButton = DefaultClickButton;

            MessageBoxButtonUid = (MessageBoxButtonUid)DefaultMessageBoxButtonUid.Clone();
        }
    }
}
