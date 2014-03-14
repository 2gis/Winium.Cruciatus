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
    using System.Windows.Automation;

    using Cruciatus.Elements;
    using Cruciatus.Interfaces;

    /// <summary>
    /// Представляет объект приложение.
    /// </summary>
    /// <typeparam name="T">
    /// Главное окно.
    /// </typeparam>
    public abstract class Application<T> where T : Window, IContainerElement, new()
    {
        private readonly bool isClickOnceApplication;

        private readonly string exeFileName;

        private readonly string clickOnceFileName;

        private readonly string pidFileName;

        private readonly string mainWindowAutomationId;

        private readonly Dictionary<string, object> objects = new Dictionary<string, object>();

        private Process process;

        private T mainWindow;

        private AutomationElement mainWindowElement;

        /// <summary>
        /// Инициализирует новый экземпляр приложения.
        /// </summary>
        /// <param name="exeFileName">
        /// Полный путь к исполняемому файлу приложения.
        /// </param>
        /// <param name="mainWindowAutomationId">
        /// Уникальный идентификатор главного окна приложения.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Входные параметры не должны быть нулевыми.
        /// </exception>
        protected Application(string exeFileName, string mainWindowAutomationId)
        {
            if (exeFileName == null)
            {
                throw new ArgumentNullException("exeFileName");
            }

            if (mainWindowAutomationId == null)
            {
                throw new ArgumentNullException("mainWindowAutomationId");
            }

            this.exeFileName = exeFileName;
            this.mainWindowAutomationId = mainWindowAutomationId;
        }

        /// <summary>
        /// Инициализирует новый экземпляр ClickOnce приложения.
        /// </summary>
        /// <param name="clickOnceFileName">
        /// Полный путь к ярлыку *.appref-ms.
        /// </param>
        /// <param name="pidFileName">
        /// Полный путь к файлу с PID запущенного приложения.
        /// </param>
        /// <param name="mainWindowAutomationId">
        /// Уникальный идентификатор главного окна ClickOnce приложения.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Входные параметры не должны быть нулевыми.
        /// </exception>
        protected Application(string clickOnceFileName, string pidFileName, string mainWindowAutomationId)
        {
            if (clickOnceFileName == null)
            {
                throw new ArgumentNullException("clickOnceFileName");
            }

            if (pidFileName == null)
            {
                throw new ArgumentNullException("pidFileName");
            }

            if (mainWindowAutomationId == null)
            {
                throw new ArgumentNullException("mainWindowAutomationId");
            }

            this.clickOnceFileName = clickOnceFileName;
            this.pidFileName = pidFileName;
            this.mainWindowAutomationId = mainWindowAutomationId;
            this.isClickOnceApplication = true;
        }

        public T MainWindow
        {
            get
            {
                if (this.process == null)
                {
                    return null;
                }

                if (this.mainWindow == null)
                {
                    this.mainWindow = new T();
                    this.mainWindow.Initialize(this.mainWindowElement, this.mainWindowAutomationId);
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
            if (this.isClickOnceApplication)
            {
                int pid;

                // Если файл с PID есть, то приложение запущено и его надо завершить
                if (File.Exists(this.pidFileName))
                {
                    // Считывание PID
                    if (!this.GetPid(out pid))
                    {
                        throw new Exception("Не удалось прочитать PID приложения.");
                    }

                    // Завершение процесса
                    bool isExit;
                    try
                    {
                        var proc = Process.GetProcessById(pid);
                        proc.CloseMainWindow();
                        isExit = proc.WaitForExit(milliseconds);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Системная ошибка при завершении старого экземпляра приложения.", e);
                    }

                    if (!isExit)
                    {
                        throw new Exception("Не удалось завершить старый экземпляр приложения.");
                    }

                    // Ожидание удаления файла с информацией о PID (что так же говорит о завершении приложения)
                    var isNotClose = CruciatusFactory.WaitingValues(
                                       () => File.Exists(this.pidFileName),
                                       value => value,
                                       milliseconds);
                    if (isNotClose)
                    {
                        throw new Exception("Не удалось завершить старый экземпляр приложения (файл с PID не удален).");
                    }
                }

                // Запуск ClickOnce сервиса
                var clickOnceApp = Process.Start(this.clickOnceFileName);
                if (clickOnceApp == null)
                {
                    throw new Exception("Не удалось запустить ClickOnce сервис.");
                }

                // Ожидание завершения ClickOnce сервиса
                if (!clickOnceApp.WaitForExit(milliseconds))
                {
                    throw new Exception("Не удалось завершить ClickOnce сервис.");
                }

                // Ожидание создания файла с информацией о PID (что так же говорит о запуске приложения)
                var isStart = CruciatusFactory.WaitingValues(
                                   () => File.Exists(this.pidFileName),
                                   value => value == false,
                                   milliseconds);
                if (isStart == false)
                {
                    throw new Exception("Не удалось запустить приложение (файл с PID не создан).");
                }

                // Считывание PID
                if (!this.GetPid(out pid))
                {
                    throw new Exception("Не удалось прочитать PID приложения.");
                }

                // Получение процесса приложения по его PID
                try
                {
                    this.process = Process.GetProcessById(pid);
                }
                catch (Exception e)
                {
                    throw new Exception("Системная ошибка при связывании с экземпляром приложения.", e);
                }
            }
            else
            {
                // Запуск приложения через исполняемый файл
                this.process = Process.Start(this.exeFileName);
            }

            // Проверка, что имеем процесс приложения
            if (this.process == null)
            {
                throw new Exception("Не удалось запустить приложение.");
            }

            // Ожидание открытия главного окна
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

        protected TU GetElement<TU>(string headerName) where TU : Window, IContainerElement, new()
        {
            if (!this.objects.ContainsKey(headerName))
            {
                var item = new TU();
                item.Initialize(this.mainWindowElement, headerName);
                this.objects.Add(headerName, item);
            }

            return (TU)this.objects[headerName];
        }

        private bool GetPid(out int pid)
        {
            using (var fs = File.Open(this.pidFileName, FileMode.Open))
            {
                using (var sr = new StreamReader(fs))
                {
                    if (sr.EndOfStream)
                    {
                        pid = 0;
                        return false;
                    }

                    var strPid = sr.ReadLine();
                    if (!int.TryParse(strPid, out pid))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
