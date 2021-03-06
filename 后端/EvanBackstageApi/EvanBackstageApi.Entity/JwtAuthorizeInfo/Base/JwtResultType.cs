﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvanBackstageApi.Entity.JwtAuthorizeInfo.Base
{
    public class JwtResultType
    {
        /// <summary>
        /// 消息结果类型
        /// </summary>
        public const string Info = "Info";

        /// <summary>
        /// 成功结果类型
        /// </summary>
        public const string Success = "success";

        /// <summary>
        /// 警告结果类型
        /// </summary>        
        public const string Warning = "warning";

        /// <summary>
        /// 异常结果类型
        /// </summary>
        public const string Error = "error";

        /// <summary>
        /// 超时
        /// </summary>
        public const string Timeout = "timeout";
    }
    /// <summary>
    /// 通过消息返回类型
    /// </summary>
    /// <typeparam name="T">返回数据类型</typeparam>
    public class JwtResultModel<T>
    {
        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 返回数据 
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 错误对象
        /// </summary>
        public dynamic Error { get; set; }
        //总数

        public int? Total { get; set; }
    }
}
