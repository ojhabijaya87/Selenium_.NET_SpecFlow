﻿using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace SeleniumSpecFlow.Utilities
{
    class Config
    {
        public static bool RemoteBrowser => bool.Parse(GetValue("RemoteBrowser"));

        public static BrowserType Browser
            => (BrowserType)Enum.Parse(typeof(BrowserType), GetValue("Browser"));

        public static string Platform => GetValue("Platform");
        public static string WebUrl => GetValue("WebUrl");
        public static string ApiUrl => GetValue("ApiUrl");
        public static string Username => GetValue("Username");
        public static string Password => GetValue("Password");

        public static bool UseSeleniumGrid => bool.Parse(GetValue("UseSeleniumGrid"));
        public static string GridHubUri => GetValue("GridHubUrl");

        public static bool UseSauceLabs => bool.Parse(GetValue("UseSauceLabs"));
        public static string SauceLabsHubUri => GetValue("SauceLabsHubUrl");
        public static string SauceLabsUsername => GetValue("SauceLabsUsername");
        public static string SauceLabsAccessKey => GetValue("SauceLabsAccessKey");

        public static bool UseBrowserstack => bool.Parse(GetValue("BrowserStack"));
        public static string BrowserStackHubUrl => GetValue("BrowserStackHubUrl");
        public static string BrowserStackUsername => GetValue("BrowserStackUsername");
        public static string BrowserStackAccessKey => GetValue("BrowserStackAccessKey");

       /* private static string GetValue(string value)
        {
            var dirName = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
            var fileInfo = new FileInfo(dirName);
            var parentDirName = fileInfo?.FullName;

            var builder = new ConfigurationBuilder()
                .SetBasePath(parentDirName)
                .AddJsonFile("appsettings.json");
            return builder.Build()[value];
        }*/

        private static string GetValue(string value)

        {

          // Environment.SetEnvironmentVariable("ENVIRONMENT_VALUE", "qa", EnvironmentVariableTarget.Process);

            var dirName = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));

            var fileInfo = new FileInfo(dirName);

            var parentDirName = fileInfo?.FullName;



            var environment = Environment.GetEnvironmentVariable("ENVIRONMENT_VALUE" , EnvironmentVariableTarget.User);

            var environmentSpecificJsonFile = $"AppSettings.{environment}.json";





            var builder = new ConfigurationBuilder()



                .SetBasePath(parentDirName)

                .AddJsonFile("appsettings.json", true)

                .AddJsonFile(environmentSpecificJsonFile, true);

            return builder.Build()[value];

        }
    }
}
