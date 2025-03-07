using DataStream.Expressions;
using DataStream.Models;
using System.Linq.Expressions;

namespace DataStream
{
    /// <summary>
    /// Evaluates a data query by applying filters, sorting, and pagination defined in a DataStreamFilter to an IQueryable source.
    /// Provides methods to either return the raw queryable result or a paginated response with projected data.
    /// </summary>
    public class DataStreamEvaluator<TSource>(IQueryable<TSource> query, DataStreamFilter filter)
    {
        private readonly IQueryable<TSource> _query = query;
        private readonly DataStreamFilter _filter = filter;

        /// <summary>
        /// Evaluates the query by applying filters, sorting, and pagination, returning the resulting IQueryable.
        /// </summary>
        /// <returns>An IQueryable with all query operations applied.</returns>
        public IQueryable<TSource> Evaluate() => _query.Where(BuildExpression()).Sort(_filter.SortFilters).Paginate(_filter.PaginateFilter);

        /// <summary>
        /// Evaluates the query with a projection, applying filters, sorting, and pagination, and returns a paginated response.
        /// </summary>
        /// <typeparam name="TTarget">The type of the projected result.</typeparam>
        /// <param name="projection">The expression to project source data into the target type.</param>
        /// <returns>A DataStreamResponse containing the paginated, projected results and pagination metadata.</returns>
        public DataStreamResponse<TTarget> Evaluate<TTarget>(Expression<Func<TSource, TTarget>> projection)
        {
            IQueryable<TSource> filteredQuery = _query.Where(BuildExpression()).Sort(_filter.SortFilters);
            IQueryable<TTarget> paginatedData = filteredQuery.Paginate(_filter.PaginateFilter).Select(projection);
            return new DataStreamResponse<TTarget>(paginatedData, filteredQuery.GetPaginationInfo(_filter.PaginateFilter));
        }

        /// <summary>
        /// Builds the filter expression based on the DataStreamFilter settings.
        /// </summary>
        /// <returns>An expression representing the filter conditions, or a true expression if the filter is empty.</returns>
        private Expression<Func<TSource, bool>> BuildExpression() => _filter.IsEmpty() ? x => true : ExpressionBuilder.Build<TSource>(_filter);
    }
}