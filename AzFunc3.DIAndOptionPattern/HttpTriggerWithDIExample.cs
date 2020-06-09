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
    private readonly ConnectionStrings _conn;


    public HttpTriggerWithDIExample(HttpClient httpClient,
                                    SendGridClient sendGridClient,
                                    IOptions<EmailConfiguration> options,
                                    IOptions<ConnectionStrings> conn)
    {
      _httpClient = httpClient;
      _sendGridClient = sendGridClient;
      _options = options;
      _conn = conn.Value;
    }

    [FunctionName("HttpTriggerWithDIExample")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
        ILogger log)
    {
      log.LogInformation("C# HTTP trigger function processed a request.");

      var response = await _httpClient.GetAsync("https://microsoft.com").ConfigureAwait(false);

      var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
      var message = MailHelper.CreateSingleEmail(new EmailAddress(_options.Value.FromMail),
                                                 new EmailAddress(_options.Value.ToEmail),
                                                 "Subject Test",
                                                 body, "");


      var resSg = await _sendGridClient.SendEmailAsync(message).ConfigureAwait(false);



      return new OkObjectResult("I win!");
    }
  }
}
