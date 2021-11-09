using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeleniumForInfoCamere;
using SeleniumForInfoCamere.Iterations.Infocamere;
using SeleniumForInfoCamere.Iterations.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test_SeleniumInfocamerfe
{
    public static class StartUp
    {
        public static void Initialize()
        {
            var builder = Host.CreateDefaultBuilder()
                 .ConfigureServices((hostContext, services) =>
                 {
                     //services.AddAutoMapper(typeof(Dati).Assembly);
                     services.AddTransient<ISeleniumChrome, SeleniumChrome>();
                     services.AddTransient<ISpiderService, SpiderService>();
                     services.AddTransient<IServiceInfoCamere, ServiceInfoCamere>();
                 }).UseConsoleLifetime();

            var host = builder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    var myService = services.GetRequiredService<ISpiderService>();
                    myService.Run();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Occured");
                }
            }

        }

    }

}
