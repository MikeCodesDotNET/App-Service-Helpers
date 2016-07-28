using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppServiceHelpers.Abstractions;
using Mindscape.Raygun4Net;

namespace AppServiceHelpers.Logger.Raygun
{
    public class Logger : ILogger
    {

        public void Init(string apiKey)
        {
            RaygunClient.Attach(apiKey);
        }

        public bool IsInitialized()
        {
            if (RaygunClient.Current != null)
                return true;
            else
                return false;
        }

        public void Report(Exception ex)
        {
            RaygunClient.Current.Send(ex);
        }

        public void Report(Exception ex, Dictionary<string, string> data)
        {
            RaygunClient.Current.Send(ex, null, data);
        }

    }
}

