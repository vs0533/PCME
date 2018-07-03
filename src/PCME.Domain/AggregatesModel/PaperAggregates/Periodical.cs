using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.PaperAggregates
{
    /// <summary>
    /// 刊物信息
    /// </summary>
    public class Periodical:Entity,IAggregateRoot
    {
        public string Name { get; set; }
        public string Level { get; set; }
        /// <summary>
        /// 简拼
        /// </summary>
        public string Logogram { get; set; }

        
    }
}
