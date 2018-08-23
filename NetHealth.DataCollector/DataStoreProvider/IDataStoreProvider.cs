
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetHealth.DataCollector.DataStoreProvider
{
    public interface IDataStoreProvider
    {
        Task AppendDataAsync(IEnumerable<NetPingResult> data, string hostName);
    }
}
