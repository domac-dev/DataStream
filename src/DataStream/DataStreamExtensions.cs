using DataStream.Models;
using System.Linq.Expressions;

namespace DataStream
{
    public static class DataStreamExtensions
    {
        public static DataStreamResponse<TTarget> Evaluate<TTarget, TSource>(this IQueryable<TSource> query, DataStreamFilter filter,
            Expression<Func<TSource, TTarget>> expression)
        {
            DataStreamEvaluator<TSource> builder = new(query, filter);
            return builder.Evaluate(expression);
        }

        public static IQueryable<TSource> Evaluate<TSource>(this IQueryable<TSource> query, DataStreamFilter filter)
        {
            DataStreamEvaluator<TSource> builder = new(query, filter);
            return builder.Evaluate();
        }
    }
}
