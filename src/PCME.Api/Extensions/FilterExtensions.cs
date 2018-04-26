using PCME.Api.Application.ParameBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PCME.Api.Extensions
{
    public static class FilterExtensions
    {
        public static IEnumerable<Filter> ToFilter(this string filter) {
            IEnumerable<Filter> filterItems = new List<Filter>();
            if (!string.IsNullOrEmpty(filter))
            {
                try
                {
                    filterItems = JsonConvert.DeserializeObject<IEnumerable<Filter>>(filter);
                }
                catch (Exception)
                {
                    
                }
            }
            return filterItems;
        }
    }
}
