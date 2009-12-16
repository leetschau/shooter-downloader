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

using org.mozilla.intl.chardet;
using System.Diagnostics;
using System.Windows.Forms;


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

        public static int GetGetBoundedValue(int intendValue, int lowerBound, int upperBound)
        {
            int trueValue;
            trueValue = Math.Min(intendValue, upperBound);
            trueValue = Math.Max(lowerBound, intendValue);
            return trueValue;
        }
        

        public enum ConversionResult
        {
            OK,
            NoConversion,
            Error
        }


        #region OS Dependent Code
        public static ConversionResult ConvertChsToCht(string inFile, string outFile)
        {
            return ConvertChsToCht(inFile, outFile, true);
        }

        public static ConversionResult ConvertChsToCht(string inFile, string outFile, bool detectEncoding)
        {
            return OsUtil.ConvertChsToCht(inFile, outFile, detectEncoding);
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


            nsDetector det = new nsDetector(2);
            Notifier not = new Notifier();
            det.Init(not);

            bool done = false;
            bool isAscii = true;

            byte[] buf = new byte[1024];
            int len = fs.Read(buf, 0, buf.Length);

            //For some reason NCharDet can't detect Unicode.
            //Manual detect Unicode here.
            if (len >= 2 && buf[0] == 0xFF && buf[1] == 0xFE)
            {
                fs.Close();
                det.DataEnd();
                return Encoding.Unicode;
            }

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

       
        public static void RunProc(string filePath, string args, bool needElevation)
        {
            OsUtil.RunProc(filePath, args, needElevation);
        }

        public static bool RegisterDll(string dllPath)
        {
            return OsUtil.RegisterDll(dllPath);
        }

        public static bool UnregisterDll(string dllPath)
        {
            return OsUtil.UnregisterDll(dllPath);
        }

        public static bool IsAdmin
        {
            get
            {
                return OsUtil.IsAdmin;
            }
        }


        //Add a shield ICON to the button to inform user privilege elevation is required.
        // Only work on Vista or above.
        public static void AddShieldToButton(Button b)
        {
            OsUtil.AddShieldToButton(b);
        }

        public static bool Is64BitOS
        {
            get
            {
                return OsUtil.Is64BitOS;
            }
        }

        private static ShooterDownloader.OsDependent.UtilAny _osUtil = null;

        public static bool IsWindows
        {
            get
            {
                bool result = false;
                OperatingSystem os = Environment.OSVersion;
                PlatformID pid = os.Platform;
                switch (pid)
                {
                    case PlatformID.Win32NT:
                    case PlatformID.Win32S:
                    case PlatformID.Win32Windows:
                    case PlatformID.WinCE:
                        result = true;
                        break;
                }

                return result;
            }
        }

        private static ShooterDownloader.OsDependent.UtilAny OsUtil
        {
            get
            {
                if (_osUtil == null)
                {
                    lock (_osUtil)
                    {
                        if (_osUtil == null)
                        {
                            if (IsWindows)
                                _osUtil = new ShooterDownloader.OsDependent.UtilWin();
                            else
                                _osUtil = new ShooterDownloader.OsDependent.UtilAny();
                        }
                    }
                }

                return _osUtil;
            }
        }
        #endregion
    }

    //For BytesToInt32
    public enum ByteOrder
    {
        LittleEndian,
        BigEndian
    }
}
