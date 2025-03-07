using DataStream.Expressions;
using DataStream.Models;
using System.Linq.Expressions;

namespace DataStream
{
    public class DataStreamEvaluator<TSource>(IQueryable<TSource> query, DataStreamFilter filter)
    {
        public IQueryable<TSource> Evaluate() => query.Where(BuildExpression()).Sort(filter.SortFilters).Paginate(filter.PaginateFilter);

        public DataStreamResponse<TTarget> Evaluate<TTarget>(Expression<Func<TSource, TTarget>> projection)
        {
            var filteredQuery = query.Where(BuildExpression()).Sort(filter.SortFilters);
            var paginatedData = filteredQuery.Paginate(filter.PaginateFilter).Select(projection);
            return new DataStreamResponse<TTarget>(paginatedData, filteredQuery.GetPaginationInfo(filter.PaginateFilter));
        }

        private Expression<Func<TSource, bool>> BuildExpression() => filter.IsEmpty() ? x => true : ExpressionBuilder.Build<TSource>(filter);
    }
}
