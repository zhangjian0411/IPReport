using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IPReport.Client
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configHost => { })
                .ConfigureServices((context, services) =>
                {
                    services.Configure<IPReportOptions>(context.Configuration.GetSection(IPReportOptions.IPReport));

                    services.AddHostedService<IPReportWorkerService>();
                });
    }
}
