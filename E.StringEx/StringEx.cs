using System;

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
    }
}
