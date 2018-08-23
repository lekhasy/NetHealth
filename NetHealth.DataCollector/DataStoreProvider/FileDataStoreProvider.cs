using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NetHealth.DataCollector.DataStoreProvider
{
    public class FileDataStoreProvider : IDataStoreProvider
    {
        DirectoryInfo dataDirectory;
        FileInfo CurrentAppendingFile;
        DateTime LastSavingDatePackage;

        public FileDataStoreProvider(string hostName)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(hostName);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                dataDirectory = new DirectoryInfo($"data/{sb.ToString()}");
                if (!dataDirectory.Exists) dataDirectory.Create();
            }
        }

        FileInfo GetCurrentAppendingFile(DateTime savingDatePackage)
        {
            throw new NotImplementedException();
            if (LastSavingDatePackage == null)
            {
                CurrentAppendingFile = new FileInfo($"{savingDatePackage.Year}{savingDatePackage.Month}{savingDatePackage.Day}");
                dataDirectory.GetFiles($"{savingDatePackage.Year}{savingDatePackage.Month}{savingDatePackage.Day}", SearchOption.TopDirectoryOnly);
            }
        }

        public Task AppendDataAsync(IEnumerable<NetPingResult> data, string hostName)
        {
            throw new NotImplementedException();

            LastSavingDatePackage = data.First().StartedMoment;
        }
    }
}
