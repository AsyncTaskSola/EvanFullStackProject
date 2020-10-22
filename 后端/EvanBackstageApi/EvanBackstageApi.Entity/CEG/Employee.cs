using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EvanBackstageApi.Entity.CEG
{
    [SugarTable("Employee")]
    public class Employee
    {
        /// <summary>
        /// employee Id
        /// </summary>
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string EmplyeeNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Gender Gender { get; set; }
        public DateTime DateofBirth { get; set; }
        [SugarColumn(IsIgnore = true), NotMapped]
        public Company Company { get; set; }
    }
}
