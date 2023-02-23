using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OdiumDev
{
    public static class Logger
    {
        /// <summary>
        /// A lock object to prevent multiple threads from writing to the log file at the same time.
        /// </summary>
        private static readonly object _lockObject = new object();
        /// <summary>
        /// A list of all log messages
        /// </summary>
        private static readonly List<string> _logMessages = new List<string>();

        /// <summary>
        /// Creates a log for each error or information message.
        /// </summary>
        /// <param name="level">Provide 'error' as param for crashdump or just put string empty for no crash dump.</param>
        /// <param name="moduleName">Module that threw the error. (Ex. Form1). Its really just an identifier for the specific function that caused the error.</param>
        /// <param name="message">The error message to be supplied, this shows in the log file and also the crashdump.</param>
        /// <param name="errorException">This is an optional parameter, include this parameter if you want a crashdump otherwise it won't generate one.</param>
        public static void Log(string level, string moduleName, string message, [Optional] Exception errorException)
        {
            lock (_lockObject)
            {
                string threadName = GetThreadName();
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm}] [{threadName}] [{level.ToUpper()}] [{moduleName}] {message}";

                _logMessages.Add(logMessage);

                Console.WriteLine(logMessage);

                // Log the message to the log file
                File.AppendAllText("log.txt", logMessage + Environment.NewLine);

                if (level.ToUpper() == "ERROR")
                {
                    try
                    {
                        if (errorException != null)
                        {
                            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\CrashDumps\\"))
                            {
                                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\CrashDumps\\");
                            }
                            // Create a crash dump file with the current date and time as the filename
                            string crashDumpFileName = Directory.GetCurrentDirectory() + $"\\CrashDumps\\crash_dump_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.crdmp";
                            FileStream fs = new FileStream(crashDumpFileName, FileMode.Create);
                            StreamWriter sw = new StreamWriter(fs);

                            // Write the stack trace and error information to the crash dump file
                            Exception ex = new Exception(message);
                            sw.WriteLine($"Exception Message: {ex.Message}");
                            sw.WriteLine($"Exception Stack Trace: {ex.StackTrace}");
                            sw.Close();
                            fs.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error creating crash dump file: {ex.Message}");
                    }
                }
            }
        }
        /// <summary>
        /// Allows the user to copy log file to a custom directory. (Does not delete the original file).
        /// </summary>
        /// <param name="path">Specify the path you want to copy it to.</param>
        public static void SaveLogs(string path)
        {
            lock (_lockObject)
            {
                try
                {
                    // Write the contents of the log file to the specified path
                    File.Copy("log.txt", path, true);
                    Console.WriteLine($"Log file saved to {path}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving log file: {ex.Message}");
                }
            }
        }
        /// <summary>
        /// Gets the thread name of the current thread.
        /// </summary>
        /// <returns>Returns the name of the thread that caused the error.</returns>
        private static string GetThreadName()
        {
            string threadId = Thread.CurrentThread.Name;
            return $"{threadId}";
        }
    }
}
