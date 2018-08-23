using System;
using System.Net.NetworkInformation;

namespace NetHealth.DataCollector
{
    public class NetPingResult
    {
        public readonly long RoundTripTime;
        public readonly IPStatus Status;
        public readonly DateTime StartedMoment;

        public NetPingResult(long roundTripTime, IPStatus status, DateTime startedMoment)
        {
            RoundTripTime = roundTripTime;
            Status = status;
            StartedMoment = startedMoment;
        }
    }
}
