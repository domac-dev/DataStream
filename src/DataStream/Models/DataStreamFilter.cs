using DataStream.Configuration;
using System.Text.Json.Serialization;

namespace DataStream.Models
{
    /// <summary>
    /// Represents the main configuration object for filtering, searching, sorting, and paginating data in the DataStream library.
    /// This class aggregates all query parameters deserialized from JSON requests, providing a unified structure for data queries.
    /// </summary>
    public class DataStreamFilter
    {
        /// <summary>
        /// Internal reference to global DataStream options, used to apply default settings for pagination and sorting.
        /// </summary>
        private readonly DataStreamOptions _options;

        /// <summary>
        /// Gets or sets the pagination settings for the query, defining page size and current page.
        /// </summary>
        [JsonPropertyName("pagination")]
        public PaginationFilterDS PaginateFilter { get; set; }

        /// <summary>
        /// Gets or sets the search filter configuration, allowing searches across multiple properties. Can be null if no search is specified.
        /// </summary>
        [JsonPropertyName("search")]
        public SearchFilterDS? SearchFilter { get; set; }

        /// <summary>
        /// Gets or sets a collection of property-specific filters (e.g., "Salary > 2500") applied to the query.
        /// </summary>
        [JsonPropertyName("filters")]
        public HashSet<PropertyFilterDS> PropertyFilters { get; set; }

        /// <summary>
        /// Gets or sets a collection of sorting rules (e.g., "Name ascending") applied to the query results.
        /// </summary>
        [JsonPropertyName("sort")]
        public HashSet<SortFilterDS> SortFilters { get; set; }

        /// <summary>
        /// Initializes a new instance of the DataStreamFilter class with default options.
        /// </summary>
        public DataStreamFilter()
        {
            _options = DataStreamOptions.Default;
            PaginateFilter = new PaginationFilterDS();
            PropertyFilters = [];
            SortFilters = [];
            Init();
        }

        /// <summary>
        /// Initializes a new instance of the DataStreamFilter class with custom options.
        /// </summary>
        /// <param name="options">Custom DataStream options. If null, defaults to DataStreamOptions.Default.</param>
        public DataStreamFilter(DataStreamOptions? options = null)
        {
            _options = options ?? DataStreamOptions.Default;
            PaginateFilter = new PaginationFilterDS();
            PropertyFilters = [];
            SortFilters = [];
            Init();
        }

        /// <summary>
        /// Determines if the filter is empty (no property filters and no search filter).
        /// </summary>
        /// <returns>True if the filter is empty; otherwise, false.</returns>
        public bool IsEmpty() => PropertyFilters.Count != 0 != true && SearchFilter is null;

        /// <summary>
        /// Initializes the SortFilters collection with default sorting rules from the DataStream options.
        /// </summary>
        private void Init()
        {
            foreach (var filter in _options.SortFilters)
            {
                SortFilters.Add(filter);
            }
        }
    }
}