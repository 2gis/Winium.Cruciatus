namespace Winium.Cruciatus
{
    #region using

    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Collections.Generic;
    using System.Management;

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

        private string processName;

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
        /// Close child process
        /// </summary>
        /// <param name="child">Input child process to close</param>
        /// <returns>
        /// true if successfully close, otherwise return fail.
        /// </returns>
        public bool Close(Process child)
        {
            child.CloseMainWindow();
            return child.WaitForExit(CruciatusFactory.Settings.WaitForExitTimeout);
        }

        /// <summary>
        /// Return process id of application.
        /// </summary>
        /// <returns>Process id</returns>
        public int GetProcessId()
        {
            return this.process.Id;
        }

        /// <summary>
        /// Return process name of application
        /// </summary>
        /// <returns></returns>
        public string GetProcessName()
        {
            return this.processName;
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
        /// Kill child process
        /// </summary>
        /// <param name="child">Input child process to kill</param>
        /// <returns>
        /// true if successfully kill, otherwise return false
        /// </returns>
        public bool Kill(Process child)
        {
            child.Kill();
            return child.WaitForExit(CruciatusFactory.Settings.WaitForExitTimeout);
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
            this.processName = this.process.ProcessName;
        }

        /// <summary>
        /// Get all children processes of parent one bases on its id.
        /// </summary>
        /// <param name="parentId">Input parent process id</param>
        /// <returns></returns>
        public List<Process> GetChildPrecesses(int parentId)
        {
            var query = "Select * From Win32_Process Where ParentProcessId = "
                    + parentId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();
            List<Process> result = new List<Process>();
            foreach (ManagementObject mo in processList)
            {
                result.Add(Process.GetProcessById(Convert.ToInt32(mo["ProcessID"])));
            }

            return result;
        }

        /// <summary>
        /// Get all running processes by input keyword in their name
        /// </summary>
        /// <param name="keyword">Input keyword to search in name</param>
        /// <returns>List of all running processes meet search condition</returns>
        public List<Process> GetAllPrecessesByName(string keyword)
        {
            ManagementClass MgmtClass = new ManagementClass("Win32_Process");
            var result = new List<Process>();
            foreach (ManagementObject mo in MgmtClass.GetInstances())
            {
                if (mo["Name"].ToString().ToLower().Contains(keyword.ToLower()))
                {
                    result.Add(Process.GetProcessById(Convert.ToInt32(mo["ProcessID"])));
                }
            }
            return result;
        }
        #endregion
    }
}
