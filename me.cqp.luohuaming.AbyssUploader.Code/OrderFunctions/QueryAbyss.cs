using me.cqp.luohuaming.AbyssUploader.PublicInfos;
using me.cqp.luohuaming.AbyssUploader.PublicInfos.API;
using me.cqp.luohuaming.AbyssUploader.Sdk.Cqp;
using me.cqp.luohuaming.AbyssUploader.Sdk.Cqp.EventArgs;
using System;
using System.IO;

namespace me.cqp.luohuaming.AbyssUploader.Code.OrderFunctions
{
    public class QueryAbyss : IOrderModel
    {
        public bool ImplementFlag { get; set; } = true;
        
        public string GetOrderStr() => "深渊快报";

        public bool Judge(string destStr) => destStr.Replace("＃", "#").StartsWith(GetOrderStr());

        public FunctionResult Progress(CQGroupMessageEventArgs e)
        {
            FunctionResult result = new FunctionResult
            {
                Result = true,
                SendFlag = true,
            };
            SendText sendText = new SendText
            {
                SendID = e.FromGroup,
            };
            result.SendObject.Add(sendText);
            APIResult apiResult = QueryInfo.QueryAbyssInfo();
            if(apiResult.IsSuccess)
            {
                APIResult.Info abyssInfo = apiResult.Data as APIResult.Info;
                string path = Path.Combine(MainSave.ImageDirectory, "AbyssUploader", $"{apiResult.Token}.png");
                if(!File.Exists(path))
                {
                    Directory.CreateDirectory(Path.Combine(MainSave.ImageDirectory, "AbyssUploader"));
                    File.WriteAllBytes(path, Convert.FromBase64String(abyssInfo.PicBase64));
                }
                sendText.MsgToSend.Add($"深渊慢报[{abyssInfo.UploadTime:G} {abyssInfo.UploadTime:ddd}]\n上传者{abyssInfo.UploaderName}");
                sendText.MsgToSend.Add(CQApi.CQCode_Image($"AbyssUploader\\{apiResult.Token}.png").ToString());
            }
            else
            {
                sendText.MsgToSend.Add($"未获取到结果，信息：{apiResult.Message}");
            }
            return result;
        }

        public FunctionResult Progress(CQPrivateMessageEventArgs e)
        {
            FunctionResult result = new FunctionResult
            {
                Result = false,
                SendFlag = false,
            };
            return result;
        }
    }
}
