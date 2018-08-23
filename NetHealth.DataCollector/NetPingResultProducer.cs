using System;
using System.Collections.Generic;
using System.Linq;

namespace NetHealth.DataCollector
{
    public class NetPingResultProducer
    {
        readonly NetPing netPing;
        List<NetPingEntry> pingResultBuffer = new List<NetPingEntry>();
        public string HostName
        {
            get
            {
                return netPing.HostName;
            }
        }

        public NetPingResultProducer(string hostName, int delayTime)
        {
            netPing = new NetPing(hostName, delayTime);
            netPing.PingResultReceived += NetPing_PingResultReceived;
            netPing.NewPingStarted += NetPing_NewPingStarted;
        }

        void NetPing_NewPingStarted(object sender, NetPingStartedEventArgs e)
        {
            pingResultBuffer.Add(new NetPingEntry(e.Sequence));
        }

        void NetPing_PingResultReceived(object sender, NetPingReceivedEventArgs e)
        {
            lock (pingResultBuffer)
            {
                var entry = pingResultBuffer.Find(etr => etr.Sequence == e.Sequence);
                entry.MarkAsCompleted(new NetPingResult(e.PingReply.RoundtripTime, e.PingReply.Status, e.StartedMoment));
            }
        }

        public List<NetPingResult> GetLatestPingResults()
        {
            lock (pingResultBuffer)
            {
                var first = pingResultBuffer.FirstOrDefault();

                var firstInValidIndex = pingResultBuffer.FindIndex(en => !en.IsCompleted);

                if (firstInValidIndex == -1) return new List<NetPingResult>();

                // the result belong to two separate day
                // so, just return for the previous day, not today
                if (pingResultBuffer[firstInValidIndex - 1].PingResult.StartedMoment.Day != pingResultBuffer.First().PingResult.StartedMoment.Day)
                {
                    firstInValidIndex = pingResultBuffer
                        .FindIndex(en => en.PingResult.StartedMoment.Day != pingResultBuffer.First().PingResult.StartedMoment.Day);
                }

                var returnedResult = pingResultBuffer.Take(firstInValidIndex);
                pingResultBuffer = pingResultBuffer.Skip(firstInValidIndex).ToList();
                return returnedResult.Select(p => p.PingResult).ToList();
            }
        }

        public void StartProduce()
        {
            netPing.Start();
        }
    }
}
