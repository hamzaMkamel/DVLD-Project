using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


public static class DataAccessSettings
{
    public static string ConnectionString = "Server=.;Database=DVLD;User Id=sa;Password=123456;";

    public static void SaveLogToFileLog(string Message)
    {
        string fileName = "AppErrorLogs";

        string appDirectory = AppDomain.CurrentDomain.BaseDirectory; //file is saved in project files to ensure to work on any device
        string fullpath = Path.Combine(appDirectory, fileName);

        if(File.Exists(fullpath))
        {
            File.AppendAllText(fullpath,Message + "|"+ DateTime.Now.ToString() + "\n");
        }
        else
        {
            File.WriteAllText(fullpath, Message + "|" + DateTime.Now.ToString() + "\n");
        }



    }
}

