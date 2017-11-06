﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MusicMetadataOrganizer
{
    public class LogWriter
    {
        private string m_exePath = "";
        public LogWriter(string logMessage)
        {
            LogWrite(logMessage);
        }

        public void LogWrite(string logMessage)
        {
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                using (StreamWriter writer = File.AppendText(m_exePath + "\\" + "log.txt"))
                {
                    Log(logMessage, writer);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not create an error log in {m_exePath}. \"{ex.Message}\"");
            }
        }

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine($"  :{logMessage}");
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not write to error log in {m_exePath}. \"{ex.Message}\"");
            }
        }
    }
}
