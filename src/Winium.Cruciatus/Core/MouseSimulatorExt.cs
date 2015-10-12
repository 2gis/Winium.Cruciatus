namespace Winium.Cruciatus.Core
{
    #region using

    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Windows;
    using System.Windows.Forms;

    using WindowsInput;

    using Winium.Cruciatus.Helpers;

    #endregion

    /// <summary>
    /// Симулятор мыши. Обёртка над WindowsInput.MouseSimulator .
    /// </summary>
    public class MouseSimulatorExt
    {
        #region Fields

        private readonly IMouseSimulator mouseSimulator;

        #endregion

        #region Constructors and Destructors

        internal MouseSimulatorExt(IMouseSimulator mouseSimulator)
        {
            this.mouseSimulator = mouseSimulator;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Текущее положение курсора.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Reviewed.")]
        public Point CurrentCursorPos
        {
            get
            {
                var currentPoint = Cursor.Position;
                return new Point(currentPoint.X, currentPoint.Y);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Эмулирует клик в текущем положении курсора.
        /// </summary>
        /// <param name="button">
        /// Целевая кнопка.
        /// </param>
        public void Click(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    this.LeftButtonClick();
                    break;
                case MouseButton.Right:
                    this.RightButtonClick();
                    break;
            }
        }

        /// <summary>
        /// Эмулирует клик в заданные координаты.
        /// </summary>
        /// <param name="button">
        /// Целевая кнопка.
        /// </param>
        /// <param name="x">
        /// Координата точки по оси X.
        /// </param>
        /// <param name="y">
        /// Координата точки по оси Y.
        /// </param>
        public void Click(MouseButton button, double x, double y)
        {
            this.SetCursorPos(x, y);
            this.Click(button);
        }

        /// <summary>
        /// Эмулирует двойной клик в текущем положении курсора.
        /// </summary>
        /// <param name="button">
        /// Целевая кнопка.
        /// </param>
        public void DoubleClick(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    this.LeftButtonDoubleClick();
                    break;
                case MouseButton.Right:
                    this.RightButtonDoubleClick();
                    break;
            }
        }

        /// <summary>
        /// Эмулирует двойной клик в заданные точку.
        /// </summary>
        /// <param name="button">
        /// Целевая кнопка.
        /// </param>
        /// <param name="x">
        /// Координата точки по оси X.
        /// </param>
        /// <param name="y">
        /// Координата точки по оси Y.
        /// </param>
        public void DoubleClick(MouseButton button, double x, double y)
        {
            this.SetCursorPos(x, y);
            this.DoubleClick(button);
        }

        /// <summary>
        /// Эмулярует клик левой кнопки мыши в текущем положении курсора.
        /// </summary>
        public void LeftButtonClick()
        {
            this.mouseSimulator.LeftButtonClick();
            Thread.Sleep(250);
        }

        /// <summary>
        /// Эмулярует двойной клик левой кнопки мыши в текущем положении курсора.
        /// </summary>
        public void LeftButtonDoubleClick()
        {
            this.mouseSimulator.LeftButtonDoubleClick();
            Thread.Sleep(250);
        }

        /// <summary>
        /// Перемещает курсор на заданное смещение по каждой координате.
        /// </summary>
        /// <param name="x">
        /// Смещение по оси X (в пикселях).
        /// </param>
        /// <param name="y">
        /// Смещение по оси Y (в пикселях).
        /// </param>
        public void MoveCursorPos(double x, double y)
        {
            var currentPoint = this.CurrentCursorPos;
            this.SetCursorPos(currentPoint.X + x, currentPoint.Y + y);
        }

        /// <summary>
        /// Эмулярует клик правой кнопки мыши в текущем положении курсора.
        /// </summary>
        public void RightButtonClick()
        {
            this.mouseSimulator.RightButtonClick();
            Thread.Sleep(250);
        }

        /// <summary>
        /// Эмулярует двойной клик правой кнопки мыши в текущем положении курсора.
        /// </summary>
        public void RightButtonDoubleClick()
        {
            this.mouseSimulator.RightButtonDoubleClick();
            Thread.Sleep(250);
        }

        /// <summary>
        /// Устанавливает курсор в заданную точку.
        /// </summary>
        /// <param name="x">
        /// Координата точки по оси X.
        /// </param>
        /// <param name="y">
        /// Координата точки по оси Y.
        /// </param>
        public void SetCursorPos(double x, double y)
        {
            var virtualScreenPoint = ScreenCoordinatesHelper.ScreenPointToVirtualScreenPoint(new Point(x, y));
            this.mouseSimulator.MoveMouseToPositionOnVirtualDesktop(virtualScreenPoint.X, virtualScreenPoint.Y);
            Thread.Sleep(250);
        }

        /// <summary>
        /// Эмулирует вертикальную прокрутку.
        /// </summary>
        /// <param name="amountOfClicks">
        /// Количество кручений в кликах. Положительное значение - вверх, отрицательное - вниз.
        /// </param>
        public void VerticalScroll(int amountOfClicks)
        {
            this.mouseSimulator.VerticalScroll(amountOfClicks);
            Thread.Sleep(250);
        }

        #endregion
    }
}
