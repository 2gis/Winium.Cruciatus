﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Menu.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент меню.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus.Elements
{
    #region using

    using System;
    using System.Linq;

    using Cruciatus.Core;
    using Cruciatus.Exceptions;

    #endregion

    /// <summary>
    /// Представляет элемент управления меню.
    /// </summary>
    public class Menu : CruciatusElement
    {
        public Menu(CruciatusElement element)
            : base(element)
        {
        }

        public Menu(CruciatusElement parent, By getStrategy)
            : base(parent, getStrategy)
        {
        }

        /// <summary>
        /// Выбирает элемент меню, указанный последним в пути.
        /// </summary>
        /// <param name="headersPath">
        /// Путь из заголовков для прохода (пример: control$view$zoom).
        /// </param>
        public virtual void SelectItem(string headersPath)
        {
            if (!Instanse.Current.IsEnabled)
            {
                Logger.Error("Element '{0}' not enabled. Select item failed.", ToString());
                throw new CruciatusException("NOT SELECT ITEM");
            }

            var item = GetItem(headersPath);
            if (item == null)
            {
                Logger.Error("Item '{0}' not found. Select item failed.", headersPath);
                throw new CruciatusException("NOT SELECT ITEM");
            }

            item.Click();
        }

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
                item = item.Get(By.Name(name));
                if (item == null)
                {
                    Logger.Error("Item '{0}' not found. Get item failed.", name);
                    throw new CruciatusException("NOT GET ITEM");
                }

                item.Click();
            }

            return item.Get(By.Name(headers.Last()));
        }
    }
}
