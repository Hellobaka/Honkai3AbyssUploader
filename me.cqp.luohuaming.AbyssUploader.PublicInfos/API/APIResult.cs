﻿using System;

namespace me.cqp.luohuaming.AbyssUploader.PublicInfos.API
{
    public class APIResult
    {
        public bool IsSuccess { get; set; } = false;
        public string Type { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public object Data { get; set; }
        public class Info
        {
            public int ID { get; set; }
            public string UploaderName { get; set; }
            public DateTime UploadTime { get; set; }
            public string PicBase64 { get; set; }
            public long Uploader { get; set; }
            public string Remark { get; set; }
        }
    }
}
