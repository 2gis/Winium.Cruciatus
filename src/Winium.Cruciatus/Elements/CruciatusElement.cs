namespace Winium.Cruciatus.Elements
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Windows.Automation;

    using NLog;

    using Winium.Cruciatus.Core;
    using Winium.Cruciatus.Exceptions;
    using Winium.Cruciatus.Extensions;

    #endregion

    /// <summary>
    /// Базовый элемент управления.
    /// </summary>
    public class CruciatusElement : IEquatable<CruciatusElement>
    {
        #region Static Fields

        protected static readonly Logger Logger = CruciatusFactory.Logger;

        #endregion

        #region Fields

        private AutomationElement instance;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Создает экземпляр элемента.
        /// </summary>
        /// <param name="element">
        /// Исходный элемент.
        /// </param>
        public CruciatusElement(CruciatusElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.Instance = element.Instance;
            this.Parent = element;
            this.FindStrategy = element.FindStrategy;
        }

        /// <summary>
        /// Создает экземпляр элемента. Поиск осуществится только при необходимости.
        /// </summary>
        /// <param name="parent">
        /// Родительский элемент.
        /// </param>
        /// <param name="findStrategy">
        /// Стратегия поиска элемента.
        /// </param>
        public CruciatusElement(CruciatusElement parent, By findStrategy)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            this.Parent = parent;
            this.FindStrategy = findStrategy;
        }

        internal CruciatusElement(CruciatusElement parent, AutomationElement element, By findStrategy)
        {
            this.Parent = parent;
            this.Instance = element;
            this.FindStrategy = findStrategy;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Стратегия поиска элемента.
        /// </summary>
        public By FindStrategy { get; internal set; }

        /// <summary>
        /// Возвращает true, если элемент существует, иначе false (например, родительское окно было закрыто).
        /// </summary>
        public bool IsStale
        {
            get
            {
                try
                {
                    this.Instance.GetCurrentPropertyValue(AutomationElement.AutomationIdProperty);
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (ElementNotAvailableException)
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Свойства элемента.
        /// </summary>
        public CruciatusElementProperties Properties
        {
            get
            {
                return new CruciatusElementProperties(this.Instance);
            }
        }

        #endregion

        #region Properties

        internal AutomationElement Instance
        {
            get
            {
                if (this.instance == null)
                {
                    var element = this.Parent.FindElement(this.FindStrategy);
                    this.instance = element != null ? element.Instance : null;
                }

                if (this.instance == null)
                {
                    throw new NoSuchElementException("ELEMENT NOT FOUND");
                }

                return this.instance;
            }

            set
            {
                this.instance = value;
            }
        }

        internal CruciatusElement Parent { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Клик по элементу.
        /// </summary>
        public void Click()
        {
            this.Click(CruciatusFactory.Settings.ClickButton);
        }

        /// <summary>
        /// Клик по элементу.
        /// </summary>
        /// <param name="button">
        /// Используемая кнопка мыши.
        /// </param>
        public void Click(MouseButton button)
        {
            this.Click(button, ClickStrategies.None, false);
        }

        /// <summary>
        /// Клик по элементу.
        /// </summary>
        /// <param name="button">
        /// Используемая кнопка мыши.
        /// </param>
        /// <param name="strategy">
        /// Стратегия клика.
        /// </param>
        public void Click(MouseButton button, ClickStrategies strategy)
        {
            this.Click(button, strategy, false);
        }

        /// <summary>
        /// Клик по элементу.
        /// </summary>
        /// <param name="button">
        /// Используемая кнопка мыши.
        /// </param>
        /// <param name="strategy">
        /// Стратегия клика.
        /// </param>
        /// <param name="doubleClick">
        /// Флаг двойного клика.
        /// </param>
        public void Click(MouseButton button, ClickStrategies strategy, bool doubleClick)
        {
            if (!this.Instance.Current.IsEnabled)
            {
                Logger.Error("Element '{0}' not enabled. Click failed.", this.ToString());
                CruciatusFactory.Screenshoter.AutomaticScreenshotCaptureIfNeeded();
                throw new ElementNotEnabledException("NOT CLICK");
            }

            if (strategy == ClickStrategies.None)
            {
                strategy = ~strategy;
            }

            if (strategy.HasFlag(ClickStrategies.ClickablePoint))
            {
                if (CruciatusCommand.TryClickOnClickablePoint(button, this, doubleClick))
                {
                    return;
                }
            }

            if (strategy.HasFlag(ClickStrategies.BoundingRectangleCenter))
            {
                if (CruciatusCommand.TryClickOnBoundingRectangleCenter(button, this, doubleClick))
                {
                    return;
                }
            }

            if (strategy.HasFlag(ClickStrategies.InvokePattern))
            {
                if (CruciatusCommand.TryClickUsingInvokePattern(this, doubleClick))
                {
                    return;
                }
            }

            Logger.Error("Click on '{0}' element failed", this.ToString());
            throw new CruciatusException("NOT CLICK");
        }

        /// <summary>
        /// Двойной клик по элементу.
        /// </summary>
        public void DoubleClick()
        {
            this.DoubleClick(CruciatusFactory.Settings.ClickButton);
        }

        /// <summary>
        /// Двойной клик по элементу.
        /// </summary>
        /// <param name="button">
        /// Используемая кнопка мыши.
        /// </param>
        public void DoubleClick(MouseButton button)
        {
            this.DoubleClick(button, ClickStrategies.None);
        }

        /// <summary>
        /// Двойной клик по элементу.
        /// </summary>
        /// <param name="button">
        /// Используемая кнопка мыши.
        /// </param>
        /// <param name="strategy">
        /// Стратегия клика.
        /// </param>
        public void DoubleClick(MouseButton button, ClickStrategies strategy)
        {
            this.Click(button, strategy, true);
        }

        public bool Equals(CruciatusElement other)
        {
            return other != null && this.Instance.Equals(other.Instance);
        }

        public override bool Equals(object obj)
        {
            var cruciatusElement = obj as CruciatusElement;
            return cruciatusElement != null && this.Equals(cruciatusElement);
        }

        /// <summary>
        /// Поиск элемента.
        /// Возвращает целевой элемент, либо null, если он не найден.
        /// </summary>
        /// <param name="strategy">
        /// Стратегия поиска.
        /// </param>
        public virtual CruciatusElement FindElement(By strategy)
        {
            return CruciatusCommand.FindFirst(this, strategy);
        }

        /// <summary>
        /// Поиск элемента по Name.
        /// Возвращает целевой элемент, либо null, если он не найден.
        /// </summary>
        /// <param name="value">
        /// Имя элемента.
        /// </param>
        public virtual CruciatusElement FindElementByName(string value)
        {
            return this.FindElement(By.Name(value));
        }

        /// <summary>
        /// Поиск элемента по AutomationId.
        /// Возвращает целевой элемент, либо null, если он не найден.
        /// </summary>
        /// <param name="value">
        /// Уникальный идентификатор элемента.
        /// </param>
        public virtual CruciatusElement FindElementByUid(string value)
        {
            return this.FindElement(By.Uid(value));
        }

        /// <summary>
        /// Поиск элементов.
        /// Возвращает целевой элемент, либо null, если он не найден.
        /// </summary>
        /// <param name="strategy">
        /// Стратегия поиска.
        /// </param>
        public IEnumerable<CruciatusElement> FindElements(By strategy)
        {
            return CruciatusCommand.FindAll(this, strategy);
        }

        public override int GetHashCode()
        {
            return this.Instance.GetHashCode();
        }

        /// <summary>
        /// Устанавливает фокус на элементе.
        /// Если элемент - окно и оно было свёрнуто, то разворачивает его.
        /// </summary>
        public void SetFocus()
        {
            if (!this.Instance.Current.IsEnabled)
            {
                Logger.Error("Element '{0}' not enabled. Set focus failed.", this.ToString());
                CruciatusFactory.Screenshoter.AutomaticScreenshotCaptureIfNeeded();
                throw new ElementNotEnabledException("NOT SET FOCUS");
            }

            if (this.Instance.Current.ControlType.Equals(ControlType.Window))
            {
                object windowPatternObject;
                if (this.Instance.TryGetCurrentPattern(WindowPattern.Pattern, out windowPatternObject))
                {
                    ((WindowPattern)windowPatternObject).SetWindowVisualState(WindowVisualState.Normal);
                    return;
                }
            }

            try
            {
                this.Instance.SetFocus();
            }
            catch (InvalidOperationException exception)
            {
                Logger.Error("Set focus on element '{0}' failed.", this.ToString());
                Logger.Debug(exception);
                throw new CruciatusException("NOT SET FOCUS");
            }
        }

        /// <summary>
        /// Установка текста.
        /// </summary>
        /// <param name="text">
        /// Целевой текст.
        /// </param>
        public void SetText(string text)
        {
            if (!this.Instance.Current.IsEnabled)
            {
                Logger.Error("Element '{0}' not enabled. Set text failed.", this.ToString());
                CruciatusFactory.Screenshoter.AutomaticScreenshotCaptureIfNeeded();
                throw new ElementNotEnabledException("NOT SET TEXT");
            }

            this.Click(MouseButton.Left, ClickStrategies.ClickablePoint | ClickStrategies.BoundingRectangleCenter);

            CruciatusFactory.Keyboard.SendCtrlA().SendBackspace().SendText(text);
        }

        /// <summary>
        /// Возвращает текст элемента.
        /// </summary>
        public string Text()
        {
            return this.Text(GetTextStrategies.None);
        }

        /// <summary>
        /// Возвращает текст элемента.
        /// </summary>
        /// <param name="strategy">
        /// Стратегия получения элемента.
        /// </param>
        public string Text(GetTextStrategies strategy)
        {
            if (strategy == GetTextStrategies.None)
            {
                strategy = ~strategy;
            }

            string text;
            if (strategy.HasFlag(GetTextStrategies.TextPattern))
            {
                if (CruciatusCommand.TryGetTextUsingTextPattern(this, out text))
                {
                    return text;
                }
            }

            if (strategy.HasFlag(GetTextStrategies.ValuePattern))
            {
                if (CruciatusCommand.TryGetTextUsingValuePattern(this, out text))
                {
                    return text;
                }
            }

            Logger.Error("Get text from '{0}' element failed.", this.ToString());
            CruciatusFactory.Screenshoter.AutomaticScreenshotCaptureIfNeeded();
            throw new CruciatusException("NO GET TEXT");
        }

        /// <summary>
        /// Возвращает строковое представление элемента.
        /// </summary>
        public override string ToString()
        {
            var typeName = this.Instance.Current.ControlType.ProgrammaticName;
            var uid = this.Instance.Current.AutomationId;
            var name = this.Instance.Current.Name;
            var str = string.Format(
                "{0}{1}{2}", 
                "type: " + typeName, 
                string.IsNullOrEmpty(uid) ? string.Empty : ", uid: " + uid, 
                string.IsNullOrEmpty(name) ? string.Empty : ", name: " + name);
            return str;
        }

        #endregion
    }
}
