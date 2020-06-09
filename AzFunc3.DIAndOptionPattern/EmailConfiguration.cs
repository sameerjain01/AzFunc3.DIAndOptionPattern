using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AzFunc3.DIAndOptionPattern
{
  public class EmailConfiguration
  {
    //This property must match the key from the local.setting.json or the Azurefunction once deployed
    //Having this as contant here allows us to not worry about using configuration.GetSection()
    // public const string SettingName = "EmailConfiguration";
    public string FromMail { get; set; }
    public string ToEmail { get; set; }
  }
}