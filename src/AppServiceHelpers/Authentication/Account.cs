using System;
using System.Collections.Generic;
using System.Text;

namespace AppServiceHelpers.Authentication
{
    public class Account
    {
        public Account()
        {

        }
        public Account(string username, IDictionary<string, string> properties)
        {
            Username = username;
            Properties = (properties == null) ? new Dictionary<string, string>() : new Dictionary<string, string>(properties);
        }

        public virtual string Username { get; set; }

        public virtual Dictionary<string, string> Properties { get; private set; }

        public string Serialize()
        {
            var sb = new StringBuilder();

            sb.Append("__username__=");
            sb.Append(Uri.EscapeDataString(Username));

            foreach (var p in Properties)
            {
                sb.Append("&");
                sb.Append(Uri.EscapeDataString(p.Key));
                sb.Append("=");
                sb.Append(Uri.EscapeDataString(p.Value));
            }
            return sb.ToString();
        }

        public static Account Deserialize(string serializedString)
        {
            var acct = new Account();

            foreach (var p in serializedString.Split('&'))
            {
                var kv = p.Split('=');

                var key = Uri.UnescapeDataString(kv[0]);
                var val = kv.Length > 1 ? Uri.UnescapeDataString(kv[1]) : "";

                if (key == "__username__")
                {
                    acct.Username = val;
                }
                else
                {
					if (acct.Properties != null)
                    	acct.Properties[key] = val;
                }
            }

            return acct;
        }

        public override string ToString()
        {
            return Serialize();
        }
    }
}
