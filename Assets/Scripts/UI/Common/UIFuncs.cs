using System;
using UI;

public class UIFuncs
{
    public static string SetText(string msg, object[] args)
    {
        try
        {
            if (args != null)
            {
                int ilength = args.Length;
                for (int i = 0; i < ilength; i++)
                {
                    if (args[i] != null)
                        msg = msg.Replace("{" + i + "}", args[i].ToString());
                }
            }
        }
        catch (Exception ex)
        {
            ILog.Error("check mulitylanguage  csv data error   languageid! detail: " + ex);
        }
        return msg;
    }

    public static void ShowMessageTip(string text)
    {

    }
}