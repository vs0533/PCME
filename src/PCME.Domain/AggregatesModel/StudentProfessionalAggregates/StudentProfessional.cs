using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PCME.Domain.AggregatesModel.StudentProfessionalAggregates
{
    /// <summary>
    /// 专业技术人员 专业技术职务职称晋升方式
    /// </summary>
    public class StudentProfessional:Entity
    {
        public int SeriesId { get; private set; }
        public int ProfessionalTitleId { get; private set; }
        [Required]
        public DateTime GetProfessionalDate { get; private set; }
        /// <summary>
        /// 最高学历
        /// </summary>
        public string Education { get; private set; }
        /// <summary>
        /// 晋升方式
        /// </summary>
        public int PromoteCategoryId { get; private set; }
        public int AuditWorkUnitId { get; private set; }
    }
}
