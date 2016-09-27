using AppServiceHelpers.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppServiceHelpers.Authentication
{
    public interface IAccountStore
    {
        IEnumerable<Account> FindAccountsForProvider(string provider);

        void Save(Account account, string provider);

        void Delete(Account account, string provider);
    }
}
