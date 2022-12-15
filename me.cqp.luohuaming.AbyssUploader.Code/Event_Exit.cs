using me.cqp.luohuaming.AbyssUploader.PublicInfos;
using me.cqp.luohuaming.AbyssUploader.Sdk.Cqp.EventArgs;
using me.cqp.luohuaming.AbyssUploader.Sdk.Cqp.Interface;

namespace me.cqp.luohuaming.AbyssUploader.Code
{
    public class Event_Exit : ICQExit
    {
        public void CQExit(object sender, CQExitEventArgs e)
        {
            MainSave.ExitFlag = true;
        }
    }
}
