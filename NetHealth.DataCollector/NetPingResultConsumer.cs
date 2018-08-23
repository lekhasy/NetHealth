using System;
using System.Threading;
using System.Threading.Tasks;
using NetHealth.DataCollector.DataStoreProvider;

namespace NetHealth.DataCollector
{
    public class NetPingResultConsumer
    {
        readonly NetPingResultProducer ResultProducer;
        readonly int ConsumeDelayTime;
        readonly IDataStoreProvider DataStoreProvider;

        public NetPingResultConsumer(NetPingResultProducer resultProducer, int consumeDelayTime, Func<string, IDataStoreProvider> getDataStoreProvider)
        {
            ResultProducer = resultProducer;
            ConsumeDelayTime = consumeDelayTime;
            DataStoreProvider = getDataStoreProvider(ResultProducer.HostName);
        }

        void StartConsume()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    var latestPingResult = ResultProducer.GetLatestPingResults();
                    if (latestPingResult.Count == 0) return;

                    // persist result to datastore
                    await DataStoreProvider.AppendDataAsync(latestPingResult, ResultProducer.HostName);

                    Thread.Sleep(ConsumeDelayTime);
                }
            });
        }
    }
}
