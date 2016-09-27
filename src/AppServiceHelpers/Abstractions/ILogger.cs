using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServiceHelpers.Abstractions
{
    public interface ILogger
    {
        void Report(Exception ex, Dictionary<string, string> data);

        void Report(Exception ex);

        bool IsInitialized();
    }
}

