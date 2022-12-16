using me.cqp.luohuaming.AbyssUploader.Code.OrderFunctions;
using me.cqp.luohuaming.AbyssUploader.PublicInfos;
using me.cqp.luohuaming.AbyssUploader.Sdk.Cqp.EventArgs;
using System;
using System.Linq;

namespace me.cqp.luohuaming.AbyssUploader.Code
{
    public class Event_GroupMessage
    {
        public static FunctionResult GroupMessage(CQGroupMessageEventArgs e)
        {
            FunctionResult result = new FunctionResult()
            {
                SendFlag = false
            };
            try
            {
                if (Config.EnableGroup.Any(x => x == e.FromGroup) is false) return result;
                if(UploadAbyss.DelayUploadList.ContainsKey((e.FromGroup, e.FromQQ)))
                {
                    result.Result = true;
                    var info = UploadAbyss.DelayUploadList[(e.FromGroup, e.FromQQ)];
                    if(string.IsNullOrEmpty(info.PicBase64))
                    {
                        UploadAbyss.DelayUploadImage(e);
                    }
                    else
                    {
                        UploadAbyss.DelayAddRemark(e);
                    }
                    return result;
                }
                if(UploadMemoryField.DelayUploadList.ContainsKey((e.FromGroup, e.FromQQ)))
                {
                    result.Result = true;
                    var info = UploadMemoryField.DelayUploadList[(e.FromGroup, e.FromQQ)];
                    if (string.IsNullOrEmpty(info.PicBase64))
                    {
                        UploadMemoryField.DelayUploadImage(e);
                    }
                    else
                    {
                        UploadMemoryField.DelayAddRemark(e);
                    }
                    return result;
                }
                foreach (var item in MainSave.Instances.Where(item => item.Judge(e.Message.Text)))
                {
                    return item.Progress(e);
                }
                return result;
            }
            catch (Exception exc)
            {
                MainSave.CQLog.Info("异常抛出", exc.Message + exc.StackTrace);
                return result;
            }
        }
    }
}
