namespace WestBot
{
    static class MySQLConnString
    {
        public static string Get()
        {
            string string1 = "Data Source=westbot.westus.cloudapp.azure.com;";
            string string2 = "Initial Catalog=Westbot;";
            string string3 = "User ID=bot;";
            string string4 = "password=westbot";
            string connString = string1 + string2 + string3 + string4;
            return connString;
        }
    }
}
