using System;

namespace me.cqp.luohuaming.AbyssUploader.PublicInfos.API
{
    public class QueryInfo
    {
        public static APIResult QueryAbyssInfo()
        {
            string token = Guid.NewGuid().ToString();
            MainSave.WebSocketClient.Send(new APIResult { Type = "QueryAbyssInfo", Token = token }.ToJson());
            return ClientMessageHandler.WaitResult(token);
        }

        public static APIResult QueryMemoryFieldInfo()
        {
            string token = Guid.NewGuid().ToString();
            MainSave.WebSocketClient.Send(new APIResult { Type = "QueryMemoryFieldInfo", Token = token }.ToJson());
            return ClientMessageHandler.WaitResult(token);
        }
    }
}
