using me.cqp.luohuaming.AbyssUploader.PublicInfos;
using me.cqp.luohuaming.AbyssUploader.PublicInfos.API;
using me.cqp.luohuaming.AbyssUploader.Sdk.Cqp.EventArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace me.cqp.luohuaming.AbyssUploader.Code.OrderFunctions
{
    public class UploadMemoryField : IOrderModel
    {
        public static Dictionary<(long, long), APIResult.Info> DelayUploadList { get; set; } = new Dictionary<(long, long), APIResult.Info>();

        public bool ImplementFlag { get; set; } = true;

        public string GetOrderStr() => Config.UploadMemoryFieldOrder;

        public bool Judge(string destStr) => destStr.Replace("＃", "#").StartsWith(GetOrderStr());//这里判断是否能触发指令

        public FunctionResult Progress(CQGroupMessageEventArgs e)//群聊处理
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

            DelayUploadList.Add((e.FromGroup, e.FromQQ), new APIResult.Info());
            sendText.MsgToSend.Add("请在下条消息发送图片");
            return result;
        }

        public static void DelayUploadImage(CQGroupMessageEventArgs e)
        {
            var info = DelayUploadList[(e.FromGroup, e.FromQQ)];

            var img = e.Message.CQCodes.FirstOrDefault(x => x.IsImageCQCode);
            if (img == null) return;
            string imgPath = e.CQApi.ReceiveImage(img);
            string nickName = e.FromGroup.GetGroupMemberInfo(e.FromQQ).Nick;

            info.PicBase64 = Convert.ToBase64String(File.ReadAllBytes(imgPath));
            info.Uploader = e.FromQQ;
            info.UploaderName = nickName;

            if (Config.MemoryFieldRemarkEnable)
            {
                e.FromGroup.SendGroupMessage("如果有需要备注的请在接下来回复，若没有请回复“无”");
            }
            else
            {
                CallUpload(e);
            }
        }

        public static void DelayAddRemark(CQGroupMessageEventArgs e)
        {
            if(e.Message.Text.Trim() != "无")
            {
                var info = DelayUploadList[(e.FromGroup, e.FromQQ)];
                info.Remark = Regex.Replace(e.Message, "\\[CQ:.*?\\]", "");
            }
            CallUpload(e);
        }

        public static void CallUpload(CQGroupMessageEventArgs e)
        {
            var info = DelayUploadList[(e.FromGroup, e.FromQQ)];
            DelayUploadList.Remove((e.FromGroup, e.FromQQ));
            var apiResult = UploadInfo.UploadMemoryField(info.PicBase64, info.UploaderName, info.Uploader, info.Remark);
            if (apiResult.IsSuccess)
            {
                e.FromGroup.SendGroupMessage("上传成功，感谢你的贡献");
            }
            else
            {
                e.FromGroup.SendGroupMessage($"上传失败，信息：{apiResult.Message}");
            }
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
