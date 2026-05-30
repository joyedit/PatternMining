using ProtoBuf;

namespace PatternMining
{
    public static class NetworkConstants
    {
        public const string ChannelName = "patternmining";
    }

    // Client -> server: "advance my pattern to the next one".
    [ProtoContract]
    public class CyclePatternRequest { }

    // Server -> client: "your pattern is now this" (drives the chat display).
    [ProtoContract]
    public class PatternSelectMessage
    {
        [ProtoMember(1)] public string PatternKey;
    }
}
