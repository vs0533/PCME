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
            foreach (var item in filter)
            {
                var contains = GetMemberExpression(parameter, typeof(TSource), item.Operator, item.Property, item.Value);
                exp = exp == null ? contains : Expression.And(exp, contains);
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
            foreach (var item in filter)
            {
                var contains = GetMemberExpression(parameter, typeof(TSource), item.Property, item.Value);
                exp = exp == null ? contains : Expression.Or(exp, contains);
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
            if (baseTypeName == "Entity" || baseTypeName == "Enumeration")
            {
                var p = t.GetProperty(split[0]);
                var p1 = p.PropertyType.GetProperty(split[1]);
                var exp = Expression.Property(left, p);
                exp_property = Expression.Property(exp, p1);
                exp_value = Expression.Constant(value);

                Expression contains = Expression.Call(exp_property, "Contains", null, new Expression[] { exp_value });
                return contains;
            }
            return null;

        }

        private static Expression GetMemberExpression(Expression left, Type t, string Operator, string property, string value)
        {
            Expression exp_property = null;
            Expression exp_value = null;
            string baseTypeName = string.Empty;
            string[] split = property.Split('.');
            if (split.Length != 2)
            {
                exp_property = Expression.Property(left, property);
                Expression contains = null;

                var s = exp_property.Type;
                if (exp_property.Type == typeof(DateTime))
                {
                    GetDateTimeExp(Operator, value, exp_property, ref contains);
                }
                else
                {
                    exp_value = Expression.Constant(value);
                    contains = Expression.Call(exp_property, "Contains", null, new Expression[] { exp_value });
                }
                return contains;
            }
            baseTypeName = t.GetProperty(split[0]).PropertyType.BaseType.Name;
            if (baseTypeName == "Entity" || baseTypeName == "Enumeration")
            {
                var p = t.GetProperty(split[0]);
                var p1 = p.PropertyType.GetProperty(split[1]);
                var exp = Expression.Property(left, p);
                exp_property = Expression.Property(exp, p1);

                Expression contains = null;
                if (p1.PropertyType == typeof(string))
                {
                    exp_value = Expression.Constant(value);
                    contains = Expression.Call(exp_property, "Contains", null, new Expression[] { exp_value });
                }
                if (p1.PropertyType == typeof(DateTime))
                {
                    GetDateTimeExp(Operator, value, exp_property, ref contains);
                }


                return contains;
            }
            return null;

        }

        private static void GetDateTimeExp(string Operator, string value, Expression exp_property, ref Expression contains)
        {
            Expression exp_value;
            var datevalue = Convert.ToDateTime(value);//.ToString("yyyy-MM-dd HH:ss:mm");
            exp_value = Expression.Constant(datevalue);
            switch (Operator)
            {
                case "eq":
                    var start = new DateTime(datevalue.Year, datevalue.Month, datevalue.Day);
                    var end = new DateTime(datevalue.Year, datevalue.Month, datevalue.Day, 23, 59, 59);
                    var exp_value_start = Expression.Constant(start);
                    var exp_value_end = Expression.Constant(end);
                    var exp_gte = Expression.GreaterThanOrEqual(exp_property, exp_value_start);
                    var exp_lte = Expression.LessThanOrEqual(exp_property, exp_value_end);
                    contains = Expression.And(exp_gte, exp_lte);
                    break;
                case "lt":
                    contains = Expression.LessThanOrEqual(exp_property, exp_value);
                    break;
                case "gt":
                    contains = Expression.GreaterThanOrEqual(exp_property, exp_value);
                    break;
                default:
                    break;
            }
        }
    }
}
