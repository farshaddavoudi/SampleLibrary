using ATA.Library.Server.Model.AppSettings;
using ATA.Library.Shared.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using System;
using System.IO;
using System.Reflection;

namespace ATA.Library.Server.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //configure logging first
            ConfigureLogging();

            //then create the host, so that if the host fails we can log errors
            CreateHost(args);
        }

        private static void ConfigureLogging()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.WithProperty("ApplicationName", AppStrings.AppProjectName)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithMachineName()
                .Enrich.WithClientIp()
                .Enrich.WithClientAgent()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Seq(
                    //MagicString=>[AppSettings:Seq:ServerUrl]
                    configuration[$"{nameof(AppSettings)}:{nameof(Model.AppSettings.Seq)}:{nameof(Model.AppSettings.Seq.ServerUrl)}"],
                    //MagicString=>["AppSettings:Seq:ApiKey"]
                    apiKey: configuration[$"{nameof(AppSettings)}:{nameof(Model.AppSettings.Seq)}:{nameof(Model.AppSettings.Seq.ApiKey)}"])
                .Enrich.WithProperty("Environment", environment)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static void CreateHost(string[] args)
        {
            try
            {
                Log.Information("Starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (System.Exception ex)
            {
                Log.Fatal($"Failed to start {Assembly.GetExecutingAssembly().GetName().Name}", ex);
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration(configuration =>
                {
                    configuration.SetBasePath(Directory.GetCurrentDirectory());
                    configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    configuration.AddJsonFile(
                        $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                        optional: true);
                    configuration.AddEnvironmentVariables();
                })
                .UseSerilog();
    }
}
