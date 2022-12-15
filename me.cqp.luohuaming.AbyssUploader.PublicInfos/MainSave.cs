using me.cqp.luohuaming.AbyssUploader.Sdk.Cqp;
using System.Collections.Generic;
using System.Threading;
using WebSocketSharp;

namespace me.cqp.luohuaming.AbyssUploader.PublicInfos
{
    public static class MainSave
    {
        /// <summary>
        /// 保存各种事件的数组
        /// </summary>
        public static List<IOrderModel> Instances { get; set; } = new List<IOrderModel>();
        public static CQLog CQLog { get; set; }
        public static CQApi CQApi { get; set; }
        public static string AppDirectory { get; set; }
        public static string ImageDirectory { get; set; }
        public static WebSocket WebSocketClient { get; set; }
        public static bool ExitFlag { get; set; } = false;
        public static void InitClient()
        {
            if (ExitFlag) return;
            if (WebSocketClient != null)
            {
                WebSocketClient.Close();
            }
            WebSocketClient = new WebSocket(Config.WebSocketURL);
            WebSocketClient.OnOpen += WebSocketClient_OnOpen;
            WebSocketClient.OnClose += WebSocketClient_OnClose;
            WebSocketClient.OnError += WebSocketClient_OnError;
            WebSocketClient.OnMessage += ClientMessageHandler.WebSocketClient_OnMessage;
            WebSocketClient.Connect();
        }


        private static void WebSocketClient_OnError(object sender, ErrorEventArgs e)
        {
            CQLog.Info("消息服务器连接", $"连接发生错误: {e.Message}");
        }

        private static void WebSocketClient_OnClose(object sender, CloseEventArgs e)
        {
            CQLog.Info("消息服务器连接", $"连接已断开，将在 {Config.ReconnectTimeout}ms 后重连");
            Thread.Sleep(Config.ReconnectTimeout);
            InitClient();
        }

        private static void WebSocketClient_OnOpen(object sender, System.EventArgs e)
        {
            CQLog.Info("消息服务器连接", $"已连接到消息服务器: {Config.WebSocketURL}");
            WebSocketClient.Send(new { Type = "Auth", Data = CQApi.GetLoginQQ().Id }.ToJson());
            new Thread(() =>
            {
                CQLog.Info("心跳线程", $"开始心跳线程，延时 {Config.HeartBeatTimeout}ms");
                while (true)
                {
                    bool flag = WebSocketClient.ReadyState != WebSocketState.Open;
                    if (flag) break;
                    int maxCount = Config.HeartBeatTimeout / 100;
                    for (int i = 0; i < maxCount; i++)
                    {
                        flag = WebSocketClient.ReadyState != WebSocketState.Open;
                        if (flag) break;
                        Thread.Sleep(100);
                    }
                    if (flag) break;
                    WebSocketClient.Send(new { Type="Heartbeat" }.ToJson());
                }
            }).Start();
        }
    }
}
