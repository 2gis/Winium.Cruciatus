// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Application.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет объект приложение.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Automation;

    using Cruciatus.Elements;

    /// <summary>
    /// Представляет объект приложение.
    /// </summary>
    /// <typeparam name="T">
    /// Главное окно.
    /// </typeparam>
    public abstract class Application<T> where T : Window, new()
    {
        private const int WaitForInputIdleTimeout = 10000;

        private const int WaitForExitTimeout = 10000;

        private readonly string fullPath;

        private Process process = null;

        private T mainWindow;

        private AutomationElement mainWindowElement;

        private Dictionary<string, object> objects = new Dictionary<string, object>();

        protected Application(string fullPath)
        {
            if (fullPath == null)
            {
                throw new ArgumentNullException("fullPath");
            }

            this.fullPath = fullPath;
        }

        public T MainWindow
        {
            get
            {
                if (this.process == null)
                {
                    throw new NullReferenceException("Приложение не запущено.");
                }

                if (this.mainWindow == null)
                {
                    this.mainWindowElement = WindowFactory.GetMainWindowElement(this.process);

                    this.mainWindow = new T();
                    this.mainWindow.LazyInitialize(this.mainWindowElement);
                }

                return this.mainWindow;
            }
        }

        public bool Start(int milliseconds = WaitForInputIdleTimeout)
        {
            this.process = Process.Start(this.fullPath);

            if (this.process == null)
            {
                // TODO: Непонятная ситуация, когда она вообще возможна и что это значит
                throw new Exception("Не удалось запустить процесс");
            }

            return this.process.WaitForInputIdle(milliseconds);
        }

        public bool Close()
        {
            if (!this.process.CloseMainWindow())
            {
                return false;
            }

            if (!this.process.WaitForExit(WaitForExitTimeout))
            {
                return false;
            }

            this.process.Close();
            return true;
        }

        public bool Kill()
        {
            this.process.Kill();
            return this.process.WaitForExit(WaitForExitTimeout);
        }

        protected TU GetElement<TU>(string headerName) where TU : Window, new()
        {
            if (!this.objects.ContainsKey(headerName))
            {
                var item = new TU();
                item.LazyInitialize(this.mainWindowElement, headerName);
                this.objects.Add(headerName, item);
            }

            return (TU)this.objects[headerName];
        }
    }
}
