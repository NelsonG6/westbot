using System;
using WestBot;

namespace Westbot
{
    //Move this config functionality to the sql database
    public static class BotConfiguration
    {
        public static char Prefix { get; set; } = '\0';
        public static string Token { get; set; } = "";
        public static ulong ServerID { get; set; } = 0;

        public static void Load(string[] config_name)
        {
            String ConfigurationName;

            if (config_name.Length == 0)
                ConfigurationName = "";
            else
                ConfigurationName = config_name[0];

            if (ConfigurationName == "")
            {
                ConfigurationName = "Testbot";
                DatabaseHandler.BumpPatch();
            }
            
            // load config from database

            try
            {
                DatabaseHandler.GetConfiguration(ConfigurationName);
                Console.WriteLine("Configuration Loaded.");
                Console.WriteLine("Config name: " + ConfigurationName);
                Console.WriteLine("This programs path: " + AppContext.BaseDirectory);
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return;
            }
            //no match was found
            Console.WriteLine("No match was found.");
        }    
    }
}