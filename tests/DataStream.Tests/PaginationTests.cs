using DataStream.Configuration;
using DataStream.Models;
using DataStream.Tests.Data;
using System.Text.Json;

namespace DataStream.Tests
{
    public class PaginationTests : TestBase
    {
        [Fact]
        public void QGrid_Pagination_ShouldReturnCorrectPageSize()
        {
            var json = @"{""pagination"": {""rows"": 5, ""page"": 1}}";
            IQueryable<Employee> query = _context.Employees.AsQueryable();

            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            DataStreamResponse<EmployeeDTO> result = query.Evaluate(filter, e => new EmployeeDTO { Name = e.Name });

            Assert.Equal(5, result.Data.Count());
            Assert.True(result.Pagination.TotalCount == _context.Employees.Count());
        }

        [Fact]
        public void QGrid_Pagination_SecondPage_ShouldReturnCorrectData()
        {
            var json = @"{""pagination"": {""rows"": 10, ""page"": 2}}";
            var query = _context.Employees.AsQueryable();
            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            var result = query.Evaluate(filter, e => new EmployeeDTO { Name = e.Name });

            Assert.Equal(10, result.Data.Count());
            Assert.Equal(35, result.Pagination.TotalCount);
            Assert.Equal(2, result.Pagination.CurrentPage);
            Assert.Equal(4, result.Pagination.TotalPages);

        }

        [Fact]
        public void QGrid_Pagination_LastPage_ShouldReturnRemainingItems()
        {
            var json = @"{""pagination"": {""rows"": 10, ""page"": 4}}";
            var query = _context.Employees.AsQueryable();
            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            var result = query.Evaluate(filter, e => new EmployeeDTO { Name = e.Name });

            Assert.Equal(5, result.Data.Count());
            Assert.Equal(35, result.Pagination.TotalCount);
            Assert.Equal(4, result.Pagination.CurrentPage);
            Assert.Equal(4, result.Pagination.TotalPages);
        }

        [Fact]
        public void QGrid_Pagination_Overfetch_ShouldReturnAllItems()
        {
            var json = @"{""pagination"": {""rows"": 50, ""page"": 1}}";
            var query = _context.Employees.AsQueryable();
            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            var result = query.Evaluate(filter, e => new EmployeeDTO { Name = e.Name });

            Assert.Equal(35, result.Data.Count());
            Assert.Equal(35, result.Pagination.TotalCount);
            Assert.Equal(1, result.Pagination.CurrentPage);
            Assert.Equal(1, result.Pagination.TotalPages);
        }

        [Fact]
        public void QGrid_Pagination_PageOutOfRange_ShouldReturnEmpty()
        {
            var json = @"{""pagination"": {""rows"": 10, ""page"": 5}}";
            var query = _context.Employees.AsQueryable();
            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            var result = query.Evaluate(filter, e => new EmployeeDTO { Name = e.Name });

            Assert.Empty(result.Data);
            Assert.Equal(35, result.Pagination.TotalCount);
            Assert.Equal(5, result.Pagination.CurrentPage);
            Assert.Equal(4, result.Pagination.TotalPages);
        }

        [Fact]
        public void QGrid_Pagination_ZeroRows_ShouldHandleGracefully()
        {
            var json = @"{""pagination"": {""rows"": 0, ""page"": 1}}";
            var query = _context.Employees.AsQueryable();
            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            var result = query.Evaluate(filter, e => new EmployeeDTO { Name = e.Name });

            Assert.Equal(DataStreamPaginationOptions.Default.DefaultPageSize, result.Data.Count());
            Assert.Equal(35, result.Pagination.TotalCount);
            Assert.Equal(1, result.Pagination.CurrentPage);
        }

