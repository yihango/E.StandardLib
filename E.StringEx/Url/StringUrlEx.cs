using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace E.StringEx.Url
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringUrlEx
    {
        #region Public Method

        /// <summary>
        /// 对字符串进行Url编码(UTF-8)
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string UrlEncode(this string str)
        {
            if (str == null)
                return null;
            return UrlEncode(str, Encoding.UTF8);
        }

        /// <summary>
        /// 对字符串进行Url编码(指定编码类型)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string UrlEncode(this string str, Encoding e)
        {
            if (str == null)
                return null;
            return Encoding.ASCII.GetString(UrlEncodeToBytes(str, e));
        }

        /// <summary>
        /// 对字符串进行URL解码(UTF-8)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UrlDecode(this string value)
        {
            return UrlDecode(value, Encoding.UTF8);
        }

        /// <summary>
        /// 对字符串进行URL解码(指定编码类型)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string UrlDecode(this string value, Encoding encoding)
        {
            if (value == null)
                return null;


            int count = value.Length;
            UrlDecoder helper = new UrlDecoder(count, encoding);

            // go through the string's chars collapsing %XX and %uXXXX and
            // appending each char as char, with exception of %XX constructs
            // that are appended as bytes

            for (int pos = 0; pos < count; pos++)
            {
                char ch = value[pos];

                if (ch == '+')
                {
                    ch = ' ';
                }
                else if (ch == '%' && pos < count - 2)
                {
                    if (value[pos + 1] == 'u' && pos < count - 5)
                    {
                        int h1 = HexToInt(value[pos + 2]);
                        int h2 = HexToInt(value[pos + 3]);
                        int h3 = HexToInt(value[pos + 4]);
                        int h4 = HexToInt(value[pos + 5]);

                        if (h1 >= 0 && h2 >= 0 && h3 >= 0 && h4 >= 0)
                        {   // valid 4 hex chars
                            ch = (char)((h1 << 12) | (h2 << 8) | (h3 << 4) | h4);
                            pos += 5;

                            // only add as char
                            helper.AddChar(ch);
                            continue;
                        }
                    }
                    else
                    {
                        int h1 = HexToInt(value[pos + 1]);
                        int h2 = HexToInt(value[pos + 2]);

                        if (h1 >= 0 && h2 >= 0)
                        {     // valid 2 hex chars
                            byte b = (byte)((h1 << 4) | h2);
                            pos += 2;

                            // don't add as char
                            helper.AddByte(b);
                            continue;
                        }
                    }
                }

                if ((ch & 0xFF80) == 0)
                    helper.AddByte((byte)ch); // 7 bit have to go as bytes because of Unicode
                else
                    helper.AddChar(ch);
            }

            return Utf16StringValidator.ValidateString(helper.GetString());
        }

        /// <summary>
        /// 对字符串中的空格进行URL编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlEncodeSpaces(this string str)
        {
            if (str != null && str.IndexOf(' ') >= 0)
                str = str.Replace(" ", "%20");
            return str;
        }

        /// <summary>
        /// 对字符串中的空格进行URL解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlDecodeSpaces(this string str)
        {
            if (str != null && str.IndexOf(' ') >= 0)
                str = str.Replace("%20", " ");
            return str;
        }


        #endregion




        #region Public Tools Method

        /// <summary>
        /// 判断字符是否需要进行URL编码
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static bool IsUrlSafeChar(this char ch)
        {
            if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9'))
                return true;

            switch (ch)
            {
                case '-':
                case '_':
                case '.':
                case '!':
                case '*':
                case '(':
                case ')':
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 10进制数字转16进制字符
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static char IntToHex(this int n)
        {
            Debug.Assert(n < 0x10);

            if (n <= 9)
                return (char)(n + (int)'0');
            else
                return (char)(n - 10 + (int)'a');
        }

        /// <summary>
        /// 16进制字符转10进制数字
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        public static int HexToInt(this char h)
        {
            return (h >= '0' && h <= '9') ? h - '0' :
            (h >= 'a' && h <= 'f') ? h - 'a' + 10 :
            (h >= 'A' && h <= 'F') ? h - 'A' + 10 :
            -1;
        }


        #endregion






        #region Private Method

        /// <summary>
        /// 字符串转字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private static byte[] UrlEncodeToBytes(string str, Encoding e)
        {
            if (str == null)
                return null;
            byte[] bytes = e.GetBytes(str);
            return UrlEncode(bytes, 0, bytes.Length, false /* alwaysCreateNewReturnValue */);
        }

        /// <summary>
        /// 字符串转字节数组
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="alwaysCreateNewReturnValue"></param>
        /// <returns></returns>
        private static byte[] UrlEncode(byte[] bytes, int offset, int count, bool alwaysCreateNewReturnValue)
        {
            byte[] encoded = UrlEncode(bytes, offset, count);

            return (alwaysCreateNewReturnValue && (encoded != null) && (encoded == bytes))
                ? (byte[])encoded.Clone()
                : encoded;
        }

        /// <summary>
        /// 编码处理
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static byte[] UrlEncode(byte[] bytes, int offset, int count)
        {
            if (!ValidateUrlEncodingParameters(bytes, offset, count))
            {
                return null;
            }

            int cSpaces = 0;
            int cUnsafe = 0;

            // count them first
            for (int i = 0; i < count; i++)
            {
                char ch = (char)bytes[offset + i];

                if (ch == ' ')
                    cSpaces++;
                else if (!IsUrlSafeChar(ch))
                    cUnsafe++;
            }

            // nothing to expand?
            if (cSpaces == 0 && cUnsafe == 0)
            {
                // DevDiv 912606: respect "offset" and "count"
                if (0 == offset && bytes.Length == count)
                {
                    return bytes;
                }
                else
                {
                    var subarray = new byte[count];
                    Buffer.BlockCopy(bytes, offset, subarray, 0, count);
                    return subarray;
                }
            }

            // expand not 'safe' characters into %XX, spaces to +s
            byte[] expandedBytes = new byte[count + cUnsafe * 2];
            int pos = 0;

            for (int i = 0; i < count; i++)
            {
                byte b = bytes[offset + i];
                char ch = (char)b;

                if (IsUrlSafeChar(ch))
                {
                    expandedBytes[pos++] = b;
                }
                else if (ch == ' ')
                {
                    expandedBytes[pos++] = (byte)'+';
                }
                else
                {
                    expandedBytes[pos++] = (byte)'%';
                    expandedBytes[pos++] = (byte)IntToHex((b >> 4) & 0xf);
                    expandedBytes[pos++] = (byte)IntToHex(b & 0x0f);
                }
            }

            return expandedBytes;
        }

        /// <summary>
        /// 验证参数
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static bool ValidateUrlEncodingParameters(byte[] bytes, int offset, int count)
        {
            if (bytes == null && count == 0)
                return false;
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            if (offset < 0 || offset > bytes.Length)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (count < 0 || offset + count > bytes.Length)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            return true;
        }
        #endregion


    }
}
