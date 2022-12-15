﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using me.cqp.luohuaming.AbyssUploader.Sdk.Cqp.EventArgs;
using me.cqp.luohuaming.AbyssUploader.PublicInfos;
using me.cqp.luohuaming.AbyssUploader.Code.OrderFunctions;

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
                if(UploadAbyss.DelayUploadList.Contains((e.FromGroup, e.FromQQ)))
                {
                    result.Result = true;
                    UploadAbyss.DelayUploadImage(e);
                    return result;
                }
                if(UploadMemoryField.DelayUploadList.Contains((e.FromGroup, e.FromQQ)))
                {
                    result.Result = true;
                    UploadMemoryField.DelayUploadImage(e);
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
