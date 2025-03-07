using DataStream.Models;
using System.Linq.Expressions;

namespace DataStream.Expressions
{
    internal static class ExpressionBuilder
    {
        public static Expression<Func<TSource, bool>> Build<TSource>(DataStreamFilter filter)
        {
            var param = Expression.Parameter(typeof(TSource), "x");
            var expr = CombineExpressions(param, filter.PropertyFilters, filter.SearchFilter);
            return expr is null ? x => true : Expression.Lambda<Func<TSource, bool>>(expr, param);
        }

        private static Expression? CombineExpressions(ParameterExpression param,
            IEnumerable<PropertyFilterDS>? propertyFilters,
            SearchFilterDS? searchFilter)
        {
            var propertyExpr = propertyFilters.BuildExpression(param);
            var searchExpr = searchFilter?.ToPropertyFilters().BuildExpression(param);

            return propertyExpr switch
            {
                null => searchExpr,
                _ => searchExpr is null ? propertyExpr : Expression.AndAlso(propertyExpr, searchExpr)
            };
        }

        private static Expression? BuildExpression(this IEnumerable<PropertyFilterDS>? filters, ParameterExpression param)
        {
            if (filters == null || !filters.Any()) return null;

            Expression? result = null;
            foreach (var filter in filters)
            {
                var filterExpr = filter.BuildFilter(param);
                result = result == null ? filterExpr : result.Combine(filterExpr, filter.LogicalOperator);
            }
            return result;
        }

        private static Expression BuildFilter(this PropertyFilterDS filter, ParameterExpression param)
        {
            var prop = ExpressionHelper.PropertyExpression(param, filter.PropertyName);
            OperandValidator.ValidateOperand(filter.OperandType, prop.Type);
            return ExpressionHelper.FilterExpression(prop, filter);
        }

        private static Expression? Combine(this Expression? current, Expression next, LogicalOperatorType op) =>
            current is null ? next : op == LogicalOperatorType.AND ? Expression.AndAlso(current, next) : Expression.OrElse(current, next);

        private static IEnumerable<PropertyFilterDS> ToPropertyFilters(this SearchFilterDS search) =>
            search.Properties.Select(p => new PropertyFilterDS(p, search.OperandType, search.SearchTerm, search.LogicalOperator));
    }
}
