namespace AzFunc3.DIAndOptionPattern
{

  /// <summary>
  /// This class matches ConnectionStrings that you will have application setting once deployed.
  /// or in local.settings.json:Values when debugging
  /// </summary>
  public class ConnectionStrings
  {
    /// <summary>
    /// Appsetting name for connection string, the value for which ideally needs to be in Keyvault.
    /// </summary>
    public string AzureCloudConnection { get; set; }
  }
}
