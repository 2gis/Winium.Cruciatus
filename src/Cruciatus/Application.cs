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
    #region using

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Automation;

    using Cruciatus.Elements;
    using Cruciatus.Exceptions;
    using Cruciatus.Interfaces;

    #endregion

    /// <summary>
    /// Представляет объект приложение.
    /// </summary>
    /// <typeparam name="T">
    /// Главное окно.
    /// </typeparam>
    public class Application<T> where T : Window, IContainerElement, new()
    {
        private readonly Dictionary<string, object> _childrenDictionary = new Dictionary<string, object>();

        private readonly string _clickOnceFileName;

        private readonly string _exeFileName;

        private readonly bool _isClickOnceApplication;

        private readonly string _mainWindowAutomationId;

        private readonly string _pidFileName;

        private T _mainWindow;

        private AutomationElement _mainWindowElement;

        private Process _process;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Application{T}"/>.
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
        public Application(string exeFileName, string mainWindowAutomationId)
        {
            if (exeFileName == null)
            {
                throw new ArgumentNullException("exeFileName");
            }

            if (mainWindowAutomationId == null)
            {
                throw new ArgumentNullException("mainWindowAutomationId");
            }

            _exeFileName = exeFileName;
            _mainWindowAutomationId = mainWindowAutomationId;
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
        public Application(string clickOnceFileName, string pidFileName, string mainWindowAutomationId)
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

            _clickOnceFileName = clickOnceFileName;
            _pidFileName = pidFileName;
            _mainWindowAutomationId = mainWindowAutomationId;
            _isClickOnceApplication = true;
        }

        public T MainWindow
        {
            get
            {
                if (_process == null)
                {
                    return null;
                }

                if (_mainWindow == null)
                {
                    _mainWindow = new T { ElementInstance = _mainWindowElement, AutomationId = _mainWindowAutomationId };
                }

                return _mainWindow;
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
            return Start(CruciatusFactory.Settings.SearchTimeout);
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
            if (_isClickOnceApplication)
            {
                int pid;

                // Если файл с PID есть, то приложение запущено и его надо завершить
                if (File.Exists(_pidFileName))
                {
                    // Считывание PID
                    if (!GetPid(out pid))
                    {
                        throw new CruciatusException("Не удалось прочитать PID приложения.");
                    }

                    // Завершение процесса
                    bool isExit;
                    try
                    {
                        var proc = Process.GetProcessById(pid);
                        proc.CloseMainWindow();
                        isExit = proc.WaitForExit(milliseconds);
                    }
                    catch (Exception exc)
                    {
                        const string message = "Системная ошибка при завершении старого экземпляра приложения.";
                        throw new CruciatusException(message, exc);
                    }

                    if (!isExit)
                    {
                        throw new CruciatusException("Не удалось завершить старый экземпляр приложения.");
                    }

                    // Ожидание удаления файла с информацией о PID (что так же говорит о завершении приложения)
                    var isNotClose = CruciatusFactory.WaitingValues(
                        () => File.Exists(_pidFileName), 
                        value => value, 
                        milliseconds);
                    if (isNotClose)
                    {
                        throw new CruciatusException(
                            "Не удалось завершить старый экземпляр приложения (файл с PID не удален).");
                    }
                }

                // Запуск ClickOnce сервиса
                var clickOnceApp = Process.Start(_clickOnceFileName);
                if (clickOnceApp == null)
                {
                    throw new CruciatusException("Не удалось запустить ClickOnce сервис.");
                }

                // Ожидание завершения ClickOnce сервиса
                if (!clickOnceApp.WaitForExit(milliseconds))
                {
                    throw new CruciatusException("Не удалось завершить ClickOnce сервис.");
                }

                // Ожидание создания файла с информацией о PID (что так же говорит о запуске приложения)
                var isStart = CruciatusFactory.WaitingValues(
                    () => File.Exists(_pidFileName), 
                    value => value == false, 
                    milliseconds);
                if (isStart == false)
                {
                    throw new CruciatusException("Не удалось запустить приложение (файл с PID не создан).");
                }

                // Считывание PID
                if (!GetPid(out pid))
                {
                    throw new CruciatusException("Не удалось прочитать PID приложения.");
                }

                // Получение процесса приложения по его PID
                try
                {
                    _process = Process.GetProcessById(pid);
                }
                catch (Exception exc)
                {
                    throw new CruciatusException("Системная ошибка при связывании с экземпляром приложения.", exc);
                }
            }
            else
            {
                if (File.Exists(_exeFileName))
                {
                    // Запуск приложения через исполняемый файл
                    _process = Process.Start(_exeFileName);
                }
                else
                {
                    throw new CruciatusException("Неверно задан путь до исполняемого файла приложения.");
                }
            }

            // Проверка, что имеем процесс приложения
            if (_process == null)
            {
                throw new CruciatusException("Не удалось запустить приложение.");
            }

            // Ожидание открытия главного окна
            _mainWindowElement = CruciatusFactory.WaitingValues(
                () => WindowFactory.GetMainWindowElement(_process.Id, _mainWindowAutomationId), 
                value => value == null, 
                milliseconds);
            return _mainWindowElement != null;
        }

        public bool Close()
        {
            var isClosed = CruciatusFactory.WaitingValues(
                () => _process.CloseMainWindow(), 
                value => value == false);

            if (!isClosed)
            {
                return false;
            }

            if (!_process.WaitForExit(CruciatusFactory.Settings.WaitForExitTimeout))
            {
                return false;
            }

            _process.Close();
            return true;
        }

        public bool Kill()
        {
            _process.Kill();
            return _process.WaitForExit(CruciatusFactory.Settings.WaitForExitTimeout);
        }

        public TU GetElement<TU>(string headerName) where TU : Window, new()
        {
            if (!_childrenDictionary.ContainsKey(headerName))
            {
                var item = new TU { Parent = _mainWindowElement, AutomationId = headerName };
                _childrenDictionary.Add(headerName, item);
            }

            return (TU)_childrenDictionary[headerName];
        }

        private bool GetPid(out int pid)
        {
            Stream fs = null;
            try
            {
                fs = File.Open(_pidFileName, FileMode.Open);
                using (var sr = new StreamReader(fs))
                {
                    fs = null;
                    if (sr.EndOfStream)
                    {
                        pid = 0;
                        return false;
                    }

                    var strPid = sr.ReadLine();
                    return int.TryParse(strPid, out pid);
                }
            }
            finally
            {
                if (fs != null)
                {
                    fs.Dispose();
                }
            }
        }
    }
}
