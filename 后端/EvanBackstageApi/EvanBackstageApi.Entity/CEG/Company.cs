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
        public Guid Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Introduction { get; set; }
        [SugarColumn(IsIgnore = true), NotMapped]
        public IEnumerable<Employee> Emplyees { get; set; }
    }
}
