using System;
using System.Configuration;

public static class ConnectionHelper
{
    public static string ConnectionString
    {
        get
        {
            return ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;
        }
    }
} 