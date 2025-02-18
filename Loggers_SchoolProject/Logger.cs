using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loggers_SchoolProject
{
    public class Logger : ILog
    {
        private string logFilePath = "log.txt";
        public void LogInsertedValues(string tableName, string insertedValues)
        {
            try
            {
                string logMessage = $"Inserted data into table: {tableName}. Values: {insertedValues}";
                LogInfo(logMessage);
            }
            catch (Exception ex)
            {
                LogError($"Error logging inserted values: {ex.Message}");
            }
        }
        public void LogInfo(string message)
        {
            Log("Info", message);
        }
        public void LogError(string message)
        {
            Log("Error", message);
        }
        private void Log(string level, string message)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string logMessage = $"{timestamp} [{level}] {message}";

                using (StreamWriter writer = new StreamWriter(logFilePath, append: true))
                {
                    writer.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging: {ex.Message}");
            }
        }
    }
}
