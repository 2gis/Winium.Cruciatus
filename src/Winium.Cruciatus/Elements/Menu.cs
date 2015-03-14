namespace Winium.Cruciatus.Elements
{
    #region using

    using System;
    using System.Linq;

    using Winium.Cruciatus.Core;
    using Winium.Cruciatus.Exceptions;
    using Winium.Cruciatus.Extensions;

    #endregion

    /// <summary>
    /// Элемент меню.
    /// </summary>
    public class Menu : CruciatusElement
    {
        #region Constructors and Destructors

        /// <summary>
        /// Создает экземпляр меню.
        /// </summary>
        /// <param name="element">
        /// Исходный элемент.
        /// </param>
        public Menu(CruciatusElement element)
            : base(element)
        {
        }

        /// <summary>
        /// Создает экземпляр меню. Поиск осуществится только при необходимости.
        /// </summary>
        /// <param name="parent">
        /// Родительский элемент.
        /// </param>
        /// <param name="getStrategy">
        /// Стратегия поиска элемента.
        /// </param>
        public Menu(CruciatusElement parent, By getStrategy)
            : base(parent, getStrategy)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Возвращает элемент меню, указанный последним в пути.
        /// </summary>
        /// <param name="headersPath">
        /// Путь из заголовков для прохода (пример: control$view$zoom).
        /// </param>
        public CruciatusElement GetItem(string headersPath)
        {
            if (string.IsNullOrEmpty(headersPath))
            {
                throw new ArgumentNullException("headersPath");
            }

            var item = (CruciatusElement)this;
            var headers = headersPath.Split('$');
            for (var i = 0; i < headers.Length - 1; ++i)
            {
                var name = headers[i];
                item = item.FindElement(By.Name(name));
                if (item == null)
                {
                    Logger.Error("Item '{0}' not found. Find item failed.", name);
                    throw new CruciatusException("NOT GET ITEM");
                }

                item.Click();
            }

            return item.FindElement(By.Name(headers.Last()));
        }

        /// <summary>
        /// Выбирает элемент меню, указанный последним в пути.
        /// </summary>
        /// <param name="headersPath">
        /// Путь из заголовков для прохода (пример: control$view$zoom).
        /// </param>
        public virtual void SelectItem(string headersPath)
        {
            if (!this.Instance.Current.IsEnabled)
            {
                Logger.Error("Element '{0}' not enabled. Select item failed.", this.ToString());
                CruciatusFactory.Screenshoter.AutomaticScreenshotCaptureIfNeeded();
                throw new CruciatusException("NOT SELECT ITEM");
            }

            var item = this.GetItem(headersPath);
            if (item == null)
            {
                Logger.Error("Item '{0}' not found. Select item failed.", headersPath);
                throw new CruciatusException("NOT SELECT ITEM");
            }

            item.Click();
        }

        #endregion
    }
}
