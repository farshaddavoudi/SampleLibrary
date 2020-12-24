using System;
using System.Linq;
using System.Linq.Expressions;

namespace ATA.Library.Shared.Service.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> AddWhere<T>(this IQueryable<T> query, Expression<Func<T, bool>>? where)
        {
            return where != null ? query.Where(where) : query;
        }

        public static bool IsTrue(this bool? clause)
        {
            return clause != null && clause == true;
        }

        public static Expression<Func<T, bool>> And<T>(
                            this Expression<Func<T, bool>> expr1,
                            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left, right), parameter);
        }

        public static Expression<Func<T, bool>> Or<T>(
               this Expression<Func<T, bool>> expr1,
                Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.Or(left, right), parameter);
        }

        private class ReplaceExpressionVisitor
: ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node)!;
            }
        }
    }
}