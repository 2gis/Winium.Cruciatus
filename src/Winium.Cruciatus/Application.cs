namespace Winium.Cruciatus
{
    #region using

    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;

    using Winium.Cruciatus.Exceptions;

    #endregion

    /// <summary>
    /// Класс для запуска и завершения приложения.
    /// </summary>
    public class Application
    {
        #region Fields

        private readonly string executableFilePath;

        private Process process;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Создает объект класса.
        /// </summary>
        /// <param name="executableFilePath">
        /// Полный путь до исполняемого файла.
        /// </param>
        public Application(string executableFilePath)
        {
            if (executableFilePath == null)
            {
                throw new ArgumentNullException("executableFilePath");
            }

            if (Path.IsPathRooted(executableFilePath))
            {
                this.executableFilePath = executableFilePath;
            }
            else
            {
                var absolutePath = Path.Combine(Environment.CurrentDirectory, executableFilePath);
                this.executableFilePath = Path.GetFullPath((new Uri(absolutePath)).LocalPath);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Посылает сообщение о закрытии главному окну приложения.
        /// </summary>
        /// <returns>
        /// true если приложение завершилось и false в противном случае.
        /// </returns>
        public bool Close()
        {
            this.process.CloseMainWindow();
            return this.process.WaitForExit(CruciatusFactory.Settings.WaitForExitTimeout);
        }

        /// <summary>
        /// Get exit state of launched application
        /// </summary>
        /// <returns>
        /// true if it's already exit, false if it's still running
        /// </returns>
        public bool HasExited()
        {
            return this.process.HasExited;
        }
        
        /// <summary>
        /// Убивает приложение.
        /// </summary>
        /// <returns>
        /// true если приложение завершилось и false в противном случае.
        /// </returns>
        public bool Kill()
        {
            this.process.Kill();
            return this.process.WaitForExit(CruciatusFactory.Settings.WaitForExitTimeout);
        }

        /// <summary>
        /// Запускает исполняемый файл.
        /// </summary>
        public void Start()
        {
            this.Start(string.Empty);
        }

        /// <summary>
        /// Запускает исполняемый файл с аргументами.
        /// </summary>
        /// <param name="arguments">
        /// Строка аргументов запуска приложения.
        /// </param>
        public void Start(string arguments)
        {
            if (!File.Exists(this.executableFilePath))
            {
                throw new CruciatusException(string.Format(@"Path ""{0}"" doesn't exists", this.executableFilePath));
            }

            var directory = Path.GetDirectoryName(this.executableFilePath);

            // ReSharper disable once AssignNullToNotNullAttribute
            // directory не может быть null, в связи с проверкой выше наличия файла executableFilePath
            var info = new ProcessStartInfo
                           {
                               FileName = this.executableFilePath, 
                               WorkingDirectory = directory, 
                               Arguments = arguments
                           };

            this.process = Process.Start(info);           
        }

        /// <summary>
        /// Update process property by process name
        /// </summary>
        /// <param name="launchedAppProcessName">Launched application process name</param>
        /// <returns>true if launched process is successfully update. false if there is error occurs</returns>
        public void UpdateRunApplicationProcessBy(string launchedAppProcessName)
        {
            if (launchedAppProcessName == String.Empty)
            {
                return;
            }

            var processList = Process.GetProcessesByName(launchedAppProcessName);
            if (processList.Length >= 1)
            {
                // Get running process base on StartTime          
                var processSortedList = processList.OrderBy(p => p.StartTime);
                if ((launchedAppProcessName.ToLower().Equals("iexplore")) || (launchedAppProcessName.ToLower().Equals("firefox")) || (launchedAppProcessName.ToLower().Equals("chrome")))
                {
                    this.process = processSortedList.First();
                }
                else
                {
                    this.process = processSortedList.Last();
                }
            }
            return;
        }
        #endregion
    }
}
