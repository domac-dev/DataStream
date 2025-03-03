using DataStream.Models;
using System.Linq.Expressions;

namespace DataStream
{
    public class DataStreamEvaluator<TSource>(IQueryable<TSource> query, DataStreamFilter filter)
    {
        private readonly DataStreamFilter _filter = filter;
        private readonly IQueryable<TSource> _query = query;

        public IQueryable<TSource> Evaluate()
        {
            Expression<Func<TSource, bool>> expression = BuildExpression(_filter);

            return _query.Where(expression)
                .Sort([.. _filter.SortFilters])
                .Paginate(_filter.PaginateFilter);
        }

        public DataStreamResponse<TTarget> Evaluate<TTarget>(Expression<Func<TSource, TTarget>> expression)
        {
            IEnumerable<TTarget> projection = Evaluate().Select(expression);
            return new DataStreamResponse<TTarget>(projection, _query.GetPaginationInfo(_filter.PaginateFilter!));
        }

        private static Expression<Func<TSource, bool>> BuildExpression(DataStreamFilter filter)
        {
            if (filter.PropertyFilters is null && filter.SearchFilter is null)
                return entity => true;

            ParameterExpression parameter = Expression.Parameter(typeof(TSource), "x");
            Expression? finalExpression = null;

            Expression? BuildFilterExpression(IEnumerable<PropertyFilterDS>? filters)
            {
                if (filters is null || !filters.Any()) return null;

                Expression? combinedExpression = null;

                foreach (var filter in filters)
                {
                    Type propertyExpression = ExpressionHelper.PropertyExpression(parameter, filter.PropertyName).Type;
                    OperandValidator.ValidateOperand(filter.OperandType, propertyExpression);

                    Expression filterExpression = ExpressionHelper.FilterExpression<TSource>(parameter, filter);

                    if (combinedExpression == null)
                        combinedExpression = filterExpression;
                    else
                        combinedExpression = ExpressionHelper.OperatorExpression(filter.LogicalOperator, combinedExpression, filterExpression);
                }

                return combinedExpression;
            }

            finalExpression = BuildFilterExpression(filter.PropertyFilters);

            if (filter.SearchFilter is not null)
            {
                List<PropertyFilterDS> searchFilters = filter.SearchFilter.Properties.Select(x => new PropertyFilterDS(x,
                    filter.SearchFilter.OperandType,
                    filter.SearchFilter.SearchTerm,
                    filter.SearchFilter.LogicalOperator)).ToList();

                Expression searchExpression = BuildFilterExpression(searchFilters)!;

                if (finalExpression == null)
                    finalExpression = searchExpression;
                else
                    finalExpression = ExpressionHelper.OperatorExpression(LogicalOperatorType.AND, finalExpression, searchExpression);
            }

            return finalExpression == null
                ? entity => true
                : Expression.Lambda<Func<TSource, bool>>(finalExpression, parameter);
        }
    }
}
