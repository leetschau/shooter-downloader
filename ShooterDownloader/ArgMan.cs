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
        private bool _useListFile = false;
        private bool _removeListFile = false;
        private string _listFilePath = String.Empty;

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
            List<string> fileList = new List<string>();

            foreach (string arg in args)
            {
                if (arg.StartsWith("-lst="))
                {
                    fileList.Clear();
                    _useListFile = true;
                    _listFilePath = ExtractArgValue(arg);
                    if (File.Exists(_listFilePath))
                    {
                        StreamReader reader = new StreamReader(_listFilePath);
                        
                        string line = reader.ReadLine();
                        while (line != null)
                        {
                            fileList.Add(line);
                            line = reader.ReadLine();
                        }
                        reader.Close();
                    }
                }
                else if (arg == "/c")
                {
                    _codeConversion = true;
                }
                else if (arg == "/r")
                {
                    _removeListFile = true;
                }
                else if (!arg.StartsWith("-") && !arg.StartsWith("/"))
                {
                    if (!_useListFile && File.Exists(arg))
                        fileList.Add(arg);
                }
            }

            if (_removeListFile && _listFilePath != null)
            {
                if (File.Exists(_listFilePath))
                    File.Delete(_listFilePath);
            }

            if (fileList.Count > 0)
                _files = fileList.ToArray();
            
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
