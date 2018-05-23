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

        public static IQueryable<TSource> Navigate<TSource>(
            this IQueryable<TSource> query, IEnumerable<Navigate> navigates)
            where TSource : class
        {
            ParameterExpression parameter = Expression.Parameter(typeof(TSource), "s");
            Expression exp = null;
            foreach (var item in navigates)
            {
                try
                {
                    Expression property = Expression.Property(parameter, "PID");
                    Expression value = Expression.Constant(item.FieldValue, typeof(int?));
                    Expression equal = Expression.Equal(property, value);
                    exp = exp == null ? equal : Expression.Or(exp, equal);

                }
                catch
                {
                }
            }
            if (exp != null)
            {
                Expression<Func<TSource, bool>> pre = Expression.Lambda<Func<TSource, bool>>(exp, parameter);
                return query.Where(pre);
            }
            return query;
        }
        public static IQueryable<TSource> FilterAnd<TSource>(
        this IQueryable<TSource> query, IEnumerable<Filter> filter)
        where TSource : class
        {

            ParameterExpression parameter = Expression.Parameter(typeof(TSource), "s");
            Expression exp = null;
            //var plist = parameter.Type.GetRuntimeProperties().ToDictionary(z => z.Name);
            foreach (var item in filter)
            {
                try
                {
                    var contains = GetMemberExpression(parameter, typeof(TSource), item.Property, item.Value);
                    //Expression property = Expression.Property(parameter, item.Property);
                    //Expression value = Expression.Constant(item.Value);
                    //Expression contains = Expression.Call(property, "Contains", null, new Expression[] { value });
                    exp = exp == null ? contains : Expression.And(exp, contains);
                }
                catch (Exception)
                {
                }
            }
            if (exp != null)
            {
                Expression<Func<TSource, bool>> pre = Expression.Lambda<Func<TSource, bool>>(exp, parameter);
                return query.Where(pre);
            }
            return query;

        }


        public static IQueryable<TSource> FilterOr<TSource>(
        this IQueryable<TSource> query, IEnumerable<Filter> filter)
        where TSource : class
        {

            ParameterExpression parameter = Expression.Parameter(typeof(TSource), "s");
            Expression exp = null;
            //var plist = parameter.Type.GetRuntimeProperties().ToDictionary(z => z.Name);
            foreach (var item in filter)
            {
                try
                {
                    var contains = GetMemberExpression(parameter, typeof(TSource), item.Property, item.Value);
                    //Expression property = Expression.Property(parameter, item.Property);
                    //Expression value = Expression.Constant(item.Value);
                    //Expression contains = Expression.Call(property, "Contains", null, new Expression[] { value });
                    exp = exp == null ? contains : Expression.Or(exp, contains);
                }
                catch
                {
                }
            }
            if (exp != null)
            {
                Expression<Func<TSource, bool>> pre = Expression.Lambda<Func<TSource, bool>>(exp, parameter);
                return query.Where(pre);
            }
            return query;

        }

        private static Expression GetMemberExpression(Expression left, Type t, string property, string value)
        {
            Expression exp_property = null;
            Expression exp_value = null;
            string baseTypeName = string.Empty;
            string[] split = property.Split('.');
            if (split.Length != 2)
            {
                exp_property = Expression.Property(left, property);
                exp_value = Expression.Constant(value);
                Expression contains = Expression.Call(exp_property, "Contains", null, new Expression[] { exp_value });
                return contains;
            }
            baseTypeName = t.GetProperty(split[0]).PropertyType.BaseType.Name;
            if (baseTypeName == "Enumeration")
            {
                exp_property = Expression.Property(left, split[0] + "Id");

                var p_enumeration = t.GetProperty(split[0]);
                var p = p_enumeration.PropertyType;
                var instance = t.Assembly.CreateInstance(p.FullName);
                var method = p.GetMethod("FromName");
                dynamic obj = method.Invoke(instance, new object[] { value });
                var result = obj.Id;

                exp_value = Expression.Constant(result);

                Expression contains = Expression.Equal(exp_property, exp_value);
                return contains;
            }
            if (baseTypeName == "Entity")
            {
                var p = t.GetProperty(split[0]);
                var p1 = p.PropertyType.GetProperty("Name");
                var exp = Expression.Property(left, p);
                exp_property = Expression.Property(exp, p1);
                exp_value = Expression.Constant(value);

                Expression contains = Expression.Call(exp_property, "Contains", null, new Expression[] { exp_value });
                return contains;
            }
            return null;

        }
    }
}
