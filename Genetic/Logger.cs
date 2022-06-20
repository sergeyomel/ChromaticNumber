using System;
using System.IO;

namespace Genetic
{
    public static class Logger
    {
        private static string pathLoggerFile = Path.Combine(Environment.CurrentDirectory + "\\Logger.txt");

        public static void SetPath(string path) 
        { 
            pathLoggerFile = path;
            if (!File.Exists(pathLoggerFile))
            {
                File.Create(pathLoggerFile);
            }
        }

        public static void InitializeLogger()
        {
            if (!File.Exists(pathLoggerFile))
            {
                File.Create(pathLoggerFile);
            }
        }

        public static void ClearFile() => File.WriteAllText(pathLoggerFile, string.Empty);

        public static void PushMessage(Exception e)
        {
            using (StreamWriter stream = File.AppendText(pathLoggerFile))
                stream.WriteLine(" ---" + DateTime.Now.ToString() + " ->\n" +
                                 "    " + e.Message + " in \n" +
                                 "    " + e.StackTrace + "\n");
        }
    }
}
