using inRiver.Remoting.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inRiver.Remoting.Log;

namespace ExtensionTester
{
    public class ConsoleLogger: IExtensionLog
    {
    public void Log(LogLevel logLevel, string message)
        {
            Console.WriteLine($"{logLevel} - {message}");
        }
        public void Log(LogLevel logLevel, string message, Exception ex)
        {
            Log(logLevel, message);
            Console.WriteLine();
            Console.WriteLine(ex.ToString());
            Console.WriteLine();

        }
    }
}
