using DataStream.Expressions;
using DataStream.Models;
using System.Linq.Expressions;

namespace DataStream
{
    /// <summary>
    /// Provides extension methods to evaluate queries using a DataStreamFilter, enabling filtering, sorting, and pagination on IQueryable data sources.
    /// </summary>
    public static class DataStreamExtensions
    {
        /// <summary>
        /// Evaluates a query with the specified filter and projects the results into a target type, returning a paginated response.
        /// </summary>
        /// <typeparam name="TTarget">The type of the projected result.</typeparam>
        /// <typeparam name="TSource">The type of the source data.</typeparam>
        /// <param name="query">The IQueryable data source to evaluate.</param>
        /// <param name="filter">The DataStreamFilter containing filtering, sorting, and pagination settings.</param>
        /// <param name="expression">The projection expression to transform source data into the target type.</param>
        /// <returns>A DataStreamResponse containing the paginated, projected results and pagination metadata.</returns>
        public static DataStreamResponse<TTarget> Evaluate<TTarget, TSource>(this IQueryable<TSource> query, DataStreamFilter filter,
            Expression<Func<TSource, TTarget>> expression)
        {
            DataStreamEvaluator<TSource> builder = new(query, filter);
            return builder.Evaluate(expression);
        }

        /// <summary>
        /// Evaluates a query with the specified filter, applying filtering, sorting, and pagination, and returns the resulting IQueryable.
        /// </summary>
        /// <typeparam name="TSource">The type of the source data.</typeparam>
        /// <param name="query">The IQueryable data source to evaluate.</param>
        /// <param name="filter">The DataStreamFilter containing filtering, sorting, and pagination settings.</param>
        /// <returns>An IQueryable representing the filtered, sorted, and paginated data.</returns>
        public static IQueryable<TSource> Evaluate<TSource>(this IQueryable<TSource> query, DataStreamFilter filter)
        {
            DataStreamEvaluator<TSource> builder = new(query, filter);
            return builder.Evaluate();
        }
    }
}