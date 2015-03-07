using System;
using System.Diagnostics;
using System.IO;

namespace Launcher
{
    public class Logging
    {
        public static string GenerateDefaultLogFileName(string BaseFileName)
        {
            return (Environment.GetEnvironmentVariable("TEMP") + @"\" + BaseFileName);
        }

        public static void LogInfo(string strInfo)
        {
            WriteToLog(GenerateDefaultLogFileName("PCPrime.txt"), strInfo);
        }

        public static void WriteToEventLog(string Source, string Message, EventLogEntryType EntryType)
        {
            try
            {
                if (!EventLog.SourceExists(Source))
                {
                    EventLog.CreateEventSource(Source, "Application");
                }
                EventLog.WriteEntry(Source, Message, EntryType);
            }
            catch (Exception)
            {
            }
        }

        public static void WriteToLog(string LogPath, string Message)
        {
            try
            {
                using (StreamWriter writer = File.AppendText(LogPath))
                {
                    writer.WriteLine(DateTime.Now + "\t" + Message);
                    writer.WriteLine();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}

