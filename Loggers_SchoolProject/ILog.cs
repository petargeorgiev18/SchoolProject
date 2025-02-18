using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loggers_SchoolProject
{
    public interface ILog
    {
        void LogInsertedValues(string tableName, string insertedValues);
        void LogInfo(string message);
        void LogError(string message);
    }
}
