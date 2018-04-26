using PCME.Api.Application.ParameBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace PCME.Api.Extensions
{
    public static class LinqWhereExtensions
    {
        public static IQueryable<TSource> Filter<TSource>(
            this IQueryable<TSource> query, IEnumerable<Filter> filter)
            where TSource:class
        {
            
            ParameterExpression parameter = Expression.Parameter(typeof(TSource),"s");
            Expression exp = null;
            //var plist = parameter.Type.GetRuntimeProperties().ToDictionary(z => z.Name);
            foreach (var item in filter)
            {
                Expression property = Expression.Property(parameter, item.Property);
                Expression value = Expression.Constant(item.Value);
                Expression contains = Expression.Call(property, "Contains", null, new Expression[] { value });
                exp = contains;
            }
            if (exp != null)
            {
                Expression<Func<TSource, bool>> pre =
                Expression.Lambda<Func<TSource, bool>>(exp, parameter);
                return query.Where(pre);
            }
            return query;
            
        }
    }
}
