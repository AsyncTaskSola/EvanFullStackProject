using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EvanBackstageApi.Entity.CEG
{
    [SugarTable("Company")]
    public  class Company
    {
        /// <summary>
        /// 公司id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CurrentTime { get; set; }
        /// <summary>
        /// 公司邮箱
        /// </summary>
        public string CompanyEmail { get; set; }
        /// <summary>
        /// 公司联系方式
        /// </summary>
        public string CompanyPhone { get; set; }
        [SugarColumn(IsIgnore = true), NotMapped]
        public IEnumerable<Employee> Emplyees { get; set; }
    }
}
