using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppServiceHelpers
{
    public class CurrentPlatform : IPlatform
    {
        IAuthenticator IPlatform.Authenticator
        {
            get
            {
                return Authenticator.Instance;
            }
        }

        public static void Init()
        {
        }
    }
}
