using System;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using SendGrid;

[assembly: FunctionsStartup(typeof(AzFunc3.DIAndOptionPattern.Startup))]
namespace AzFunc3.DIAndOptionPattern
{
  public class Startup : FunctionsStartup
  {
    public override void Configure(IFunctionsHostBuilder builder)
    {
      //Adding instamce of HTTP Client as singleton
      builder.Services.AddHttpClient();

      //adding sendclient client as singleton using DI
      builder.Services.AddSingleton((sendGridClient) =>
      {
        return new SendGridClient(Environment.GetEnvironmentVariable("SendGridAPIKey"));
      });


      //reference https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection
      builder.Services
        .AddOptions<EmailConfiguration>()
              .Configure<IConfiguration>((settings, configuration) =>
                                  {
                                    configuration.Bind("EmailConfiguration", settings);
                                  });

      builder.Services
        .AddOptions<ConnectionStrings>()
              .Configure<IConfiguration>((settings, configuration) =>
              {
                configuration.Bind("ConnectionStrings", settings);
              });

      //builder.Services
      //       .AddOptions<EmailConfiguration>()
      //.Configure<IConfiguration>((settings, configuration) => { configuration.Bind("EmailConfiguration", settings); });
    }
  }
}
