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
    using System.Threading;
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
        private readonly string fullPath;

        private readonly string mainWindowAutomationId;

        private readonly Dictionary<string, object> objects = new Dictionary<string, object>();

        private Process process;

        private T mainWindow;

        private AutomationElement mainWindowElement;

        protected Application(string fullPath, string mainWindowAutomationId)
        {
            if (fullPath == null)
            {
                throw new ArgumentNullException("fullPath");
            }

            if (mainWindowAutomationId == null)
            {
                throw new ArgumentNullException("mainWindowAutomationId");
            }

            this.fullPath = fullPath;
            this.mainWindowAutomationId = mainWindowAutomationId;
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
                    this.mainWindow = new T();
                    this.mainWindow.LazyInitialize(this.mainWindowElement, this.mainWindowAutomationId);
                }

                return this.mainWindow;
            }
        }

        public bool Start()
        {
            return this.Start(CruciatusFactory.Settings.SearchTimeout);
        }

        public bool Start(int milliseconds)
        {
            this.process = Process.Start(this.fullPath);

            if (this.process == null)
            {
                // TODO: Непонятная ситуация, когда она вообще возможна и что это значит
                throw new Exception("Не удалось запустить процесс");
            }

            this.mainWindowElement = CruciatusFactory.WaitingValues(
                    () => WindowFactory.GetMainWindowElement(this.process.Id, this.mainWindowAutomationId),
                    value => value == null,
                    milliseconds);
            return this.mainWindowElement != null;
        }

        public bool Close()
        {
            var isClosed = CruciatusFactory.WaitingValues(
                    () => this.process.CloseMainWindow(),
                    value => value == false);

            if (!isClosed)
            {
                return false;
            }

            if (!this.process.WaitForExit(CruciatusFactory.Settings.WaitForExitTimeout))
            {
                return false;
            }

            this.process.Close();
            return true;
        }

        public bool Kill()
        {
            this.process.Kill();
            return this.process.WaitForExit(CruciatusFactory.Settings.WaitForExitTimeout);
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
