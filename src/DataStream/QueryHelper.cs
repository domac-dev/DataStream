using DataStream.Exceptions;
using DataStream.Expressions;
using DataStream.Models;
using System.Linq.Expressions;

namespace DataStream
{
    internal static class QueryHelper
    {
        internal static IQueryable<T> Sort<T>(this IQueryable<T> query, HashSet<SortFilterDS> sortFilters)
        {
            if (sortFilters == null || sortFilters.Count == 0) return query;

            IQueryable<T> sortedQuery = query;
            bool isFirst = true;

            foreach (var filter in sortFilters)
            {
                sortedQuery = sortedQuery.ApplySort(filter.PropertyName, filter.Ascending, isFirst);
                isFirst = false;
            }

            return sortedQuery;
        }

        private static IQueryable<T> ApplySort<T>(this IQueryable<T> query, string propertyName, bool ascending, bool isFirst)
        {
            ArgumentExceptionDS.ThrowIfNullOrEmpty(propertyName);
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = ExpressionHelper.PropertyExpression(parameter, propertyName);

            if (property.Type != typeof(string) && !typeof(IComparable).IsAssignableFrom(property.Type))
                throw new ArgumentExceptionDS($"Property {propertyName} must implement {nameof(IComparable)}");

            var lambda = Expression.Lambda(property, parameter);
            string methodName = isFirst
                ? (ascending ? "OrderBy" : "OrderByDescending")
                : (ascending ? "ThenBy" : "ThenByDescending");

            var result = Expression.Call(
                typeof(Queryable),
                methodName,
                [typeof(T), property.Type],
                query.Expression,
                Expression.Quote(lambda));

            return query.Provider.CreateQuery<T>(result);
        }

        internal static DataStreamPagination GetPaginationInfo<T>(this IQueryable<T> query, PaginationFilterDS? filter)
        {
            filter ??= new();
            int totalCount = query.Count();
            int rows = Math.Max(filter.Rows, 0);
            int page = Math.Max(filter.Page, 1);
            int count = query.Skip((page - 1) * rows).Take(rows).Count();
            return new DataStreamPagination(page, rows, totalCount, count);
        }

        internal static IQueryable<T> Paginate<T>(this IQueryable<T> query, PaginationFilterDS? filter) =>
            query.Skip(((filter?.Page ?? 1) - 1) * (filter?.Rows ?? 0)).Take(filter?.Rows ?? 0);
    }
}
