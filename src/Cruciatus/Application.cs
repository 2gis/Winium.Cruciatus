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
    using System.Diagnostics;
    using System.IO;

    using Cruciatus.Exceptions;

    #endregion

    /// <summary>
    /// Представляет объект приложение.
    /// </summary>
    public class Application
    {
        private readonly string _clickOnceFileName;

        private readonly string _exeFileName;

        private readonly bool _isClickOnceApplication;

        private readonly string _pidFileName;

        private Process _process;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Application"/>.
        /// </summary>
        /// <param name="exeFileName">
        /// Полный путь к исполняемому файлу приложения.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Входные параметры не должны быть нулевыми.
        /// </exception>
        public Application(string exeFileName)
        {
            if (exeFileName == null)
            {
                throw new ArgumentNullException("exeFileName");
            }

            _exeFileName = exeFileName;
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
        /// <exception cref="ArgumentNullException">
        /// Входные параметры не должны быть нулевыми.
        /// </exception>
        public Application(string clickOnceFileName, string pidFileName)
        {
            if (clickOnceFileName == null)
            {
                throw new ArgumentNullException("clickOnceFileName");
            }

            if (pidFileName == null)
            {
                throw new ArgumentNullException("pidFileName");
            }

            _clickOnceFileName = clickOnceFileName;
            _pidFileName = pidFileName;
            _isClickOnceApplication = true;
        }

        /// <summary>
        /// Запуск приложения с временем ожидания по умолчанию.
        /// </summary>
        /// <exception cref="Exception">
        /// Произвести процесс запуска приложения не удалось.
        /// </exception>
        public void Start()
        {
            Start(CruciatusFactory.Settings.SearchTimeout);
        }

        /// <summary>
        /// Запуск приложения с временем ожидания по умолчанию.
        /// </summary>
        /// <param name="milliseconds">
        /// Задает время ожидания запуска приложения.
        /// </param>
        /// <exception cref="Exception">
        /// Произвести процесс запуска приложения не удалось.
        /// </exception>
        public void Start(int milliseconds)
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
                    var directory = Path.GetDirectoryName(_exeFileName);

                    // ReSharper disable once AssignNullToNotNullAttribute
                    // directory не может быть null, в связи с проверкой выше наличия файла _exeFileName
                    var info = new ProcessStartInfo { FileName = _exeFileName, WorkingDirectory = directory };
                    _process = Process.Start(info);
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
