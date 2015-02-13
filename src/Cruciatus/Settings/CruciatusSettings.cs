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

    using Cruciatus.Core;
    using Cruciatus.Settings.MessageBoxSettings;

    #endregion

    public class CruciatusSettings
    {
        private const int DefaultWaitingPeriod = 25;

        private const int DefaultSearchTimeout = 60000;

        private const int DefaultWaitForExitTimeout = 10000;

        private const int DefaultWaitForReadyTimeout = 5000;

        private const int DefaultWaitForGetValueTimeout = 7500;

        private const int DefaultMouseMoveSpeed = 2500;

        private const int DefaultScrollBarWidth = 18;

        private const int DefaultScrollBarHeight = 18;

        private const MouseButton DefaultClickButton = MouseButton.Left;

        private static readonly MessageBoxButtonUid DefaultMessageBoxButtonUid = new MessageBoxButtonUid();

        private static readonly OpenFileDialogUid DefaultOpenFileDialogUid = new OpenFileDialogUid();

        private static readonly SaveFileDialogUid DefaultSaveFileDialogUid = new SaveFileDialogUid();

        private static CruciatusSettings _instance;

        private CruciatusSettings()
        {
            DefaultMessageBoxButtonUid.CloseButton = "Close";
            DefaultMessageBoxButtonUid.OkType = new OkType { Ok = "2" };
            DefaultMessageBoxButtonUid.OkCancelType = new OkCancelType { Ok = "1", Cancel = "2" };
            DefaultMessageBoxButtonUid.YesNoType = new YesNoType { Yes = "6", No = "7" };
            DefaultMessageBoxButtonUid.YesNoCancelType = new YesNoCancelType { Yes = "6", No = "7", Cancel = "2" };

            DefaultOpenFileDialogUid.OpenButton = "1";
            DefaultOpenFileDialogUid.CancelButton = "2";
            DefaultOpenFileDialogUid.FileNameEditableComboBox = "1148";

            DefaultSaveFileDialogUid.SaveButton = "1";
            DefaultSaveFileDialogUid.CancelButton = "2";
            DefaultSaveFileDialogUid.FileNameEditableComboBox = "FileNameControlHost";
            DefaultSaveFileDialogUid.FileTypeComboBox = "FileTypeControlHost";

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
        public MouseButton ClickButton { get; set; }

        /// <summary>
        /// Возвращает или задает информацию о уникальных идентификаторах кнопок в MessageBox.
        /// </summary>
        public MessageBoxButtonUid MessageBoxButtonUid { get; set; }

        /// <summary>
        /// Возвращает или задает информацию о уникальных идентификаторах элементов в OpenFileDialog.
        /// </summary>
        public OpenFileDialogUid OpenFileDialogUid { get; set; }

        /// <summary>
        /// Возвращает или задает информацию о уникальных идентификаторах элементов в SaveFileDialog.
        /// </summary>
        public SaveFileDialogUid SaveFileDialogUid { get; set; }

        /// <summary>
        /// Возвращает или задает информацию о типе симулятора клавиатуры.
        /// </summary>
        public KeyboardSimulatorType KeyboardSimulatorType { get; set; }

        /// <summary>
        /// Возвращает или задает директорию для скриншотов.
        /// </summary>
        public string ScreenshotsPath { get; set; }

        /// <summary>
        /// По умолчанию false.
        /// </summary>
        public bool AutomaticScreenshotCapture { get; set; }

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
            KeyboardSimulatorType = KeyboardSimulatorType.BasedOnWindowsFormsSendKeysClass;
            ScreenshotsPath = "Screenshots";
            AutomaticScreenshotCapture = false;

            MessageBoxButtonUid = (MessageBoxButtonUid)DefaultMessageBoxButtonUid.Clone();
            OpenFileDialogUid = (OpenFileDialogUid)DefaultOpenFileDialogUid.Clone();
            SaveFileDialogUid = (SaveFileDialogUid)DefaultSaveFileDialogUid.Clone();
        }
    }
}
