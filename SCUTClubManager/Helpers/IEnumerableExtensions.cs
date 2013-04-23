using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Reflection;

namespace SCUTClubManager.Helpers
{
    public static class IEnumerableExtensions
    {
        private static MethodInfo orderByMethod = null;
        private static MethodInfo orderByDescendingMethod = null;

        private static LambdaExpression GetPropertyExpr<TSource>(string property_string) where TSource : class
        {
            ParameterExpression param = Expression.Parameter(typeof(TSource));
            Expression expr = param;

            // 处理导航属性。
            while (property_string.Contains("."))
            {
                int dot_pos = property_string.IndexOf(".");
                string property = property_string.Substring(0, dot_pos);

                property_string = property_string.Substring(dot_pos + 1);
                expr = Expression.Property(expr, property);
            }

            expr = Expression.Property(expr, property_string);

            return Expression.Lambda(expr, param);
        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> collection, string order_by) where TSource : class
        {
            try
            {
                if (orderByMethod == null)
                {
                    // Get the ORZ desired generic method.
                    var members = typeof(Enumerable).GetMember("OrderBy");

                    orderByMethod = (from s in members
                                     where s is MethodInfo && (s as MethodInfo).GetParameters().Count() == 2
                                     select s as MethodInfo).Single();
                }

                LambdaExpression lambda_expr = GetPropertyExpr<TSource>(order_by);
                var generic_method = orderByMethod.MakeGenericMethod(typeof(TSource), lambda_expr.Body.Type);
                var compiled_expr = lambda_expr.Compile();
                var result = generic_method.Invoke(null, new object[] { collection, compiled_expr });

                return result as IOrderedEnumerable<TSource>;
            }
            catch
            {
                return collection as IOrderedEnumerable<TSource>;
            }
        }

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource>(this IEnumerable<TSource> collection, string order_by) where TSource : class
        {
            try
            {
                if (orderByDescendingMethod == null)
                {
                    // Get the ORZ desired generic method.
                    var members = typeof(Enumerable).GetMember("OrderByDescending");

                    orderByDescendingMethod = (from s in members
                                               where s is MethodInfo && (s as MethodInfo).GetParameters().Count() == 2
                                               select s as MethodInfo).Single();
                }

                LambdaExpression lambda_expr = GetPropertyExpr<TSource>(order_by);
                var generic_method = orderByDescendingMethod.MakeGenericMethod(typeof(TSource), lambda_expr.Body.Type);
                var compiled_expr = lambda_expr.Compile();
                var result = generic_method.Invoke(null, new object[] { collection, compiled_expr });

                return result as IOrderedEnumerable<TSource>;
            }
            catch
            {
                return collection as IOrderedEnumerable<TSource>;
            }
        }
    }
}