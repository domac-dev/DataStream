using DataStream.Configuration;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace DataStream.Models
{
    public class PaginationFilterDS
    {
        private readonly DataStreamOptions _options;
        private int _rows;
        private int _page;

        /// <summary>
        /// Gets or sets the number of rows per page. The value is limited by the configuration options.
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
        /// Gets or sets the current page number. The value must be at least 1.
        /// </summary>
        [JsonPropertyName("page")]
        public int Page
        {
            get => _page;
            set => _page = value < 1 ? _options.PaginationSettings.DefaultPage : value;
        }

        public PaginationFilterDS()
        {
            _options = DataStreamOptions.Default;
            Page = _options.PaginationSettings.DefaultPage;
            Rows = _options.PaginationSettings.DefaultPageSize;
        }

        public PaginationFilterDS(DataStreamOptions? options = null)
        {
            _options = options ?? DataStreamOptions.Default;
            Page = _options.PaginationSettings.DefaultPage;
            Rows = _options.PaginationSettings.DefaultPageSize;
        }

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