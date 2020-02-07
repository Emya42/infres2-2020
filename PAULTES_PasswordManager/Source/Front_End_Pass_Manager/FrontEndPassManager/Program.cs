using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace FrontEndPassManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).UseUrls("http://localhost:5000/").Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
.UseStartup<Startup>();
        }
    }
}
