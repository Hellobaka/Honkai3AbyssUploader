﻿using me.cqp.luohuaming.AbyssUploader.Sdk.Cqp.Model;
using System;
using System.Xml.Linq;

namespace me.cqp.luohuaming.AbyssUploader.PublicInfos.API
{
    public class UploadInfo
    {
        public static APIResult QueryUploadAbyssState()
        {
            string token = Guid.NewGuid().ToString();
            APIResult request = new APIResult
            {
                Type = "QueryUploadAbyssState",
                Token = token
            }; 
            MainSave.WebSocketClient.Send(request.ToJson());
            return ClientMessageHandler.WaitResult(token);
        }
        public static APIResult QueryUploadMemoryFieldState()
        {
            string token = Guid.NewGuid().ToString();
            APIResult request = new APIResult
            {
                Type = "QueryUploadMemoryFieldState",
                Token = token
            }; 
            MainSave.WebSocketClient.Send(request.ToJson());
            return ClientMessageHandler.WaitResult(token);
        }

        public static APIResult UploadAbyssInfo(string base64, string name)
        {
            string token = Guid.NewGuid().ToString();
            APIResult request = new APIResult
            {
                Type = "UploadAbyssInfo",
                Token = token,
                Data = new APIResult.Info
                {
                    UploaderName = name,
                    UploadTime = DateTime.Now,
                    PicBase64 = base64,
                }
            };
            MainSave.WebSocketClient.Send(request.ToJson());
            return ClientMessageHandler.WaitResult(token);
        }
        public static APIResult UploadMemoryField(string base64, string name)
        {
            string token = Guid.NewGuid().ToString();
            APIResult request = new APIResult
            {
                Type = "UploadMemoryField",
                Token = token,
                Data = new APIResult.Info
                {
                    UploaderName = name,
                    UploadTime = DateTime.Now,
                    PicBase64 = base64,
                }
            };
            MainSave.WebSocketClient.Send(request.ToJson());
            return ClientMessageHandler.WaitResult(token);
        }
    }
}
