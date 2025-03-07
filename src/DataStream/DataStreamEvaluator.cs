using DataStream.Expressions;
using DataStream.Models;
using System.Linq.Expressions;

namespace DataStream
{
    public class DataStreamEvaluator<TSource>(IQueryable<TSource> query, DataStreamFilter filter)
    {
        public IQueryable<TSource> Evaluate() => query.Where(BuildExpression()).Sort(filter.SortFilters).Paginate(filter.PaginateFilter);

        public DataStreamResponse<TTarget> Evaluate<TTarget>(Expression<Func<TSource, TTarget>> projection) =>
            new(Evaluate().Select(projection), query.GetPaginationInfo(filter.PaginateFilter));

        private Expression<Func<TSource, bool>> BuildExpression() => filter.IsEmpty() ? x => true : ExpressionBuilder.Build<TSource>(filter);
    }
}
