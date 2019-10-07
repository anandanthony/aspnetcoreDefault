using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASPNETCoreCertTest.Pages
{
    public class IndexModel : PageModel
    {
        public ILogger<IndexModel> Logger { get; }
        private readonly IConfiguration _config;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
        {
            Logger = logger;
            _config = config;
        }

        public void OnGet()
        {
            try
            {
                Console.WriteLine("I am here!");
                Logger.LogInformation("Log Information");
                string thumbPrint1 = _config.GetValue<string>("WEBSITE_LOAD_CERTIFICATES");
                string thumbPrint = ConfigurationManager.AppSettings["WEBSITE_LOAD_CERTIFICATES"];
                Logger.LogInformation("Thumbprint: " + thumbPrint + "Thumbprint2: "+thumbPrint1);
                if (!string.IsNullOrEmpty(thumbPrint1))
                {
                    Logger.LogInformation("----------Start GET Certificaate Details----------");
                    X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                    //System.Diagnostics.Trace.WriteLine("certificates in certStore: "+certStore.Certificates[0].SubjectName.Name);
                    certStore.Open(OpenFlags.ReadOnly);
                    X509Certificate2Collection certCollection = certStore.Certificates.Find(
                                                X509FindType.FindByThumbprint,
                                                // Replace below with your certificate's thumbprint
                                                "7ABECB3415C320F361F907F713A097115F969C42",
                                                false);

                    Logger.LogInformation("Cert Count: " + certCollection.Count.ToString());
                    // Get the first cert with the thumbprint
                    if (certCollection.Count > 0)
                    {
                        Logger.LogInformation("Inside Cert Details");
                        Logger.LogInformation("Cert Details-Value: " + certCollection[0].Version);
                        Logger.LogInformation("Cert Details-IssuerName: " + certCollection[0].IssuerName.Name);
                        Logger.LogInformation("Cert Details-SubjectName: " + certCollection[0].SubjectName.Name);
                        Logger.LogInformation("Cert Details-SignatureAlgorithm.FriendlyName: " + certCollection[0].SignatureAlgorithm.FriendlyName);
                        Logger.LogInformation("Cert Details-SignatureAlgorithm.Value: " + certCollection[0].SignatureAlgorithm.Value);

                        X509Certificate2 cert = certCollection[0];
                        // Use certificate
                        Console.WriteLine(cert.IssuerName.Name);
                    }
                    certStore.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.LogInformation("Error: " + ex.ToString());
            }
        }
    }
}