        [Fact]
        public void QGrid_Pagination_NegativePage_ShouldHandleGracefully()
        {
            var json = @"{""pagination"": {""rows"": 5, ""page"": -1}}";
            var query = _context.Employees.AsQueryable();
            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            var result = query.Evaluate(filter, e => new EmployeeDTO { Name = e.Name });

            Assert.Equal(5, result.Data.Count());
            Assert.Equal(35, result.Pagination.TotalCount);
            Assert.Equal(DataStreamPaginationOptions.Default.DefaultPage, result.Pagination.CurrentPage);
            Assert.Equal(7, result.Pagination.TotalPages);
        }

        [Fact]
        public void QGrid_Pagination_NegativeRows_ShouldHandleGracefully()
        {
            var json = @"{""pagination"": {""rows"": -5, ""page"": 1}}";
            var query = _context.Employees.AsQueryable();
            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            var result = query.Evaluate(filter, e => new EmployeeDTO { Name = e.Name });

            Assert.Equal(DataStreamPaginationOptions.Default.DefaultPageSize, result.Data.Count());
            Assert.Equal(35, result.Pagination.TotalCount);
            Assert.Equal(1, result.Pagination.CurrentPage);
        }

        [Fact]
        public void QGrid_Pagination_ExceedsPageSizeLimit_ShouldCapAtLimit()
        {
            var json = @"{""pagination"": {""rows"": 100, ""page"": 1}}";
            var query = _context.Employees.AsQueryable();
            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            var result = query.Evaluate(filter, e => new EmployeeDTO { Name = e.Name });

            Assert.Equal(35, result.Data.Count()); // returns all elements with maximum of 50 as set by default
            Assert.Equal(35, result.Pagination.TotalCount);
            Assert.Equal(1, result.Pagination.CurrentPage);
            Assert.Equal(1, result.Pagination.TotalPages);
        }

        [Fact]
        public void QGrid_Pagination_EmptyDataSet_ShouldReturnEmptyResult()
        {
            var json = @"{""pagination"": {""rows"": 10, ""page"": 1}}";
            var query = new List<Employee>().AsQueryable();
            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            var result = query.Evaluate(filter, e => new EmployeeDTO { Name = e.Name });

            Assert.Empty(result.Data);
            Assert.Equal(0, result.Pagination.TotalCount);
            Assert.Equal(1, result.Pagination.CurrentPage);
            Assert.Equal(0, result.Pagination.TotalPages);
        }

        [Fact]
        public void QGrid_Pagination_NullPagination_ShouldUseDefaults()
        {
            var json = "{}";
            var query = _context.Employees.AsQueryable();
            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            var result = query.Evaluate(filter, e => new EmployeeDTO { Name = e.Name });

            Assert.Equal(DataStreamPaginationOptions.Default.DefaultPageSize, result.Data.Count());
            Assert.Equal(35, result.Pagination.TotalCount);
            Assert.Equal(DataStreamPaginationOptions.Default.DefaultPage, result.Pagination.CurrentPage);
            Assert.Equal(4, result.Pagination.TotalPages);
        }

        [Fact]
        public void QGrid_Pagination_ItemIndexes_ShouldReturnCorrectData()
        {
            var json = @"{""pagination"": {""rows"": 10, ""page"": 2}}";
            var query = _context.Employees.AsQueryable();
            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            var result = query.Evaluate(filter, e => new EmployeeDTO { Name = e.Name });

            Assert.Equal(11, result.Pagination.FirstItemIndex);
            Assert.Equal(20, result.Pagination.LastItemIndex);
        }

        [Fact]
        public void QGrid_Pagination_ItemIndexes_Marginal_ShouldReturnCorrectData()
        {
            var json = @"{""pagination"": {""rows"": 10, ""page"": 4}}";
            var query = _context.Employees.AsQueryable();
            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            var result = query.Evaluate(filter, e => new EmployeeDTO { Name = e.Name });

            Assert.Equal(31, result.Pagination.FirstItemIndex);
            Assert.Equal(35, result.Pagination.LastItemIndex);
        }
    }
}
