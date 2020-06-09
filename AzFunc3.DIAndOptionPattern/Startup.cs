using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

      //This used for adding a singleton instance of custom properties that you will have insides "Values"
      //please note I have tries using EmailConfiguration outside of "Values" but wasn't successful. If you know how to do that kindly share
      builder
        .Services
        .AddOptions<EmailConfiguration>()
        .Configure<IConfiguration>((settings, configuration) =>
                                  {
                                    configuration.Bind(EmailConfiguration.CustomPropertiesNames, settings);
                                  });

      //This is custom properties outside of "Values", 
      builder
        .Services
        .AddOptions<ConnectionStrings>()
        .Configure<IConfiguration>((settings, configuration) =>
        {
          configuration.Bind("ConnectionStrings", settings);
        });


    }
  }
}
