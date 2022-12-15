using me.cqp.luohuaming.AbyssUploader.PublicInfos.API;
using me.cqp.luohuaming.AbyssUploader.Sdk.Cqp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using WebSocketSharp;

namespace me.cqp.luohuaming.AbyssUploader.PublicInfos
{
    public class ClientMessageHandler
    {
        private static Dictionary<string, APIResult> APIResults { get; set; } = new Dictionary<string, APIResult>();
        public static void WebSocketClient_OnMessage(object sender, MessageEventArgs e)
        {
            APIResult json = JsonConvert.DeserializeObject<APIResult>(e.Data);
            switch (json.Type)
            {
                case "UploadAbyssInfo":
                case "UploadMemoryField":
                case "QueryAbyssInfo":
                case "QueryMemoryFieldInfo":
                    APIResults.Add(json.Token, json);
                    break;
                case "BoardcastAbyss":
                    BoardcastAbyss(json);
                    break;
                case "BoardcastMemoryField":
                    BoardcastMemory(json);
                    break;
                case "Heartbeat":
                default:
                    break;
            }

        }

        public static APIResult WaitResult(string token)
        {
            int maxCount = Config.APIWaitTimeout / 100;
            for (int i = 0; i < maxCount; i++)
            {
                if (APIResults.ContainsKey(token))
                {
                    var r = APIResults[token];
                    APIResults.Remove(token);
                    return r;
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
            return new APIResult { IsSuccess = false, Message = "Timeout" };
        }

        private static void BoardcastAbyss(APIResult result)
        {
            MainSave.CQLog.Info("收到广播", "获取到深渊广播信息，开始发送消息");
            Directory.CreateDirectory(Path.Combine(MainSave.ImageDirectory, "AbyssUploader"));
            string filename = $"{result.Token}.jpg";
            APIResult.Info abyssInfo = result.Data as APIResult.Info;
            File.WriteAllBytes(Path.Combine(MainSave.ImageDirectory, "AbyssUploader", filename), Convert.FromBase64String(abyssInfo.PicBase64));
            foreach (var item in Config.EnableGroup.OrderBy(x => Guid.NewGuid().ToString()))
            {
                MainSave.CQApi.SendGroupMessage(item, $"深渊慢报[{abyssInfo.UploadTime:G} {abyssInfo.UploadTime:ddd}]\n上传者{abyssInfo.UploaderName}");
                MainSave.CQApi.SendGroupMessage(item, CQApi.CQCode_Image($"AbyssUploader\\{filename}"));
                Thread.Sleep(5 * 1000);
            }
        }

        private static void BoardcastMemory(APIResult result)
        {
            MainSave.CQLog.Info("收到广播", "获取到战场广播信息，开始发送消息");
            Directory.CreateDirectory(Path.Combine(MainSave.ImageDirectory, "AbyssUploader"));
            string filename = $"{result.Token}.jpg";
            APIResult.Info abyssInfo = result.Data as APIResult.Info;
            File.WriteAllBytes(Path.Combine(MainSave.ImageDirectory, "AbyssUploader", filename), Convert.FromBase64String(abyssInfo.PicBase64));
            foreach (var item in Config.EnableGroup.OrderBy(x => Guid.NewGuid().ToString()))
            {
                MainSave.CQApi.SendGroupMessage(item, $"战场慢报[{abyssInfo.UploadTime:G} {abyssInfo.UploadTime:ddd}]\n上传者{abyssInfo.UploaderName}");
                MainSave.CQApi.SendGroupMessage(item, CQApi.CQCode_Image($"AbyssUploader\\{filename}"));
                Thread.Sleep(5 * 1000);
            }
        }
    }
}
