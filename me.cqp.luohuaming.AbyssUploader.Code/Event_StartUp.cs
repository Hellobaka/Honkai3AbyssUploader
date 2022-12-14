using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using me.cqp.luohuaming.AbyssUploader.Code.OrderFunctions;
using me.cqp.luohuaming.AbyssUploader.Sdk.Cqp.EventArgs;
using me.cqp.luohuaming.AbyssUploader.Sdk.Cqp.Interface;
using me.cqp.luohuaming.AbyssUploader.PublicInfos;
using System.Reflection;
using System.IO;

namespace me.cqp.luohuaming.AbyssUploader.Code
{
    public class Event_StartUp : ICQStartup
    {
        public void CQStartup(object sender, CQStartupEventArgs e)
        {
            MainSave.AppDirectory = e.CQApi.AppDirectory;
            MainSave.CQApi = e.CQApi;
            MainSave.CQLog = e.CQLog;
            MainSave.ImageDirectory = CommonHelper.GetAppImageDirectory();
            ConfigHelper.ConfigFileName = Path.Combine(MainSave.AppDirectory, "Config.json");
            foreach (var item in Assembly.GetAssembly(typeof(Event_GroupMessage)).GetTypes())
            {
                if (item.IsInterface)
                    continue;
                foreach (var instance in item.GetInterfaces())
                {
                    if (instance == typeof(IOrderModel))
                    {
                        IOrderModel obj = (IOrderModel)Activator.CreateInstance(item);
                        if (obj.ImplementFlag == false)
                            break;
                        MainSave.Instances.Add(obj);
                    }
                }
            }
        }
    }
}
