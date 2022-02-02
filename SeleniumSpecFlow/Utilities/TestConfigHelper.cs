using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeleniumSpecFlow.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestLibrary.Utilities
{
    public class TestConfigHelper
    {
        public static BrowserType browser { get; private set; }
        private static readonly string workingDirectory = Environment.CurrentDirectory;
        private static readonly string projectPath = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
        private static string jsonString;

        public static IConfigurationRoot GetIConfigurationBase()
        {
            return new ConfigurationBuilder()
            .AddJsonFile(projectPath + "\\appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        }

        public static EnvironmentConfigSettings GetApplicationConfiguration()
        {
            LaunchSettingsFixture();
            var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");
            browser = (BrowserType)Enum.Parse(typeof(BrowserType), Environment.GetEnvironmentVariable("BROWSER")); ;
            var systemConfiguration = new SystemConfiguration();
            var iTestConfigurationRoot = GetIConfigurationBase();
            iTestConfigurationRoot.GetSection("SystemConfiguration").Bind(systemConfiguration);
            if (environment != null)
            {
                if (environment.ToLower() == "Development".ToLower())
                {
                    return systemConfiguration.DevelopmentEnvironmentConfigSettings;
                }
                else if (environment.ToLower() == "QA".ToLower())
                {
                    return systemConfiguration.StagingEnvironmentConfigSettings;
                }
                else if (environment.ToLower() == "Remote".ToLower())
                {
                    return systemConfiguration.RemoteEnvironmentConfigSettings;
                }


            }
            return null;
        }

        public static void LaunchSettingsFixture()
        {
            var cs = projectPath + "\\Properties\\launchSettings.json";
            using var file = File.OpenText(projectPath + "\\Properties\\launchSettings.json");
            var reader = new JsonTextReader(file);
            var jObject = JObject.Load(reader);

            var variables = jObject
                .GetValue("profiles")
                //select a proper profile here
                .SelectMany(profiles => profiles.Children())
                .SelectMany(profile => profile.Children<JProperty>())
                .Where(prop => prop.Name == "environmentVariables")
                .SelectMany(prop => prop.Value.Children<JProperty>())
                .ToList();

            foreach (var variable in variables)
            {
                Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
            }
        }
    }
}
