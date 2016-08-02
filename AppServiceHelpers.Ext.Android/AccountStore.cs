using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AppServiceHelpers.Authentication;
using Java.Security;
using Java.IO;
using Javax.Crypto;

namespace AppServiceHelpers
{
    internal class AccountStore : IAccountStore
    {
        Context context;
        KeyStore ks;
        KeyStore.PasswordProtection prot;

        static readonly object fileLock = new object();

        const string FileName = "AppServiceHelpers";
        static readonly char[] Password = "058abb8e9c954489a38c97750212ab44a8acbf318b444f6c9bab9d2b5f919eff058abb8e9c954489a38c97750212ab44".ToCharArray();

        public AccountStore(Context context)
        {
            this.context = context;
            ks = KeyStore.GetInstance(KeyStore.DefaultType);
            prot = new KeyStore.PasswordProtection(Password);

            try
            {
                lock(fileLock)
                {
                    using (var s = context.OpenFileInput(FileName))
                    {
                        ks.Load(s, Password);
                    }
                }
            }
            catch(FileNotFoundException)
            {
                LoadEmptyKeyStore(Password);
            }
        }

        public static IAccountStore Create(Context context)
        {
            return new AccountStore(context);
        }

        public void Delete(Account account, string provider)
        {
            var alias = MakeAlias(account, provider);
            ks.DeleteEntry(alias);
            Save();
        }

        public IEnumerable<Account> FindAccountsForProvider(string provider)
        {
            var r = new List<Account>();
            var postfix = "-" + provider;

            var aliases = ks.Aliases();
            while (aliases.HasMoreElements)
            {
                var alias = aliases.NextElement().ToString();
                if (alias.EndsWith(postfix))
                {
                    var e = ks.GetEntry(alias, prot) as KeyStore.SecretKeyEntry;
                    if (e != null)
                    {
                        var bytes = e.SecretKey.GetEncoded();
                        var serialized = Encoding.UTF8.GetString(bytes);
                        var acct = Account.Deserialize(serialized);
                        r.Add(acct);
                    }
                }
            }

            r.Sort((a, b) => a.Username.CompareTo(b.Username));
            return r;
        }

        public void Save(Account account, string provider)
        {
            var alias = MakeAlias(account, provider);

            var secretKey = new SecretAccount(account);
            var entry = new KeyStore.SecretKeyEntry(secretKey);
            ks.SetEntry(alias, entry, prot);

            Save();
        }

        void Save()
		{ 
			lock (fileLock) { 
				using (var s = context.OpenFileOutput(FileName, FileCreationMode.Private)) { 
					ks.Store(s, Password); 
				} 
			} 
		} 

         
 		static string MakeAlias(Account account, string serviceId)
 		{ 
 			return account.Username + "-" + serviceId; 
 		}

        class SecretAccount : Java.Lang.Object, ISecretKey 
		{ 
			byte[] bytes; 
			public SecretAccount(Account account)
			{ 
				bytes = Encoding.UTF8.GetBytes(account.Serialize ()); 
			} 
			public byte[] GetEncoded()
			{ 
				return bytes; 
			} 
			public string Algorithm
            { 
				get
                {
                    return "RAW";
                } 
			} 
			public string Format
            { 
				get
                {
                    return "RAW";
                } 
			} 
		}

        static IntPtr id_load_Ljava_io_InputStream_arrayC;
        void LoadEmptyKeyStore(char[] password)
 		{ 
 			if (id_load_Ljava_io_InputStream_arrayC == IntPtr.Zero) { 
 				id_load_Ljava_io_InputStream_arrayC = JNIEnv.GetMethodID(ks.Class.Handle, "load", "(Ljava/io/InputStream;[C)V"); 
 			} 
 			IntPtr intPtr = IntPtr.Zero; 
 			IntPtr intPtr2 = JNIEnv.NewArray(password); 
 			JNIEnv.CallVoidMethod(ks.Handle, id_load_Ljava_io_InputStream_arrayC, new JValue[] 
 				{ 
 					new JValue(intPtr), 
 					new JValue(intPtr2)
 				}); 
 			JNIEnv.DeleteLocalRef(intPtr); 
 			if (password != null) 
 			{ 
 				JNIEnv.CopyArray(intPtr2, password); 
 				JNIEnv.DeleteLocalRef(intPtr2); 
 			} 
 		} 

    }
}