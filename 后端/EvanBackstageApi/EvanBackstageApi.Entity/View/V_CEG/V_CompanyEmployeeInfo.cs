using EvanBackstageApi.Entity.CEG;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EvanBackstageApi.Entity.View.V_CEG
{
    [SugarTable("View_CompanyEmployeeInfo")]
    public class V_CompanyEmployeeInfo
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
        public Guid CompanyId { get; set; }
        public string EmplyeeNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Gender Gender { get; set; }
        public DateTime DateofBirth { get; set; }
        [SugarColumn(IsIgnore = true), NotMapped]
        public Company Company { get; set; }
        [SugarColumn(IsIgnore = true), NotMapped]
        public IEnumerable<Employee> Emplyees { get; set; }
    }
}
