using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Security.Credentials;

namespace AppServiceHelpers
{
    internal class AccountStore
    {
        public static void SaveAccount(Dictionary<string, string> account)
        {
            var vault = new PasswordVault();
            vault.Add(new PasswordCredential
            {
                UserName = account["userId"],
                Password = account["authenticationToken"],
                Resource = $"appServiceHelpers/{account["identityProvider"]}"
            });

            System.Diagnostics.Debug.Write("TEST");
        }

        public static Dictionary<string, string> GetAccount()
        {
            var vault = new PasswordVault();
            var passwords = vault.RetrieveAll();
            foreach (var pass in passwords)
            {
                if (pass.Resource.Contains("appServiceHelpers"))
                {
                    pass.RetrievePassword();
                    // TODO: Sort out why UserName and Password fields are getting switched around...
                    var dict = new Dictionary<string, string>
                    {
                        { "authenticationToken", pass.UserName },
                        { "userId", pass.Password },
                        { "identityProvider", pass.Resource.Split('/')[1] }
                    };

                    return dict;
                }
            }

            return null;
        }

        public static MobileServiceAuthenticationProvider GetIdentityProvider()
        {
            var account = GetAccount();

            var identityProvider = account["identityProvider"];
            switch (identityProvider)
            {
                case "Facebook":
                    return MobileServiceAuthenticationProvider.Facebook;
                case "Twitter":
                    return MobileServiceAuthenticationProvider.Twitter;
                case "Google":
                    return MobileServiceAuthenticationProvider.Google;
                case "MicrosoftAccount":
                    return MobileServiceAuthenticationProvider.MicrosoftAccount;
                case "WindowsAzureActiveDirectory":
                    return MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory;
            }

            return MobileServiceAuthenticationProvider.Google;
        }
    }
}
