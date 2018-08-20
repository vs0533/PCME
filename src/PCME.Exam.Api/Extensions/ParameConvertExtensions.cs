
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PCME.Exam.Api.Extensions
{
    public static class ParameConvertExtensions
    {
        public static IEnumerable<T> ToObject<T>(this string json) {
            IEnumerable<T> Items = new List<T>();
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    Items = JsonConvert.DeserializeObject<IEnumerable<T>>(json);
                }
                catch (Exception)
                {
                    
                }
            }
            return Items;
        }
    }
}
