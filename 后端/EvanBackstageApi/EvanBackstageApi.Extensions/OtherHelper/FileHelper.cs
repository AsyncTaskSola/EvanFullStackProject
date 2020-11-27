using System;
using System.Collections.Generic;
using System.Text;

namespace EvanBackstageApi.Extensions.OtherHelper
{

    public class FileHelper : IDisposable
    {
        private bool _alreadyDispose = false;
        #region 构造函数
        public FileHelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
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
    }
}
