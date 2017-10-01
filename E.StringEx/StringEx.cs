using System;
using System.Text;

namespace E.StringEx
{
    /// <summary>
    /// 字符串扩展函数
    /// </summary>
    public static class StringEx
    {
        /// <summary>
        /// 判断字符串是否为null/空字符串/空格
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>为空返回true,不为空false</returns>
        public static bool IsNull(this string str)
        {
            return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 字符串转换为int
        /// (转换失败将抛出类型转换错误的异常)
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的结果</returns>
        public static int ToInt32(this string str)
        {
            return int.Parse(str);
        }

        /// <summary>
        /// 字符串转换为int
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="res">转换后的结果</param>
        /// <returns>返回true表示转换成功,false表示转换失败</returns>
        public static bool ToInt32(this string str,out int res)
        {
            return int.TryParse(str, out res);
        }

        /// <summary>
        /// 字符串转HexNumber
        /// (转换失败将抛出类型转换错误的异常)
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns></returns>
        public static int ToHexInt32(this string str)
        {
            return Int32.Parse(str, System.Globalization.NumberStyles.HexNumber);
        }


        /// <summary>
        /// 字符串转换为double
        /// (转换失败将抛出类型转换错误的异常)
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的结果</returns>
        public static double ToDouble(this string str)
        {
            return double.Parse(str);
        }

        /// <summary>
        /// 字符串转换为double
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="res">转换后的结果</param>
        /// <returns>返回true表示转换成功,false表示转换失败</returns>
        public static bool ToDouble(this string str,out double res)
        {
            return double.TryParse(str, out res);
        }

        /// <summary>
        /// 字符串转换为DateTime
        /// (转换失败将抛出类型转换错误的异常)
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的结果</returns>
        public static DateTime ToDateTime(this string str)
        {
            return DateTime.Parse(str);
        }

        /// <summary>
        /// 字符串转换为DateTime
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="res">转换后的结果</param>
        /// <returns>返回true表示转换成功,false表示转换失败</returns>
        public static bool ToDateTime(this string str,out DateTime res)
        {
            return DateTime.TryParse(str, out res);
        }


        /// <summary>
        /// 将字符串转换成字符数组
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>字符数组</returns>
        public static char[] ToArray(this string str)
        {
            return str.ToCharArray();
        }


        /// <summary>
        ///  将字符串中的指定子字符串转换成字符数组
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>从startIndex到Length之间的字符数组</returns>
        public static char[] ToArray(this string str,int startIndex,int length)
        {
            return str.ToCharArray(startIndex,length);
        }


        /// <summary>
        /// 16进制字符串转字节字符串
        /// </summary>
        /// <param name="hexStr">要转换的字符串</param>
        /// <returns>转换后的结果,传入的字符串若为空则返回空</returns>
        public static string HexToBitStr(this string hexStr)
        {
            if(IsNull(hexStr))
                return string.Empty;


            string[] strs =
            {"0000",   "0001",   "0010",   "0011",
             "0100",   "0101",   "0110",   "0111",
             "1000",   "1001",   "1010",   "1011",
             "1100",   "1101",   "1110",   "1111"
            };

            var Result = new StringBuilder();

            for (int i = 0; i < hexStr.Length; i++)
            {
                Result.Append(strs[Convert.ToByte(hexStr[i])]);
            }

            var res = Result.ToString();

            while (res.IndexOf('0') == 1)
            {
                res = res.Remove(0, 1);
            }

            return res;
        }


        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>转换后的结果</returns>
        public static string ByteToHexStr(this byte[] bytes)
        {
            string returnStr = string.Empty;
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
    }
}
