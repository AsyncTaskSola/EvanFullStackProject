using SqlSugar;

namespace EvanBackstageApi.Entity.JwtAuthorizeInfo.Root
{
    //角色表
    public  class Role:RootEntity
    {
        /// <summary>
        /// 角色名
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 20)]
        public string Name { get; set; }
        /// <summary>
        /// 角色编码
        /// </summary>
        [SugarColumn(Length = 20)]
        public string Code { get; set; }
        /// <summary>
        /// 权限描述
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 40)]
        public string Describe { get; set; }
    }
}
