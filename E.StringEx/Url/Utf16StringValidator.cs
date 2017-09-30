using System;
using System.Collections.Generic;
using System.Text;

namespace E.StringEx.Url
{
    /// <summary>
    /// UTF-16字符验证类
    /// </summary>
    public static class Utf16StringValidator
    {

        private const char UNICODE_NULL_CHAR = '\0';
        private const char UNICODE_REPLACEMENT_CHAR = '\uFFFD';


        /// <summary>
        /// false [default] - disallow invalid UTF-16 (like unpaired surrogates) when deserializing JSON strings and inside UrlDecode
        /// true - allow malformed strings like "\ud800" and "%ud800" to be deserialized (dangerous!)
        /// </summary>
        public static bool SkipUtf16Validation { get; set; }

        /// <summary>
        /// 验证字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ValidateString(string input)
        {
            return ValidateString(input, SkipUtf16Validation);
        }

        // only internal for unit testing
        private static string ValidateString(string input, bool skipUtf16Validation)
        {
            if (skipUtf16Validation || string.IsNullOrEmpty(input))
            {
                return input;
            }

            // locate the first surrogate character
            int idxOfFirstSurrogate = -1;
            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsSurrogate(input[i]))
                {
                    idxOfFirstSurrogate = i;
                    break;
                }
            }

            // fast case: no surrogates = return input string
            if (idxOfFirstSurrogate < 0)
            {
                return input;
            }

            // slow case: surrogates exist, so we need to validate them
            char[] chars = input.ToCharArray();
            for (int i = idxOfFirstSurrogate; i < chars.Length; i++)
            {
                char thisChar = chars[i];

                // If this character is a low surrogate, then it was not preceded by
                // a high surrogate, so we'll replace it.
                if (Char.IsLowSurrogate(thisChar))
                {
                    chars[i] = UNICODE_REPLACEMENT_CHAR;
                    continue;
                }

                if (Char.IsHighSurrogate(thisChar))
                {
                    // If this character is a high surrogate and it is followed by a
                    // low surrogate, allow both to remain.
                    if (i + 1 < chars.Length && char.IsLowSurrogate(chars[i + 1]))
                    {
                        i++; // skip the low surrogate also
                        continue;
                    }

                    // If this character is a high surrogate and it is not followed
                    // by a low surrogate, replace it.
                    chars[i] = UNICODE_REPLACEMENT_CHAR;
                    continue;
                }

                // Otherwise, this is a non-surrogate character and just move to the
                // next character.
            }
            return new string(chars);
        }
    }
}
