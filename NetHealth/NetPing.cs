using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace NetHealth
{
    public class NetPing
    {
        readonly string hostNameOrAddress = null;
        readonly int delayTime = 0;
        readonly Ping ping = new Ping();
        uint requestCount = 0;

        public NetPing(string hostNameOrAddress, int delayTime)
        {
            this.hostNameOrAddress = hostNameOrAddress;
            this.delayTime = delayTime;
        }

        public event EventHandler<NetPingStartedEventArgs> NewPingStarted = null;
        public event EventHandler<NetPingReceivedEventArgs> PingResultReceived = null;

        public void Start()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var sequence = requestCount++;
                    NewPingStarted?.Invoke(this, new NetPingStartedEventArgs(sequence));
                    Task.Run(async () =>
                   {
                       var startedMoment = DateTime.Now;
                       var pingReply = await ping.SendPingAsync(hostNameOrAddress);
                       PingResultReceived?.Invoke(this, new NetPingReceivedEventArgs(pingReply, startedMoment, sequence));
                   });
                    Thread.Sleep(delayTime);
                }
            });
        }

    }

    public class NetPingStartedEventArgs : EventArgs
    {
        public readonly uint Sequence;
        public NetPingStartedEventArgs(uint sequence)
        {
            Sequence = sequence;
        }
    }

    public class NetPingReceivedEventArgs : EventArgs
    {
        public readonly uint Sequence;
        public readonly PingReply PingReply;
        public readonly DateTime StartedMoment;

        public NetPingReceivedEventArgs(PingReply pingReply, DateTime startedMoment, uint sequence)
        {
            this.PingReply = pingReply;
            this.StartedMoment = startedMoment;
            this.Sequence = sequence;
        }
    }

}
