using DataStream.Exceptions;
using System.Text.Json.Serialization;

namespace DataStream
{
    public record DataStreamPagination
    {
        [JsonPropertyName("currentPage")]
        public int CurrentPage { get; init; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; init; }

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; init; }

        [JsonPropertyName("count")]
        public int Count { get; init; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; init; }

        [JsonPropertyName("hasNext")]
        public bool HasNext => CurrentPage < TotalPages;

        [JsonPropertyName("hasPrevious")]
        public bool HasPrevious => CurrentPage > 1;

        [JsonPropertyName("firstItemIndex")]
        public int FirstItemIndex => TotalCount == 0 ? 0 : (CurrentPage - 1) * PageSize + 1;

        [JsonPropertyName("lastItemIndex")]
        public int LastItemIndex => TotalCount == 0 ? 0 : Math.Min(CurrentPage * PageSize, TotalCount);

        [JsonPropertyName("isFirstPage")]
        public bool IsFirstPage => CurrentPage == 1;

        [JsonPropertyName("isLastPage")]
        public bool IsLastPage => CurrentPage >= TotalPages;

        [JsonPropertyName("hasData")]
        public bool HasData => Count > 0;

        public DataStreamPagination(int currentPage, int pageSize, int totalCount, int count)
        {
            CurrentPage = currentPage < 1 ? 1 : currentPage;

            if (pageSize < 0)
                throw new ArgumentExceptionDS("Page size cannot be negative.");

            PageSize = pageSize;

            if (totalCount < 0)
                throw new ArgumentExceptionDS("Total count cannot be negative.");

            TotalCount = totalCount;

            if (count < 0)
                throw new ArgumentExceptionDS("Count cannot be negative.");

            Count = count;
            TotalPages = totalCount == 0 || pageSize <= 0 ? 0 : (int)Math.Ceiling((double)totalCount / pageSize);
        }
    }
}