using Microsoft.Extensions.Configuration;
using System.IO;

namespace BackEndPassManag.Service
{
    public static class AppSettingsJson
    {
        public static string ApplicationExeDirectory()
        {
            string location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string appRoot = Path.GetDirectoryName(location);
            return appRoot;
        }

        public static IConfigurationRoot GetAppSettings()
        {
            string applicationExeDirectory = ApplicationExeDirectory();
            IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(applicationExeDirectory)
            .AddJsonFile("appsettings.json");
            return builder.Build();
        }
    }
}
