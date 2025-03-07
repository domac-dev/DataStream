using DataStream.Configuration;
using System.Text.Json.Serialization;

namespace DataStream.Models
{
    /// <summary>
    /// Defines pagination settings for a data query, controlling the page number and number of rows per page.
    /// Ensures pagination values stay within configured limits and defaults.
    /// </summary>
    public class PaginationFilterDS
    {
        /// <summary>
        /// Internal reference to global DataStream options, used to enforce pagination defaults and limits.
        /// </summary>
        private readonly DataStreamOptions _options;

        /// <summary>
        /// Backing field for the Rows property, storing the number of rows per page.
        /// </summary>
        private int _rows;

        /// <summary>
        /// Backing field for the Page property, storing the current page number.
        /// </summary>
        private int _page;

        /// <summary>
        /// Gets or sets the number of rows per page. Values are constrained by the DataStream options' DefaultPageSize and DefaultPageSizeLimit.
        /// </summary>
        [JsonPropertyName("rows")]
        public int Rows
        {
            get => _rows;
            set
            {
                if (value <= 0)
                    _rows = _options.PaginationSettings.DefaultPageSize;
                else if (value > _options.PaginationSettings.DefaultPageSizeLimit)
                    _rows = _options.PaginationSettings.DefaultPageSizeLimit;
                else
                    _rows = value;
            }
        }

        /// <summary>
        /// Gets or sets the current page number. Must be at least 1, defaults to the DataStream options' DefaultPage if less.
        /// </summary>
        [JsonPropertyName("page")]
        public int Page
        {
            get => _page;
            set => _page = value < 1 ? _options.PaginationSettings.DefaultPage : value;
        }

        /// <summary>
        /// Initializes a new instance of PaginationFilterDS with default options.
        /// </summary>
        public PaginationFilterDS()
        {
            _options = DataStreamOptions.Default;
            Page = _options.PaginationSettings.DefaultPage;
            Rows = _options.PaginationSettings.DefaultPageSize;
        }

        /// <summary>
        /// Initializes a new instance of PaginationFilterDS with custom options.
        /// </summary>
        /// <param name="options">Custom DataStream options. If null, defaults to DataStreamOptions.Default.</param>
        public PaginationFilterDS(DataStreamOptions? options = null)
        {
            _options = options ?? DataStreamOptions.Default;
            Page = _options.PaginationSettings.DefaultPage;
            Rows = _options.PaginationSettings.DefaultPageSize;
        }

        /// <summary>
        /// Initializes a new instance of PaginationFilterDS with specified page, rows, and options.
        /// </summary>
        /// <param name="page">The current page number.</param>
        /// <param name="rows">The number of rows per page.</param>
        /// <param name="options">Custom DataStream options. If null, defaults to DataStreamOptions.Default.</param>
        public PaginationFilterDS(int page, int rows, DataStreamOptions? options = null)
        {
            _options = options ?? DataStreamOptions.Default;
            Page = page < 1 ? _options.PaginationSettings.DefaultPage : page;

            if (rows <= 0)
                Rows = _options.PaginationSettings.DefaultPageSize;

            else if (rows > _options.PaginationSettings.DefaultPageSizeLimit)
                Rows = _options.PaginationSettings.DefaultPageSizeLimit;

            else
                Rows = rows;
        }
    }
}