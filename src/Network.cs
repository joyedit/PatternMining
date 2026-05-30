using ProtoBuf;

namespace PatternMining
{
    public static class NetworkConstants
    {
        public const string ChannelName = "patternmining";
    }

    [ProtoContract]
    public class PatternSelectMessage
    {
        [ProtoMember(1)] public string PatternKey;
    }
}
