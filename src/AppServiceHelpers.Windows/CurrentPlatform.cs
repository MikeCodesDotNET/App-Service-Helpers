using AppServiceHelpers.Authentication;
using AppServiceHelpers.Platform;

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
            SQLitePCL.Batteries.Init();
        }
    }
}
