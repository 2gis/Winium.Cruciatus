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
    using System.IO;
    using System.Linq;
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

        /// <summary>
        /// Инициализирует новый экземпляр приложения.
        /// </summary>
        /// <param name="fullPath">
        /// Полный путь к исполняемому файлу *.exe (к *.appref-ms в случае ClickOnce приложения).
        /// </param>
        /// <param name="mainWindowAutomationId">
        /// Уникальный идентификатор главного окна приложения.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Входные параметры не должны быть нулевыми.
        /// </exception>
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

        /// <summary>
        /// Запуск приложения с временем ожидания по умолчанию.
        /// </summary>
        /// <returns>
        /// Значение true если запуск приложения удался; в противном случае значение - false.
        /// </returns>
        /// <exception cref="Exception">
        /// Произвести процесс запуска приложения не удалось.
        /// </exception>
        public bool Start()
        {
            return this.Start(CruciatusFactory.Settings.SearchTimeout);
        }

        /// <summary>
        /// Запуск приложения с временем ожидания по умолчанию.
        /// </summary>
        /// <param name="milliseconds">
        /// Задает время ожидания запуска приложения.
        /// </param>
        /// <returns>
        /// Значение true если запуск приложения удался; в противном случае значение - false.
        /// </returns>
        /// <exception cref="Exception">
        /// Произвести процесс запуска приложения не удалось.
        /// </exception>
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

        /// <summary>
        /// Запуск ClickOnce приложения с временем ожидания по умолчанию (необходимо указать имя процесса приложения).
        /// </summary>
        /// <param name="processName">
        /// Имя процесса.
        /// </param>
        /// <returns>
        /// Значение true если запуск ClickOnce приложения удался; в противном случае значение - false.
        /// </returns>
        /// <exception cref="Exception">
        /// Произвести процесс запуска ClickOnce приложения не удалось.
        /// </exception>
        public bool StartClickOnce(string processName)
        {
            return this.StartClickOnce(processName, CruciatusFactory.Settings.SearchTimeout);
        }

        /// <summary>
        /// Запуск ClickOnce приложения с заданным временем ожидания (необходимо указать имя процесса приложения).
        /// </summary>
        /// <param name="processName">
        /// Имя процесса.
        /// </param>
        /// <param name="milliseconds">
        /// Задает время ожидания запуска ClickOnce приложения.
        /// </param>
        /// <returns>
        /// Значение true если запуск ClickOnce приложения удался; в противном случае значение - false.
        /// </returns>
        /// <exception cref="Exception">
        /// Произвести процесс запуска ClickOnce приложения не удалось.
        /// </exception>
        public bool StartClickOnce(string processName, int milliseconds)
        {
            // Условие для искомого процесса
            var func = new Func<Process, bool>(p => p.ProcessName == processName);
            
            // Получаем существующие процессы с полученным именем и убиваем их
            var processes = Process.GetProcesses().ToList().Where(func);
            foreach (var proc in processes)
            {
                proc.Kill();
                if (!proc.WaitForExit(milliseconds))
                {
                    throw new Exception("Не удалось убить старые экземпляры приложений.");
                }
            }

            // Запуск ClickOnce приложения
            var clickOnceApp = Process.Start(this.fullPath);
            if (clickOnceApp == null)
            {
                throw new Exception("Не удалось запустить ClickOnce приложение.");
            }

            // Ожидание завершения ClickOnce приложения
            clickOnceApp.WaitForExit();

            // Ожидаем появление подходящего процесса
            this.process = CruciatusFactory.WaitingValues(
                               () => Process.GetProcesses().ToList().FirstOrDefault(func),
                               value => value == null,
                               milliseconds);
            if (this.process == null)
            {
                throw new Exception("Не удалось запустить приложение.");
            }

            // Ожидаем открытие главного окна
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
