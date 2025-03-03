using DataStream.Exceptions;
using DataStream.Models;
using System.Linq.Expressions;

namespace DataStream
{
    internal static class QueryHelper
    {
        internal static IQueryable<T> Sort<T>(this IQueryable<T> query, List<SortFilterDS> sortFilters)
        {
            return sortFilters.Aggregate(query, (currentQuery, filter) => currentQuery.Sort(filter.PropertyName, filter.Ascending));
        }

        internal static IQueryable<T> Sort<T>(this IQueryable<T> query, string propertyName, bool ascending)
        {
            ArgumentExceptionDS.ThrowIfNullOrEmpty(propertyName);
            ParameterExpression? parameter = Expression.Parameter(typeof(T), "x");
            Expression? property = ExpressionHelper.PropertyExpression(parameter, propertyName);

            if (property.Type != typeof(string) && !typeof(IComparable).IsAssignableFrom(property.Type))
                throw new ArgumentExceptionDS($"Property {propertyName} must implement {nameof(IComparable)}");

            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = ascending ? "OrderBy" : "OrderByDescending";
            MethodCallExpression? result = Expression.Call(
                typeof(Queryable),
                methodName,
                [typeof(T), property.Type],
                query.Expression,
                Expression.Quote(lambda));

            return query.Provider.CreateQuery<T>(result);
        }

        internal static DataStreamPagination GetPaginationInfo<T>(this IQueryable<T> query, PaginationFilterDS filter)
        {
            filter ??= new PaginationFilterDS();

            int totalCount = query.Count();
            int rows = filter!.Rows;
            int currentPage = filter!.Page;
            int count = query.Skip((currentPage - 1) * rows).Take(rows).Count();

            return new DataStreamPagination(currentPage, rows, totalCount, count);
        }

        internal static IQueryable<T> Paginate<T>(this IQueryable<T> query, PaginationFilterDS filter)
        {
            int page = filter.Page < 1 ? 1 : filter.Page;
            int rows = filter.Rows < 1 ? 0 : filter.Rows;
            int skip = (page - 1) * rows;
            query = query.Skip(skip).Take(rows);

            return query;
        }
    }
}
