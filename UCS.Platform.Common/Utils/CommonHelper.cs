using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace UCS.Platform.Common.Utils
{
    public class CommonHelper
    {
        public static byte Char2Byte(char c)
        {
            switch (c)
            {
                default:
                    return 0;
                case '0':
                    return 0;

                case '1':
                    return 1;

                case '2':
                    return 2;

                case '3':
                    return 3;

                case '4':
                    return 4;

                case '5':
                    return 5;

                case '6':
                    return 6;

                case '7':
                    return 7;

                case '8':
                    return 8;

                case '9':
                    return 9;

                case 'A':
                    return 10;

                case 'B':
                    return 11;

                case 'C':
                    return 12;

                case 'D':
                    return 13;

                case 'E':
                    return 14;

                case 'F':
                    return 15;
            }
        }

        public static string Bytes2Hex(byte[] bytes)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                //stringBuilder.AppendFormat("{0:x2}", b);
                stringBuilder.Append(b.ToString("X2"));
            }
            return stringBuilder.ToString();
        }

        public static byte[] Hex2Bytes1(string hexString)
        {
            hexString = hexString.ToUpper();
            char[] array = hexString.ToCharArray();
            byte[] array2 = new byte[array.Length / 2];
            int num = 0;
            for (int i = 0; i < array.Length; i += 2)
            {
                byte b = 0;
                b |= Char2Byte(array[i]);
                b = (byte)(b << 4);
                b |= Char2Byte(array[i + 1]);
                array2[num] = b;
                num++;
            }
            return array2;
        }
        public static byte[] Hex2Bytes(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        public static byte[] Hash(string text)
        {
            SHA1 sha1 = SHA1.Create();
            return sha1.ComputeHash(Encoding.UTF8.GetBytes(text));
        }

        /// <summary>
        /// 生成混合随机数
        /// </summary>
        /// <param length="length">随机字符串长度</param>
        /// <returns></returns>
        public static string GetMixRandomNum(int length)
        {
            string a = "0123456789";
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                sb.Append(a[new Random(Guid.NewGuid().GetHashCode()).Next(0, a.Length - 1)]);
            }

            return sb.ToString();
        }
    }
}
