using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Application.ParameBinder
{
    public class Filter
    {
        public string Property { get; set; }
        public string Value { get; set; }
        public string Operator { get; set; }
}
}
