using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

/// <summary>
/// 最佳性能的字符串拼接
/// 建议所有的 "A"+"B",string.format() 都替换成这种方式
/// </summary>
public static class MTString {

    //只会存在一个静态变量，可重复使用
    private static StringBuilder m_stringBuilder = new StringBuilder();
    /// <summary>
    /// 性能最优的字符串拼接方法
    /// </summary>
    /// <param name="str1"></param>
    /// <param name="str2"></param>
    /// <returns></returns>
    public static string GetString(string str1, string str2)
    {
        lock(m_stringBuilder)
        {
            m_stringBuilder.Length = 0;
            var length = str1.Length + str2.Length;
            if (length > m_stringBuilder.Capacity)
                m_stringBuilder.Capacity = length;

            m_stringBuilder.Append(str1);
            m_stringBuilder.Append(str2);

            return m_stringBuilder.ToString();
        }

    }

    /// <summary>
    /// 性能最优的字符串拼接方法
    /// </summary>
    /// <param name="str1"></param>
    /// <param name="str2"></param>
    /// <param name="str3"></param>
    /// <returns></returns>
    public static string GetString(string str1, string str2,string str3)
    {
        lock (m_stringBuilder)
        {
            m_stringBuilder.Length = 0;
            var length = str1.Length + str2.Length + str3.Length;
            if (length > m_stringBuilder.Capacity)
                m_stringBuilder.Capacity = length;

            m_stringBuilder.Append(str1);
            m_stringBuilder.Append(str2);
            m_stringBuilder.Append(str3);

            return m_stringBuilder.ToString();
        }
    }

