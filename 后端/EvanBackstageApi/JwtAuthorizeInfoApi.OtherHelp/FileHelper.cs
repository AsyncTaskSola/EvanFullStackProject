using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace JwtAuthorizeInfoApi.OtherHelp
{
    public class FileHelper : IDisposable
    {

        private bool _alreadyDispose = false;
        #region 构造函数
        public FileHelper()
        {
        }
        ~FileHelper()
        {
            Dispose();
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (_alreadyDispose) return;
            _alreadyDispose = true;
        }
        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region 取得文件后缀名
        /****************************************
          * 函数名称：GetPostfixStr
          * 功能说明：取得文件后缀名
          * 参     数：filename:文件名称
          * 调用示列：
          *            string filename = "aaa.aspx";        
          *            string s = EC.FileObj.GetPostfixStr(filename);         
         *****************************************/
        /// <summary>
        /// 取后缀名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>.gif|.html格式</returns>
        public static string GetPostfixStr(string filename)

        {
            int start = filename.LastIndexOf(".");
            int length = filename.Length;
            string postfix = filename.Substring(start, length - start);
            string postfixs = filename.Split('.')[1].ToString();
            return postfix;
        }
        #endregion

        public static async Task<AvatarRes> WriteAvatar(IFormFile avatar, Guid uid, IHostingEnvironment _webHostEnvironment, IConfiguration _configuration)
        {
            var file = avatar;
            var types = new string[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
            var avatarRes = new AvatarRes();
            if (!types.Contains(file.ContentType))
            {
                avatarRes.Message = "上传头像图片只能是 JPG/JPEG/PNG/GIF 格式!";
                return avatarRes;
            }
            if (file.Length / 1024 / (double)1024 > 10)
            {
                avatarRes.Message = "上传头像图片大小不能超过 10MB!";
                return avatarRes;
            }
            var basePath = _webHostEnvironment.WebRootPath;
            // wwwroot下面的目录
            var avatarPath = Path.Combine(basePath, "avatar");
            // 是否存在avatar文件夹
            if (!Directory.Exists(avatarPath))
            {
                Directory.CreateDirectory(avatarPath);
            }
            var type = GetPostfixStr(file.FileName);
            var avatarFilePath = $"{avatarPath}/{uid}{type}";//存储路径
            using (var fs = new FileStream(avatarFilePath, FileMode.Create, FileAccess.Write))
            {
                using (var stream = file.OpenReadStream())
                {
                    var bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, bytes.Length);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            var link = _configuration.GetValue(typeof(String), "URLS") + $"/src/avatar/{uid}{type}";
            avatarRes.Message = link;
            avatarRes.Success = true;
            return avatarRes;
        }
        /// <summary>
        /// 头像响应类
        /// </summary>
        public class AvatarRes
        {
            public bool Success { get; set; }
            public string Message { get; set; }
        }
    }
}
