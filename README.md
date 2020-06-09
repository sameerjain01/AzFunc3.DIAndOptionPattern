# Azure Function using DI and IOption Pattern

This is simple start project to start your new Azure Serverless Project using Dependency Injection and Options pattern.

 1. I created this project becasue I was no open to go back and find how I did DI and Options in my last project 
    
 2. I was tired of keeping track of Environment.getPropertyname()
 
 ## Use Case
1. The use case for Dependency Injection is fairly obvious you need to insert different client based on your workflow.
2. The use case for IOption is best practice. If you are like me I don't want to change code in multiple place and would go
any length (while practical) to find a way to reduce the duplication and increase maintenance and extensibility.

IOption Using IOptions<ClassName> 
![IOption Using IOptions<ClassName>](https://github.com/sameerjain01/AzFunc3.DIAndOptionPattern/blob/master/IOptionONRuntime.png)

IOption Using Values 
![IOption Using Values](https://github.com/sameerjain01/AzFunc3.DIAndOptionPattern/blob/master/IoptionMethod2.PNG)


## Code
### Startup.cs Setting Up Dependecy Injection

```
//Please note I am using FunctionStartUp instead IWebJobStartup because I need the IOption and configure services
[assembly: FunctionsStartup(typeof(AzFunc3.DIAndOptionPattern.Startup))] 
namespace AzFunc3.DIAndOptionPattern
{
  public class Startup : FunctionsStartup
  {
    public override void Configure(IFunctionsHostBuilder builder)
    {
    //your code
    }
   }
  }
  ```
### Startup.cs Configuring Services
```
 //Adding instamce of HTTP Client as singleton
      builder.Services.AddHttpClient();

      //adding sendclient client as singleton using DI
      builder.Services.AddSingleton((sendGridClient) =>
      {
        return new SendGridClient(Environment.GetEnvironmentVariable("SendGridAPIKey"));
      });


      //reference https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection

      //This used for adding a singleton instance of custom properties that you will have insides "Values"
      //please note I have tries using EmailConfiguration outside of "Values" but wasn't successful. 
      //If you know how to do that kindly share
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
```

## Plain Old CLR Object (POCO) for IOptions
 *EmailConfiguration*

```
 public class EmailConfiguration
  {
    public const string CustomPropertiesNames = "EmailConfiguration";

    /// <summary>
    /// From Email address
    /// EmailConfiguration:FromMail
    /// </summary>
    public string FromMail { get; set; }

    /// <summary>
    /// TO Email Address
    /// EmailConfiguration:ToEmail
    /// </summary>
    public string ToEmail { get; set; }
  }
```

 *ConnectionStrings*
```
public class ConnectionStrings
  {
    /// <summary>
    /// Appsetting name for connection string, the value for which ideally needs to be in Keyvault.
    /// </summary>
    public string AzureCloudConnection { get; set; }
  }
```

## Gotcha
The Ioption only works if you are using your custom class inside "Values" or Using the ConnectionStrings 
which is standard Appsetting when you deploy to Azure. This is not unusual at all but can trip you
if you are not thinking how final solution will be deployed.

```
{
  
  "Values": {
   "EmailConfiguration:FromMail": "<sender email>",
    "EmailConfiguration:ToEmail": "<receiver email>"
  },
  "ConnectionStrings": {
    "AzureCloudConnection": "SOME SQL CONNECTION Stored Ideally in Key vault or your app settings"
  }

}
```

 ## Dependencies
 1. Microsoft.Azure.Functions.Extensions 1.0.0
 2. Microsoft.Azure.WebJobs.Extensions.SendGrid 3.0.0
 3. Microsoft.Extensions.Http 3.1.4
 4. Microsoft.NET.Sdk.Functions 3.0.7

Built using Azure Function with C# in Visual Studio 2019

## Author
* Sameer

## Licence
Under MIT Licence. Please feel free to use and extend.

## Acknowledgement
Microsoft Doc
