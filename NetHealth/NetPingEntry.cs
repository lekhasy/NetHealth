using System;
namespace NetHealth
{
    public class NetPingEntry
    {
        public NetPingEntry(uint sequence)
        {
            Sequence = sequence;
        }

        bool isCompleted = false;
        public NetPingResult PingResult { get; private set; }
        public uint Sequence { get; private set; }

        public void MarkAsCompleted(NetPingResult result)
        {
            isCompleted = true;
            PingResult = result;
        }

        public bool IsCompleted
        {
            get
            {
                return isCompleted;
            }
        }
    }
}
