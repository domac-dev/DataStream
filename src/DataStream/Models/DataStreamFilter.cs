using DataStream.Configuration;
using System.Text.Json.Serialization;

namespace DataStream.Models
{
    public class DataStreamFilter
    {
        private readonly DataStreamOptions _options;

        [JsonPropertyName("pagination")]
        public PaginationFilterDS PaginateFilter { get; set; }

        [JsonPropertyName("search")]
        public SearchFilterDS? SearchFilter { get; set; }

        [JsonPropertyName("filters")]
        public HashSet<PropertyFilterDS> PropertyFilters { get; set; }

        [JsonPropertyName("sort")]
        public HashSet<SortFilterDS> SortFilters { get; set; }

        public DataStreamFilter()
        {
            _options = DataStreamOptions.Default;
            PaginateFilter = new PaginationFilterDS();
            PropertyFilters = [];
            SortFilters = [];
            Init();
        }

        public DataStreamFilter(DataStreamOptions? options = null)
        {
            _options = options ?? DataStreamOptions.Default;
            PaginateFilter = new PaginationFilterDS();
            PropertyFilters = [];
            SortFilters = [];
            Init();
        }

        private void Init()
        {
            foreach (var filter in _options.SortFilters)
            {
                SortFilters.Add(filter);
            }
        }
    }
}