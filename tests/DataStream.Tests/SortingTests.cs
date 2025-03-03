using DataStream.Models;
using DataStream.Tests.Data;
using System.Text.Json;

namespace DataStream.Tests
{
    public class SortingTests : TestBase
    {
        [Fact]
        public void QGrid_Sorting_String_Ascending_ShouldReturnCorrectResults()
        {
            IQueryable<Employee> query = _context.Employees.AsQueryable();

            string json = @"{
                ""sort"": [
                    { ""property"": ""Name"", ""ascending"": true }
                ]
            }";

            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            DataStreamResponse<EmployeeDTO> result = query.Evaluate(filter, e => new EmployeeDTO { Name = e.Name });

            Assert.NotEmpty(result.Data);
            Assert.True(result.Data.Select(e => e.Name)
                .Zip(result.Data.Select(e => e.Name).Skip(1), (a, b) => string.Compare(a ?? "", b ?? "") <= 0)
                .All(x => x),
                "The results should be sorted in ascending order by Name.");
        }

        [Fact]
        public void QGrid_Sorting_String_Descending_ShouldReturnCorrectResults()
        {
            IQueryable<Employee> query = _context.Employees.AsQueryable();

            string json = @"{
                ""sort"": [
                    { ""property"": ""Name"", ""ascending"": false }
                ]
            }";

            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            DataStreamResponse<EmployeeDTO> result = query.Evaluate(filter, e => new EmployeeDTO { Name = e.Name });

            Assert.NotEmpty(result.Data);
            Assert.True(result.Data.Select(e => e.Name)
                .Zip(result.Data.Select(e => e.Name).Skip(1), (a, b) => string.Compare(a ?? "", b ?? "") >= 0)
                .All(x => x),
                "The results should be sorted in descending order by Name.");
        }

        [Fact]
        public void QGrid_Sorting_Int_Ascending_ShouldReturnCorrectResults()
        {
            IQueryable<Employee> query = _context.Employees.AsQueryable();

            string json = @"{
                ""sort"": [
                    { ""property"": ""Age"", ""ascending"": true }
                ]
            }";

            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            DataStreamResponse<EmployeeDTO> result = query.Evaluate(filter, e => new EmployeeDTO { Age = e.Age });

            Assert.NotEmpty(result.Data);
            Assert.True(result.Data.Select(e => e.Age)
                .Zip(result.Data.Select(e => e.Age).Skip(1), (a, b) => a <= b)
                .All(x => x),
                "The results should be sorted in ascending order by Age.");
        }

        [Fact]
        public void QGrid_Sorting_Int_Descending_ShouldReturnCorrectResults()
        {
            IQueryable<Employee> query = _context.Employees.AsQueryable();

            string json = @"{
                ""sort"": [
                    { ""property"": ""Age"", ""ascending"": false }
                ]
            }";

            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            DataStreamResponse<EmployeeDTO> result = query.Evaluate(filter, e => new EmployeeDTO { Age = e.Age });

            Assert.NotEmpty(result.Data);
            Assert.True(result.Data.Select(e => e.Age)
                .Zip(result.Data.Select(e => e.Age).Skip(1), (a, b) => a >= b)
                .All(x => x),
                "The results should be sorted in descending order by Age.");
        }

        [Fact]
        public void QGrid_Sorting_Double_Ascending_ShouldReturnCorrectResults()
        {
            IQueryable<Employee> query = _context.Employees.AsQueryable();

            string json = @"{
                ""sort"": [
                    { ""property"": ""Salary"", ""ascending"": true }
                ]
            }";

            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            DataStreamResponse<EmployeeDTO> result = query.Evaluate(filter, e => new EmployeeDTO { Salary = e.Salary });

            Assert.NotEmpty(result.Data);
            Assert.True(result.Data.Select(e => e.Salary)
                .Zip(result.Data.Select(e => e.Salary).Skip(1), (a, b) => a <= b)
                .All(x => x),
                "The results should be sorted in ascending order by Salary.");
        }

        [Fact]
        public void QGrid_Sorting_Double_Descending_ShouldReturnCorrectResults()
        {
            IQueryable<Employee> query = _context.Employees.AsQueryable();

            string json = @"{
                ""sort"": [
                    { ""property"": ""Salary"", ""ascending"": false }
                ]
            }";

            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            DataStreamResponse<EmployeeDTO> result = query.Evaluate(filter, e => new EmployeeDTO { Salary = e.Salary });

            Assert.NotEmpty(result.Data);
            Assert.True(result.Data.Select(e => e.Salary)
                .Zip(result.Data.Select(e => e.Salary).Skip(1), (a, b) => a >= b)
                .All(x => x),
                "The results should be sorted in descending order by Salary.");
        }

        [Fact]
        public void QGrid_Sorting_DateTime_Ascending_ShouldReturnCorrectResults()
        {
            IQueryable<Employee> query = _context.Employees.AsQueryable();

            string json = @"{
                ""sort"": [
                    { ""property"": ""JoinDate"", ""ascending"": true }
                ]
            }";

            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            DataStreamResponse<EmployeeDTO> result = query.Evaluate(filter, e => new EmployeeDTO { JoinDate = e.JoinDate });

            Assert.NotEmpty(result.Data);
            Assert.True(result.Data.Select(e => e.JoinDate)
                .Zip(result.Data.Select(e => e.JoinDate).Skip(1), (a, b) => a <= b)
                .All(x => x),
                "The results should be sorted in ascending order by JoinDate.");
        }

        [Fact]
        public void QGrid_Sorting_DateTime_Descending_ShouldReturnCorrectResults()
        {
            IQueryable<Employee> query = _context.Employees.AsQueryable();

            string json = @"{
                ""sort"": [
                    { ""property"": ""JoinDate"", ""ascending"": false }
                ]
            }";

            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            DataStreamResponse<EmployeeDTO> result = query.Evaluate(filter, e => new EmployeeDTO { JoinDate = e.JoinDate });

            Assert.NotEmpty(result.Data);
            Assert.True(result.Data.Select(e => e.JoinDate)
                .Zip(result.Data.Select(e => e.JoinDate).Skip(1), (a, b) => a >= b)
                .All(x => x),
                "The results should be sorted in descending order by JoinDate.");
        }
    }
}