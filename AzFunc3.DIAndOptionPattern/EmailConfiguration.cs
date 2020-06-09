
namespace AzFunc3.DIAndOptionPattern
{
  /// <summary>
  /// This class matches any custom properties that you will have application setting once deployed 
  /// or in local.settings.json:Values when debugging
  /// </summary>
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
}