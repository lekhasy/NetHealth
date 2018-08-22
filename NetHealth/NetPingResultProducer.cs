using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;


namespace NetHealth
{
    public class NetPingResultProducer
    {
        readonly NetPing netPing;
        List<NetPingEntry> pingResultBuffer = new List<NetPingEntry>();

        public NetPingResultProducer(string hostNameOrAddress, int delayTime)
        {
            netPing = new NetPing(hostNameOrAddress, delayTime);
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

        IEnumerable<NetPingResult> GetLatestPingResults()
        {
            lock (pingResultBuffer)
            {
                var firstInValidIndex = pingResultBuffer.FindIndex(en => !en.IsCompleted);

                if (firstInValidIndex == -1) return new List<NetPingResult>();

                var returnedResult = pingResultBuffer.Take(firstInValidIndex);
                pingResultBuffer = pingResultBuffer.Skip(firstInValidIndex).ToList();
                return returnedResult.Select(p => p.PingResult);
            }
        }

        public void StartPing()
        {
            netPing.Start();
        }
    }
}
