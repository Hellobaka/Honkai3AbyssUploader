using me.cqp.luohuaming.AbyssUploader.PublicInfos;
using me.cqp.luohuaming.AbyssUploader.Sdk.Cqp.EventArgs;

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
