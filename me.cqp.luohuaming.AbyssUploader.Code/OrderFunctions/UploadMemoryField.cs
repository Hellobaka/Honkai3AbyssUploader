using me.cqp.luohuaming.AbyssUploader.PublicInfos;
using me.cqp.luohuaming.AbyssUploader.Sdk.Cqp.EventArgs;

namespace me.cqp.luohuaming.AbyssUploader.Code.OrderFunctions
{
    public class UploadMemoryField : IOrderModel
    {
        public bool ImplementFlag { get; set; } = true;
        
        public string GetOrderStr() => "这里输入触发指令";

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

            sendText.MsgToSend.Add("这里输入需要发送的文本");
            result.SendObject.Add(sendText);
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
