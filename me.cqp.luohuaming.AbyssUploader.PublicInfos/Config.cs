using System.Collections.Generic;

namespace me.cqp.luohuaming.AbyssUploader.PublicInfos
{
    public static class Config
    {
        public static string WebSocketURL { get; set; }
        public static int ReconnectTimeout { get; set; }
        public static int HeartBeatTimeout { get; set; }
        public static int APIWaitTimeout { get; set; }
        public static List<long> EnableGroup { get; set; } = new List<long>();
    }
}
