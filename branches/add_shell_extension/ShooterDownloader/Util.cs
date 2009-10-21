/*
 *   Shooter Subtitle Downloader: Automatic Subtitle Downloader for the http://shooter.cn.
 *   Copyright (C) 2009  John Fung
 *
 *   This program is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU Affero General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU Affero General Public License for more details.
 *
 *   You should have received a copy of the GNU Affero General Public License
 *   along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.IO.Compression;
using System.Runtime.InteropServices;
using org.mozilla.intl.chardet;

namespace ShooterDownloader
{
    class Util
    {
        public static string CaculateFileHash(string filePath)
        {
            string hashString = "";
            FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            long fileLength = file.Length;
            long[] offset = new long[4];
            if (fileLength < 8192)
            {
                //a video file less then 8k? impossible! <-- says SPlayer

            }
            else
            {
                const int BlockSize = 4096;
                const int NumOfSegments = 4;

                offset[3] = fileLength - 8192;
                offset[2] = fileLength / 3;
                offset[1] = fileLength / 3 * 2;
                offset[0] = BlockSize;

                MD5 md5 = new MD5CryptoServiceProvider();

                BinaryReader reader = new BinaryReader(file);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < NumOfSegments; i++)
                {
                    file.Seek(offset[i], SeekOrigin.Begin);
                    byte[] dataBlock = reader.ReadBytes(BlockSize);
                    MD5 md5Crypt = new MD5CryptoServiceProvider();
                    byte[] hash = md5Crypt.ComputeHash(dataBlock);
                    if (sb.Length > 0)
                    {
                        sb.Append(';');
                    }
                    foreach (byte a in hash)
                    {
                        if (a < 16)
                            sb.AppendFormat("0{0}", a.ToString("x"));
                        else
                            sb.Append(a.ToString("x"));
                    }
                }

                reader.Close();
                hashString = sb.ToString();
            }

            return hashString;
        }

        public static int BytesToInt32(byte[] bytes, ByteOrder byteOrder)
        {
            if (byteOrder == ByteOrder.BigEndian)
                Array.Reverse(bytes);
            
            return BitConverter.ToInt32(bytes, 0);
        }

        public static bool UnGZip(string inFile, string outFile)
        {
            bool ret = false;
            FileStream inStream = null;
            FileStream outStream = null;
            GZipStream decompressStream = null;
            try
            {
                inStream = new FileStream(inFile, FileMode.Open);
                decompressStream = new GZipStream(inStream, CompressionMode.Decompress);
                outStream = new FileStream(outFile, FileMode.OpenOrCreate);
                
                byte[] buffer = new byte[4096];
                int accuRead = 0;
                while ((accuRead = decompressStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    outStream.Write(buffer, 0, accuRead);
                }
                ret = true;
            }
            catch (Exception ex)
            {
                LogMan.Instance.Log(ex.Message);
            }
            finally
            {
                if (decompressStream != null)
                {
                    decompressStream.Close();
                }
                else if (inStream != null)
                {
                    inStream.Close();
                }

                if (outStream != null)
                    outStream.Close();
            }

            return ret;
        }


        //For ConvertChsToCht
        private const int LOCALE_SYSTEM_DEFAULT = 0x0800;
        private const int LOCALE_TAIWAN = 1028;
        private const int LCMAP_SIMPLIFIED_CHINESE = 0x02000000;
        private const int LCMAP_TRADITIONAL_CHINESE = 0x04000000;

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int LCMapString(int Locale, int dwMapFlags, string lpSrcStr, int cchSrc
            , [Out] string lpDestStr, int cchDest);

        public enum ConversionResult
        {
            OK,
            NoConversion,
            Error
        }

        public static ConversionResult ConvertChsToCht(string inFile, string outFile)
        {
            ConversionResult ret = ConversionResult.Error;
            StreamReader reader = null;
            FileStream outStream = null;
            StreamWriter writer = null;

            try
            {
                Encoding inEncoding = DetectEncoding(inFile);
                if (inEncoding != null && inEncoding.CodePage == 936)  //If the encoding is GB2312
                {
                    reader = new StreamReader(inFile, inEncoding);
                    outStream = new FileStream(outFile, FileMode.OpenOrCreate);
                    Encoding outEncoding = Encoding.GetEncoding("Big5");
                    writer = new StreamWriter(outStream, outEncoding);
                    string line = null;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string chtLine = new String(' ', line.Length);
                        LCMapString(LOCALE_TAIWAN, LCMAP_TRADITIONAL_CHINESE
                            , line, line.Length, chtLine, chtLine.Length);
                        writer.WriteLine(chtLine);
                    }


                    ret = ConversionResult.OK;

                }
                else
                {
                    ret = ConversionResult.NoConversion;
                }
                
            }
            catch (Exception ex)
            {
                LogMan.Instance.Log(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (writer != null)
                {
                    writer.Close();
                }
                else if (outStream != null)
                {
                    outStream.Close();
                }

            }

            return ret;
        }

        //For DetectEncoding
        private static volatile bool encodingFound;
        private static volatile string encodingName = String.Empty;
        private class Notifier : nsICharsetDetectionObserver
        {
            public void Notify(String charset)
            {
                Util.encodingFound = true;
                Util.encodingName = charset;
            }
        }

        public static Encoding DetectEncoding(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Encoding encoding = null;


            nsDetector det = new nsDetector();
            Notifier not = new Notifier();
            det.Init(not);

            bool done = false;
            bool isAscii = true;

            byte[] buf = new byte[1024];
            int len = fs.Read(buf, 0, buf.Length);

            while (len > 0)
            {
                // Check if the stream is only ascii.
                if (isAscii)
                    isAscii = det.isAscii(buf, len);

                // DoIt if non-ascii and not done yet.
                if (!isAscii && !done)
                    done = det.DoIt(buf, len, false);

                len = fs.Read(buf, 0, buf.Length);
            }
            fs.Close();
            det.DataEnd();

            if (isAscii)
            {
                encodingFound = true;
                encoding = Encoding.ASCII;
            }

            if (!encodingFound)
            {
                String[] prob = det.getProbableCharsets();
                encodingName = prob[0];
            }

            if (encoding == null)
            {
                encoding = Encoding.GetEncoding(encodingName);
            }

            return encoding;
        }

        // P/Invoke declarations
        private delegate int DllRegisterServer();
        private delegate int DllUnregisterServer();
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadLibraryEx(string path, IntPtr dummy, int flags);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hdl);
        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hdl, string name);

        public static bool RegisterDll(string dllPath)
        {
            // Register COM component, false if not a COM component
            IntPtr module = LoadLibraryEx(dllPath, IntPtr.Zero, 0);
            if (module == IntPtr.Zero)
            {
                LogMan.Instance.Log(Properties.Resources.ErrDllReg, Marshal.GetLastWin32Error());
                return false;
            }

            try
            {
                IntPtr addr = GetProcAddress(module, "DllRegisterServer");
                if (addr == IntPtr.Zero)
                {
                    LogMan.Instance.Log(Properties.Resources.ErrDllReg, Marshal.GetLastWin32Error());
                    return false;
                }
                DllRegisterServer dlg = (DllRegisterServer)Marshal.GetDelegateForFunctionPointer(addr, typeof(DllRegisterServer));
                int hr = dlg.Invoke();
                if (hr != 0)
                {
                    Exception e = Marshal.GetExceptionForHR(hr);
                    LogMan.Instance.Log(e.Message);
                    return false;
                }
                return true;
            }
            finally
            {
                FreeLibrary(module);
            }
        }

        public static bool UnregisterDll(string dllPath)
        {
            // Register COM component, false if not a COM component
            IntPtr module = LoadLibraryEx(dllPath, IntPtr.Zero, 0);
            if (module == IntPtr.Zero)
            {
                LogMan.Instance.Log(Properties.Resources.ErrDllUnreg, Marshal.GetLastWin32Error());
                return false;
            }
            try
            {
                IntPtr addr = GetProcAddress(module, "DllUnregisterServer");
                if (addr == IntPtr.Zero)
                {
                    LogMan.Instance.Log(Properties.Resources.ErrDllUnreg, Marshal.GetLastWin32Error());
                    return false;
                }
                DllUnregisterServer dlg = (DllUnregisterServer)Marshal.GetDelegateForFunctionPointer(addr, typeof(DllUnregisterServer));
                int hr = dlg.Invoke();
                if (hr != 0)
                {
                    Exception e = Marshal.GetExceptionForHR(hr);
                    LogMan.Instance.Log(e.Message);
                    return false;
                }
                return true;
            }
            finally
            {
                FreeLibrary(module);
            }
        }
    }

    //For BytesToInt32
    public enum ByteOrder
    {
        LittleEndian,
        BigEndian
    }
}
