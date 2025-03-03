using DataStream.Exceptions;
using System.Text.Json.Serialization;

namespace DataStream
{
    public record DataStreamResponse<T>
    {
        [JsonPropertyName("pagination")]
        public DataStreamPagination Pagination { get; init; }

        [JsonPropertyName("data")]
        public IEnumerable<T> Data { get; init; }

        public DataStreamResponse(IEnumerable<T> data, DataStreamPagination paginationInfo)
        {
            ArgumentExceptionDS.ThrowIfNull(data);
            ArgumentExceptionDS.ThrowIfNull(paginationInfo);

            Data = data;
            Pagination = paginationInfo;
        }
    }
}
