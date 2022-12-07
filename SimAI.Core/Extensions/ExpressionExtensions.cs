using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SimAI.Core.Extensions {
    public static class ExpressionExtensions {

        internal class ExpressionSubstitute : ExpressionVisitor {
            public readonly Expression From, To;

            public ExpressionSubstitute(Expression from, Expression to) {
                From = from;
                To = to;
            }

            public override Expression Visit(Expression node) {
                if (node == From) return To;
                return base.Visit(node);
            }
        }

        public static void Swap() {
            //var swap = new ExpressionSubstitute(e1.Parameters[1], Expression.Constant("Fixed Value Here"));
            //var lambda = Expression.Lambda<Func<string, bool>>(swap.Visit(e1.Body), e1.Parameters[0]);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName) {

            var entityType = typeof(T);
            var propertyInfo = entityType.GetProperty("Item");
            var arg = Expression.Parameter(entityType, "x");
            //var property = Expression.Property(arg, propertyName);
            var property = Expression.MakeIndex(arg, propertyInfo, new [] { Expression.Constant(propertyName) });
            var selector = Expression.Lambda(property, arg);
            var enumarableType = typeof(Queryable);
            var method = enumarableType.GetMethods()
                .Where(m => m.Name == "OrderBy" && m.IsGenericMethodDefinition)
                .Where(m => {
                    var parameters = m.GetParameters().ToList();        
                    return parameters.Count == 2;
                }).Single();

            MethodInfo genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);
            
            return (IOrderedQueryable<T>)genericMethod.Invoke(genericMethod, new object[] { query, selector });
        }

        /*public static MemberExpression Access<T>(this ParameterExpression parameter, Expression<MulticastDelegate> expression) {
            return Expression.MakeMemberAccess(parameter, GetMember(expression));
        }*/

        public static MemberInfo GetMember<T>(this Expression<T> expression) {
            var lambda = expression as LambdaExpression;
            var debuggableType = lambda.Body.GetType();

            if (lambda.Body is MethodCallExpression methodCall)
                return methodCall.Method;

            if (lambda.Body is UnaryExpression unary) {
                switch (unary.Operand) {
                    //case ConstantExpression constant:
                    //    return unary.Method;
                    case MemberExpression member:
                        return member.Member;
                    case MethodCallExpression method:
                        return method.Method;
                }
            }

            var property = lambda.Body as MemberExpression;
            return property.Member;
        }
    }
}