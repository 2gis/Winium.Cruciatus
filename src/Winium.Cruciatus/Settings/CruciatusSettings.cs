namespace Winium.Cruciatus.Settings
{
    #region using

    using Winium.Cruciatus.Core;
    using Winium.Cruciatus.Settings.MessageBoxSettings;

    #endregion

    /// <summary>
    /// Класс настроек Cruciatus.
    /// </summary>
    public class CruciatusSettings
    {
        #region Constants

        private const MouseButton DefaultClickButton = MouseButton.Left;

        private const int DefaultScrollBarHeight = 18;

        private const int DefaultScrollBarWidth = 18;

        private const int DefaultSearchTimeout = 10000;

        private const int DefaultWaitForExitTimeout = 10000;

        #endregion

        #region Static Fields

        private static readonly MessageBoxButtonUid DefaultMessageBoxButtonUid = new MessageBoxButtonUid();

        private static readonly OpenFileDialogUid DefaultOpenFileDialogUid = new OpenFileDialogUid();

        private static readonly SaveFileDialogUid DefaultSaveFileDialogUid = new SaveFileDialogUid();

        private static CruciatusSettings instance;

        #endregion

        #region Constructors and Destructors

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

            this.ResetToDefault();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Флаг автоматического снятия скриншотов. По умолчанию false.
        /// </summary>
        public bool AutomaticScreenshotCapture { get; set; }

        /// <summary>
        /// Кнопка для клика по умолчанию.
        /// </summary>
        public MouseButton ClickButton { get; set; }

        /// <summary>
        /// Информация о типе симулятора клавиатуры.
        /// </summary>
        public KeyboardSimulatorType KeyboardSimulatorType { get; set; }

        /// <summary>
        /// Информация о уникальных идентификаторах кнопок в диалоговом окне MessageBox.
        /// </summary>
        public MessageBoxButtonUid MessageBoxButtonUid { get; set; }

        /// <summary>
        /// Информация о уникальных идентификаторах элементов в OpenFileDialog.
        /// </summary>
        public OpenFileDialogUid OpenFileDialogUid { get; set; }

        /// <summary>
        /// Информация о уникальных идентификаторах элементов в SaveFileDialog.
        /// </summary>
        public SaveFileDialogUid SaveFileDialogUid { get; set; }

        /// <summary>
        /// Директорию для сохранения скриншотов. По умолчанию './Screenshots'.
        /// </summary>
        public string ScreenshotsPath { get; set; }

        /// <summary>
        /// Высота полосы прокрутки.
        /// </summary>
        public int ScrollBarHeight { get; set; }

        /// <summary>
        /// Ширина полосы прокрутки.
        /// </summary>
        public int ScrollBarWidth { get; set; }

        /// <summary>
        /// Время поиска элемента (миллисекунды).
        /// </summary>
        public int SearchTimeout { get; set; }

        /// <summary>
        /// Время ожидания завершения приложения (миллисекунды).
        /// </summary>
        public int WaitForExitTimeout { get; set; }

        #endregion

        #region Properties

        internal static CruciatusSettings Instance
        {
            get
            {
                return instance ?? (instance = new CruciatusSettings());
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Сбрасывает значения настроек на исходные.
        /// </summary>
        public void ResetToDefault()
        {
            this.SearchTimeout = DefaultSearchTimeout;
            this.WaitForExitTimeout = DefaultWaitForExitTimeout;
            this.ScrollBarWidth = DefaultScrollBarWidth;
            this.ScrollBarHeight = DefaultScrollBarHeight;
            this.ClickButton = DefaultClickButton;
            this.KeyboardSimulatorType = KeyboardSimulatorType.BasedOnWindowsFormsSendKeysClass;
            this.ScreenshotsPath = "Screenshots";
            this.AutomaticScreenshotCapture = false;

            this.MessageBoxButtonUid = (MessageBoxButtonUid)DefaultMessageBoxButtonUid.Clone();
            this.OpenFileDialogUid = (OpenFileDialogUid)DefaultOpenFileDialogUid.Clone();
            this.SaveFileDialogUid = (SaveFileDialogUid)DefaultSaveFileDialogUid.Clone();
        }

        #endregion
    }
}
