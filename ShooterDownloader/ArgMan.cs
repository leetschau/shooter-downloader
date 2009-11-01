using System;
using System.Collections.Generic;
using System.IO;

namespace ShooterDownloader
{
    

    class ArgMan
    {
        private static ArgMan _instance = null;
        private static object _instanceLock = new Object();
        private string[] _files = null;
        private bool _codeConversion = false;

        public static ArgMan Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_instanceLock)
                    {
                        if (_instance == null)
                            _instance = new ArgMan();
                    }
                }

                return _instance;
            }
        }

        private ArgMan()
        {
        }

        public string[] Files
        {
            get
            {
                return _files;
            }
        }

        public bool CodeConversionOnly
        {
            get 
            { 
                return _codeConversion; 
            }
        }

        public void ParseArgs(string[] args)
        {
            foreach (string arg in args)
            {
                if (arg.StartsWith("-tmp="))
                {
                    string tmpPath = ExtractArgValue(arg);
                    if (File.Exists(tmpPath))
                    {
                        StreamReader reader = new StreamReader(tmpPath);
                        List<string> fileList = new List<string>();
                        string line = reader.ReadLine();
                        while (line != null)
                        {
                            fileList.Add(line);
                            line = reader.ReadLine();
                        }
                        reader.Close();
                        File.Delete(tmpPath);
                        //SelectPaths(fileList.ToArray());
                        _files = fileList.ToArray();
                    }
                }
                else if (arg == "/c")
                {
                    _codeConversion = true;
                }
            }
        }

        private string ExtractArgValue(string arg)
        {
            int idx = arg.IndexOf('=');
            if ((idx + 1) < arg.Length)
            {
                return arg.Substring(idx + 1);
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
