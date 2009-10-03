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
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Resources;
using System.Reflection;
using System.IO.Compression;
using System.Runtime.InteropServices;
using ShooterDownloader.Properties;
using href.Utils;

namespace ShooterDownloader
{
    enum ByteOrder
    {
        LittleEndian,
        BigEndian
    }

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
                reader = EncodingTools.OpenTextFile(inFile);
                if (reader.CurrentEncoding == Encoding.GetEncoding("GB2312"))
                {
                    outStream = new FileStream(outFile, FileMode.OpenOrCreate);
                    Encoding enc = Encoding.GetEncoding("Big5");
                    writer = new StreamWriter(outStream, enc);
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
    }
}
