using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace firstWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //选择配置文件appsetting.json
            var Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            
            Log.Logger = new LoggerConfiguration()
                //管道的配置由配置文件进行修改
                 .ReadFrom.Configuration(Configuration)
                 .CreateLogger();

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)           
                .UseStartup<Startup>()
                //Serilog.AspNetCore的方法
                .UseSerilog()
                .UseUrls("http://localhost:5001")
                .Build();
    }
}
