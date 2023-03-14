// TODO: implement the LogService class from the ILogService interface.
//       One explicit requirement - for the read method, if the file is not found, an InvalidOperationException should be thrown
//       Other implementation details are up to you, they just have to match the interface requirements
//       and tests, for example, in LogServiceTests you can find the necessary constructor format.

using System;
using System.IO;
using CoolParking.BL.Interfaces;

public class LogService : ILogService
{
    public LogService(string logPath)
    {
        LogPath = logPath;
    }

    public string LogPath
    {
        get
        {
            return _logPath;
        }

        private set
        {
            _logPath = value;
        }
    }

    public string Read()
    {
        if (!File.Exists(LogPath))
            throw new InvalidOperationException();

        string logData = default;

        using (StreamReader sr = new StreamReader(_logPath))
        {
            logData = sr.ReadToEnd();
            sr.Close();
        }

        return logData;
    }

    public void Write(string logInfo)
    {
        if (logInfo == default(string)) return;

        using(StreamWriter sw = new StreamWriter(LogPath, true))
        {
            sw.WriteLine(logInfo);
            sw.Close();
        }
    }

    private string _logPath;
}
