using me.cqp.luohuaming.AbyssUploader.PublicInfos;
using me.cqp.luohuaming.AbyssUploader.PublicInfos.API;
using me.cqp.luohuaming.AbyssUploader.Sdk.Cqp.EventArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace me.cqp.luohuaming.AbyssUploader.Code.OrderFunctions
{
    public class UploadAbyss : IOrderModel
    {
        public static List<(long, long)> DelayUploadList { get; set; } = new List<(long, long)>();
        public bool ImplementFlag { get; set; } = true;

        public string GetOrderStr() => Config.UploadAbyssOrder;

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
            //if (UploadInfo.QueryUploadAbyssState().IsSuccess is false)
            //{
            //    sendText.MsgToSend.Add("本周期已有用户上传了深渊快报，若想查询请输入 深渊快报。若想重复上传，请重新输入上传指令，不过不会进行推送。");
            //    return result;
            //}
            if (e.Message.CQCodes.Any(x => x.IsImageCQCode))
            {
                DelayUploadImage(e);
            }
            else
            {
                DelayUploadList.Add((e.FromGroup, e.FromQQ));
                sendText.MsgToSend.Add("请在下条消息发送图片");
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

        public static void DelayUploadImage(CQGroupMessageEventArgs e)
        {
            var img = e.Message.CQCodes.FirstOrDefault(x => x.IsImageCQCode);
            if (img == null) return;
            DelayUploadList.Remove((e.FromGroup, e.FromQQ));
            string imgPath = e.CQApi.ReceiveImage(img);
            string nickName = e.FromGroup.GetGroupMemberInfo(e.FromQQ).Nick;
            var apiResult = UploadInfo.UploadAbyssInfo(Convert.ToBase64String(File.ReadAllBytes(imgPath)), nickName, e.FromQQ.Id);
            if (apiResult.IsSuccess)
            {
                e.FromGroup.SendGroupMessage("上传成功，感谢你的贡献");
            }
            else
            {
                e.FromGroup.SendGroupMessage($"上传失败，信息：{apiResult.Message}");
            }
        }
    }
}
