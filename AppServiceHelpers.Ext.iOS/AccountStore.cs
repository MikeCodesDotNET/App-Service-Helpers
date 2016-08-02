using AppServiceHelpers.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Security;
using Foundation;
using System.Linq;

namespace AppServiceHelpers
{
    internal class AccountStore : IAccountStore
    {
        public static IAccountStore Create()
        {
            return new AccountStore();
        }

        public IEnumerable<Account> FindAccountsForProvider(string provider)
        {
            var query = new SecRecord(SecKind.GenericPassword);
            query.Service = provider;

            SecStatusCode result;
            var records = SecKeyChain.QueryAsRecord(query, 1000, out result);

            return records != null ? records.Select(GetAccountFromRecord).ToList() : new List<Account>();
        }

        Account GetAccountFromRecord(SecRecord r)
        {
            var serializeData = NSString.FromData(r.Generic, NSStringEncoding.UTF8);
            return Account.Deserialize(serializeData);
        }

        Account FindAccount(string username, string provider)
        {
            var query = new SecRecord(SecKind.GenericPassword);
            query.Service = provider;
            query.Account = username;

            SecStatusCode result;
            var record = SecKeyChain.QueryAsRecord(query, out result);

            return record != null ? GetAccountFromRecord(record) : null;
        }

        public void Save(Account account, string provider)
        {
            var statusCode = SecStatusCode.Success;
            var serializedAccount = account.Serialize();
            var data = NSData.FromString(serializedAccount, NSStringEncoding.UTF8);

            var existing = FindAccount(account.Username, provider);

            if (existing != null)
            {
                var query = new SecRecord(SecKind.GenericPassword);
                query.Service = provider;
                query.Account = account.Username;

                statusCode = SecKeyChain.Remove(query);
                if (statusCode != SecStatusCode.Success)
                {
                    throw new Exception("Could not save account to KeyChain: " + statusCode);
                }
            }

            var record = new SecRecord(SecKind.GenericPassword);
            record.Service = provider;
            record.Account = account.Username;
            record.Generic = data;
            record.Accessible = SecAccessible.WhenUnlocked;

            statusCode = SecKeyChain.Add(record);

            if (statusCode != SecStatusCode.Success)
            {
                throw new Exception("Could not save account to KeyChain: " + statusCode);
            }
        }
        
        public void Delete(Account account, string provider)
        {
            var query = new SecRecord(SecKind.GenericPassword);
            query.Service = provider;
            query.Account = account.Username;

            var statusCode = SecKeyChain.Remove(query);

            if (statusCode != SecStatusCode.Success)
            {
                throw new Exception("Could not delete account from Keychain: " + statusCode);
            }
        }

    }
}
