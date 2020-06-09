using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AzFunc3.DIAndOptionPattern
{
  public class HttpTriggerWithDIExample
  {
    //Dependency Injection readonly property for the htto client
    private readonly HttpClient _httpClient;

    //Dependency Injection readonly property for the Send Grid CLient
    private readonly SendGridClient _sendGridClient;

    //First way:Inline properties, using Ioption pattern for the Congifuration to get strongly typed configuration and not worry about environment.getproperty
    private readonly IOptions<EmailConfiguration> _options;
    
    //Second way: outside of values, using Ioption pattern for the Congifuration to get strongly typed configuration and not worry about environment.getproperty
    private readonly ConnectionStrings _connectionString;


    //Constructor for the Function
    public HttpTriggerWithDIExample(HttpClient httpClient,
                                    SendGridClient sendGridClient,
                                    IOptions<EmailConfiguration> options,
                                    IOptions<ConnectionStrings> conn)
    {
      //Assigning readonly instance of the Httpclient
      _httpClient = httpClient;

      //Assigning readonly instance of the Sendgrid client
      _sendGridClient = sendGridClient;

      //Assigning option using the IOption<Propertyname>
            _options = options;

      //Second way to use IOptions using the IOption<Propertyname>.vakues
      _connectionString = conn.Value;
    }

    [FunctionName("HttpTriggerWithDIExample")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
        ILogger log)
    {
      log.LogInformation("C# HTTP trigger function processed a request.");

      //Invoking a simple Get call using the httpclient
      var response = await _httpClient.GetAsync("https://microsoft.com").ConfigureAwait(false);

      //reading the response in email body
      var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

      //invoking the Ioption to get the email values from App setting or configs
      var message = MailHelper.CreateSingleEmail(new EmailAddress(_options.Value.FromMail),
                                                 new EmailAddress(_options.Value.ToEmail),
                                                 "Subject Test",
                                                 body, "");


      var resSg = await _sendGridClient.SendEmailAsync(message).ConfigureAwait(false);
      if (resSg.StatusCode == System.Net.HttpStatusCode.Accepted)
      {
        return new OkObjectResult("Now you know how to Implement DI and Options Pattern in the Azure Function");
      }
      else
      {
        return new BadRequestObjectResult("WOAA Something is Broken");
      }

      
      
    }
  }
}
