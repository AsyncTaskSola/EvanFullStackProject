using System;
using System.Collections.Generic;
using System.Text;

namespace EvanBackstageApi.Entity
{
    public  class DataTableResult<T>
    {
        /// <summary>
        /// 总行数
        /// </summary>        
        public int Rows { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }

        public string ExpStr { get; set; }
    }
}
