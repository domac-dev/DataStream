using DataStream.Models;
using DataStream.Tests.Data;
using System.Text.Json;

namespace DataStream.Tests
{
    public class FilteringTests : TestBase
    {
        [Fact]
        public void QGrid_Filter_DateTime_GreaterThan_ShouldReturnCorrectResults()
        {
            IQueryable<Employee> query = _context.Employees.AsQueryable();
            DateTime tenYearsAgo = DateTime.UtcNow.AddYears(-10);

            string json = @"{
                ""filters"": [
                    { ""property"": ""JoinDate"", ""value"": ""03.03.2015"", ""operand"": ""gte"", ""operator"": ""and"" }
                ]
            }";

            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            DataStreamResponse<EmployeeDTO> result = query.Evaluate(filter, e => new EmployeeDTO { JoinDate = e.JoinDate });

            Assert.NotNull(result.Data);
            Assert.NotEmpty(result.Data);
            Assert.All(result.Data, employee =>
                Assert.True(employee.JoinDate > tenYearsAgo,
                    $"Employee with JoinDate {employee.JoinDate} should be greater than {tenYearsAgo}."));
        }

        [Fact]
        public void QGrid_Filter_DateTime_LessThan_ShouldReturnCorrectResults()
        {
            IQueryable<Employee> query = _context.Employees.AsQueryable();
            DateTime utcNow = DateTime.UtcNow;

            string json = @"{
                ""filters"": [
                    { ""property"": ""JoinDate"", ""value"": """ + utcNow.ToString("O") + @""", ""operand"": ""lt"", ""operator"": ""and"" }
                ]
            }";

            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            DataStreamResponse<EmployeeDTO> result = query.Evaluate(filter, e => new EmployeeDTO { JoinDate = e.JoinDate });

            Assert.NotNull(result.Data);
            Assert.NotEmpty(result.Data);
            Assert.All(result.Data, employee =>
                Assert.True(employee.JoinDate < utcNow,
                    $"Employee with JoinDate {employee.JoinDate} should be less than {utcNow}."));
        }

        [Fact]
        public void QGrid_Filter_String_Equals_CompanyName_ShouldReturnCorrectResults()
        {
            IQueryable<Employee> query = _context.Employees.AsQueryable();
            string companyName = "TechCorp";

            string json = @"{
                ""filters"": [
                    { ""property"": ""Company.Name"", ""value"": """ + companyName + @""", ""operand"": ""eq"", ""operator"": ""and"" }
                ]
            }";

            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            DataStreamResponse<EmployeeDTO> result = query.Evaluate(filter, e => new EmployeeDTO { CompanyName = e.Company.Name });

            Assert.NotNull(result.Data);
            Assert.NotEmpty(result.Data);
            Assert.All(result.Data, employee =>
                Assert.True(employee.CompanyName == companyName));
        }

        [Fact]
        public void QGrid_MultipleFilters_DateTimeAndInt_ShouldReturnCorrectResults()
        {
            IQueryable<Employee> query = _context.Employees.AsQueryable();
            DateTime tenYearsAgo = DateTime.UtcNow.AddYears(-10);

            string json = @"{
                ""filters"": [
                    { ""property"": ""JoinDate"", ""value"": """ + tenYearsAgo.ToString("O") + @""", ""operand"": ""gt"", ""operator"": ""and"" },
                    { ""property"": ""Age"", ""value"": ""30"", ""operand"": ""lte"", ""operator"": ""and"" }
                ]
            }";

            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            DataStreamResponse<EmployeeDTO> result = query.Evaluate(filter, e => new EmployeeDTO { JoinDate = e.JoinDate, Age = e.Age });

            Assert.NotNull(result.Data);
            Assert.NotEmpty(result.Data);
            Assert.All(result.Data, employee =>
            {
                Assert.True(employee.JoinDate > tenYearsAgo,
                    $"Employee with JoinDate {employee.JoinDate} should be greater than {tenYearsAgo}.");
                Assert.True(employee.Age <= 30,
                    $"Employee with Age {employee.Age} should be less than or equal to 30.");
            });
        }

        [Fact]
        public void QGrid_Filter_String_Contains_Name_ShouldReturnCorrectResults()
        {
            IQueryable<Employee> query = _context.Employees.AsQueryable();
            string namePart = "John";

            string json = @"{
                ""filters"": [
                    { ""property"": ""Name"", ""value"": """ + namePart + @""", ""operand"": ""cn"", ""operator"": ""and"" }
                ]
            }";

            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            DataStreamResponse<EmployeeDTO> result = query.Evaluate(filter, e => new EmployeeDTO { Name = e.Name });

            Assert.NotNull(result.Data);
            Assert.NotEmpty(result.Data);
            Assert.All(result.Data, employee =>
                Assert.Contains(namePart, employee.Name, StringComparison.OrdinalIgnoreCase));
        }
    }
}