    /// <summary>
    /// 性能最优的字符串拼接方法
    /// </summary>
    /// <param name="str1"></param>
    /// <param name="str2"></param>
    /// <param name="str3"></param>
    /// <param name="str4"></param>
    /// <returns></returns>
    public static string GetString(string str1, string str2, string str3, string str4)
    {
        lock (m_stringBuilder)
        {
            m_stringBuilder.Length = 0;
            var length = str1.Length + str2.Length + str3.Length + str4.Length;
            if (length > m_stringBuilder.Capacity)
                m_stringBuilder.Capacity = length;

            m_stringBuilder.Append(str1);
            m_stringBuilder.Append(str2);
            m_stringBuilder.Append(str3);
            m_stringBuilder.Append(str4);

            return m_stringBuilder.ToString();
        }
    }
    /// <summary>
    /// 性能最优的字符串拼接方法
    /// </summary>
    /// <param name="str1"></param>
    /// <param name="str2"></param>
    /// <param name="str3"></param>
    /// <param name="str4"></param>
    /// <param name="str5"></param>
    /// <returns></returns>
    public static string GetString(string str1, string str2, string str3, string str4, string str5)
    {
        lock (m_stringBuilder)
        {
            m_stringBuilder.Length = 0;
            var length = str1.Length + str2.Length + str3.Length + str4.Length + str5.Length;
            if (length > m_stringBuilder.Capacity)
                m_stringBuilder.Capacity = length;

            m_stringBuilder.Append(str1);
            m_stringBuilder.Append(str2);
            m_stringBuilder.Append(str3);
            m_stringBuilder.Append(str4);
            m_stringBuilder.Append(str5);

            return m_stringBuilder.ToString();
        }
    }
    /// <summary>
    /// 性能最优的字符串拼接方法
    /// </summary>
    /// <param name="str1"></param>
    /// <param name="str2"></param>
    /// <param name="str3"></param>
    /// <param name="str4"></param>
    /// <param name="str5"></param>
    /// <param name="str6"></param>
    /// <returns></returns>
    public static string GetString(string str1, string str2, string str3, string str4, string str5, string str6)
    {
        lock (m_stringBuilder)
        {
            m_stringBuilder.Length = 0;
            var length = str1.Length + str2.Length + str3.Length + str4.Length + str5.Length
                + str6.Length;
            if (length > m_stringBuilder.Capacity)
                m_stringBuilder.Capacity = length;

            m_stringBuilder.Append(str1);
            m_stringBuilder.Append(str2);
            m_stringBuilder.Append(str3);
            m_stringBuilder.Append(str4);
            m_stringBuilder.Append(str5);
            m_stringBuilder.Append(str6);

            return m_stringBuilder.ToString();
        }
    }
    /// <summary>
    /// 性能最优的字符串拼接方法
    /// </summary>
    /// <param name="str1"></param>
    /// <param name="str2"></param>
    /// <param name="str3"></param>
    /// <param name="str4"></param>
    /// <param name="str5"></param>
    /// <param name="str6"></param>
    /// <param name="str7"></param>
    /// <returns></returns>
    public static string GetString(string str1, string str2, string str3, string str4, string str5, string str6, string str7)
    {
        lock (m_stringBuilder)
        {
            m_stringBuilder.Length = 0;
            var length = str1.Length + str2.Length + str3.Length + str4.Length + str5.Length
                + str6.Length + str7.Length;
            if (length > m_stringBuilder.Capacity)
                m_stringBuilder.Capacity = length;

            m_stringBuilder.Append(str1);
            m_stringBuilder.Append(str2);
            m_stringBuilder.Append(str3);
            m_stringBuilder.Append(str4);
            m_stringBuilder.Append(str5);
            m_stringBuilder.Append(str6);
            m_stringBuilder.Append(str7);

            return m_stringBuilder.ToString();
        }
    }
    /// <summary>
    /// 性能最优的字符串拼接方法
    /// </summary>
    /// <param name="str1"></param>
    /// <param name="str2"></param>
    /// <param name="str3"></param>
    /// <param name="str4"></param>
    /// <param name="str5"></param>
    /// <param name="str6"></param>
    /// <param name="str7"></param>
    /// <param name="str8"></param>
    /// <returns></returns>
    public static string GetString(string str1, string str2, string str3, string str4, string str5, string str6, string str7, string str8)
    {
        lock (m_stringBuilder)
        {
            m_stringBuilder.Length = 0;
            var length = str1.Length + str2.Length + str3.Length + str4.Length + str5.Length
                + str6.Length + str7.Length + str8.Length;
            if (length > m_stringBuilder.Capacity)
                m_stringBuilder.Capacity = length;

            m_stringBuilder.Append(str1);
            m_stringBuilder.Append(str2);
            m_stringBuilder.Append(str3);
            m_stringBuilder.Append(str4);
            m_stringBuilder.Append(str5);
            m_stringBuilder.Append(str6);
            m_stringBuilder.Append(str7);
            m_stringBuilder.Append(str8);

            return m_stringBuilder.ToString();
        }
    }
    /// <summary>
    /// 性能最优的字符串拼接方法
    /// </summary>
    /// <param name="str1"></param>
    /// <param name="str2"></param>
    /// <param name="str3"></param>
    /// <param name="str4"></param>
    /// <param name="str5"></param>
    /// <param name="str6"></param>
    /// <param name="str7"></param>
    /// <param name="str8"></param>
    /// <param name="str9"></param>
    /// <returns></returns>
    public static string GetString(string str1, string str2, string str3, string str4, string str5, string str6, string str7, string str8, string str9)
    {
        lock (m_stringBuilder)
        {
            m_stringBuilder.Length = 0;
            var length = str1.Length + str2.Length + str3.Length + str4.Length + str5.Length
                + str6.Length + str7.Length + str8.Length + str9.Length;
            if (length > m_stringBuilder.Capacity)
                m_stringBuilder.Capacity = length;

            m_stringBuilder.Append(str1);
            m_stringBuilder.Append(str2);
            m_stringBuilder.Append(str3);
            m_stringBuilder.Append(str4);
            m_stringBuilder.Append(str5);
            m_stringBuilder.Append(str6);
            m_stringBuilder.Append(str7);
            m_stringBuilder.Append(str8);
            m_stringBuilder.Append(str9);

            return m_stringBuilder.ToString();
        }
    }
    /// <summary>
    /// 性能最优的字符串拼接方法
    /// </summary>
    /// <param name="str1"></param>
    /// <param name="str2"></param>
    /// <param name="str3"></param>
    /// <param name="str4"></param>
    /// <param name="str5"></param>
    /// <param name="str6"></param>
    /// <param name="str7"></param>
    /// <param name="str8"></param>
    /// <param name="str9"></param>
    /// <param name="str10"></param>
    /// <returns></returns>
    public static string GetString(string str1, string str2, string str3, string str4, string str5, string str6, string str7, string str8, string str9, string str10)
    {
        lock (m_stringBuilder)
        {
            m_stringBuilder.Length = 0;
            var length = str1.Length + str2.Length + str3.Length + str4.Length + str5.Length
                + str6.Length + str7.Length + str8.Length + str9.Length + str10.Length;
            if (length > m_stringBuilder.Capacity)
                m_stringBuilder.Capacity = length;

            m_stringBuilder.Append(str1);
            m_stringBuilder.Append(str2);
            m_stringBuilder.Append(str3);
            m_stringBuilder.Append(str4);
            m_stringBuilder.Append(str5);
            m_stringBuilder.Append(str6);
            m_stringBuilder.Append(str7);
            m_stringBuilder.Append(str8);
            m_stringBuilder.Append(str9);
            m_stringBuilder.Append(str10);

            return m_stringBuilder.ToString();
        }
    }


    /// <summary>
    /// 慎用！！！！
    /// 有太多字符串的情况下(超过十个以上)，才勉强可以使用
    /// </summary>
    /// <param name="strList"></param>
    /// <returns></returns>
    public static string GetReprotString(params string[] strList)
    {
        lock (m_stringBuilder)
        {
            m_stringBuilder.Length = 0;

            for (int i = 0; i < strList.Length; i++)
            {
                var length = m_stringBuilder.Capacity + strList[i].Length;
                if (length > m_stringBuilder.Capacity)
                    m_stringBuilder.Capacity = length;

                m_stringBuilder.Append(strList[i]);
            }

            return m_stringBuilder.ToString();
        }
    }

}
