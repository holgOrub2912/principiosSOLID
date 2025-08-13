using System.Reflection.Metadata;

namespace Isas_Pizza
{
    public static class DefaultParameters
    {
        public const string dbServer = "localhost";
        public const string dbName = "pizzeria";
        public const string dbUser = "postgres";
        public static string dbPassword = Environment.GetEnvironmentVariable("PIZZERIA_DBPASSWORD") ?? "";
        public const string authfile = "userdb.txt";
    }
}