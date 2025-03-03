using DataStream.Exceptions;
using DataStream.Models;

namespace DataStream.Configuration
{
    public class DataStreamOptions
    {
        public DataStreamOptions() { }
        public static DataStreamOptions Default => new();

        public IList<SortFilterDS> SortFilters { get; private set; } = [];
        public DataStreamPaginationOptions PaginationSettings { get; private set; } = DataStreamPaginationOptions.Default;

        /// <summary>
        /// Set the default page number.
        /// </summary>
        public DataStreamOptions SetDefaultPage(int page)
        {
            PaginationSettings.DefaultPage = page;
            return this;
        }

        /// <summary>
        /// Set the default number of rows per page.
        /// </summary>
        public DataStreamOptions SetDefaultPageSize(int pageSize)
        {
            PaginationSettings.DefaultPageSize = pageSize;
            return this;
        }

        /// <summary>
        /// Set the maximum allowed rows per page.
        /// </summary>
        public DataStreamOptions SetPageSizeLimit(int pageSizeLimit)
        {
            PaginationSettings.DefaultPageSizeLimit = pageSizeLimit;
            return this;
        }

        /// <summary>
        /// Add a default sort filter.
        /// </summary>
        public DataStreamOptions SetDefaultOrder(string propertyName, bool ascending)
        {
            SortFilters.Add(new SortFilterDS(propertyName, ascending));
            return this;
        }

        /// <summary>
        /// Set the list of default sort filters.
        /// </summary>
        public DataStreamOptions SetDefaultOrder(IList<SortFilterDS> sortFilters)
        {
            ArgumentExceptionDS.ThrowIfNull(sortFilters);
            SortFilters = sortFilters;
            return this;
        }
    }
}