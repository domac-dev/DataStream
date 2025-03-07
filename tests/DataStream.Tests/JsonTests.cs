using DataStream.Models;
using DataStream.Tests.Data;
using System.Text.Json;
namespace DataStream.Tests
{
    public class JsonTests : TestBase
    {
        [Fact]
        public void QGrid_JSON_Test_ShouldReturnCorrectResults_1()
        {
            string json = @"{
                ""filters"": [
                    { ""property"": ""Age"", ""value"": ""30"", ""operand"": ""gt"", ""operator"": ""and"" },
                    { ""property"": ""Salary"", ""value"": ""45000"", ""operand"": ""gt"", ""operator"": ""and"" }
                ],
                ""sort"": [
                    { ""property"": ""Name"", ""ascending"": true }
                ]
            }";

            var query = _context.Employees.AsQueryable();
            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            DataStreamEvaluator<Employee> builder = new(query, filter);
            DataStreamResponse<EmployeeDTO> result = builder.Evaluate(employee => new EmployeeDTO
            {
                Name = employee.Name,
                Salary = employee.Salary,
                Age = employee.Age,
                CompanyName = employee.Company.Name,
                CompanyAddress = employee.Company.Address.Street,
                CompanyAverageSalary = employee.Company.Employees.Average(e => e.Salary),
                CompanyEmployeesCount = employee.Company.Employees.Count()
            });

            Assert.All(result.Data, e => Assert.True(e.Age > 30 && e.Salary > 45000));
            Assert.True(result.Data.Zip(result.Data.Skip(1), (a, b) => string.Compare(a.Name, b.Name) <= 0).All(x => x),
                "The data is not sorted by Name in ascending order.");
        }

        [Fact]
        public void QGrid_JSON_Test_ShouldReturnCorrectResults_2()
        {
            // Age less than 25 OR Company.Name equals "xAI", sorted by JoinDate descending
            string json = @"{
                ""filters"": [
                    { ""property"": ""Age"", ""value"": ""25"", ""operand"": ""lt"", ""operator"": ""or"" },
                    { ""property"": ""Company.Name"", ""value"": ""xAI"", ""operand"": ""eq"", ""operator"": ""or"" }
                ],
                ""sort"": [
                    { ""property"": ""JoinDate"", ""ascending"": false }
                ]
            }";
            var query = _context.Employees.AsQueryable();
            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            DataStreamEvaluator<Employee> builder = new(query, filter);
            DataStreamResponse<EmployeeDTO> result = builder.Evaluate(employee => new EmployeeDTO
            {
                Name = employee.Name,
                Salary = employee.Salary,
                Age = employee.Age,
                CompanyName = employee.Company.Name,
                CompanyAddress = employee.Company.Address.Street,
                CompanyAverageSalary = employee.Company.Employees.Average(e => e.Salary),
                CompanyEmployeesCount = employee.Company.Employees.Count()
            });

            Assert.All(result.Data, e => Assert.True(e.Age < 25 || e.CompanyName == "xAI"));
            Assert.True(result.Data.Zip(result.Data.Skip(1), (a, b) => a.JoinDate >= b.JoinDate).All(x => x),
                "The data is not sorted by JoinDate in descending order.");
        }

        [Fact]
        public void QGrid_JSON_Test_ShouldReturnCorrectResults_3()
        {
            // Name starts with "Jo" && IsActive equals true, paginated to 5 rows on page 1
            string json = @"{
                ""filters"": [
                    { ""property"": ""IsActive"", ""value"": ""true"", ""operand"": ""eq"", ""operator"": ""and"" }
                ],
                ""search"": {
                    ""term"": ""Jo"",
                    ""operand"": ""sw"",
                    ""properties"": [ ""Name"" ]
                },
                ""pagination"": {
                    ""rows"": 5,
                    ""page"": 1
                }
            }";
            var query = _context.Employees.AsQueryable();
            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            DataStreamEvaluator<Employee> builder = new(query, filter);
            DataStreamResponse<EmployeeDTO> result = builder.Evaluate(employee => new EmployeeDTO
            {
                Name = employee.Name,
                Salary = employee.Salary,
                Age = employee.Age,
                CompanyName = employee.Company.Name,
                CompanyAddress = employee.Company.Address.Street,
                CompanyAverageSalary = employee.Company.Employees.Average(e => e.Salary),
                CompanyEmployeesCount = employee.Company.Employees.Count()
            });

            Assert.All(result.Data, e => Assert.StartsWith("Jo", e.Name));
        }

        [Fact]
        public void QGrid_JSON_Test_ShouldReturnCorrectResults_4()
        {
            // JoinDate greater than "2020-01-01" && Company.Address.Street contains "Main", sorted by Salary descending
            string json = @"{
                ""filters"": [
                    { ""property"": ""JoinDate"", ""value"": ""1980-01-01"", ""operand"": ""gte"", ""operator"": ""and"" },
                    { ""property"": ""Company.Address.Street"", ""value"": ""Market"", ""operand"": ""cn"", ""operator"": ""and"" }
                ],
                ""sort"": [
                    { ""property"": ""Salary"", ""ascending"": false }
                ]
            }";
            var query = _context.Employees.AsQueryable();
            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            DataStreamEvaluator<Employee> builder = new(query, filter);
            DataStreamResponse<EmployeeDTO> result = builder.Evaluate(employee => new EmployeeDTO
            {
                Name = employee.Name,
                Salary = employee.Salary,
                Age = employee.Age,
                JoinDate = employee.JoinDate,
                CompanyName = employee.Company.Name,
                CompanyAddress = employee.Company.Address.Street,
                CompanyAverageSalary = employee.Company.Employees.Average(e => e.Salary),
                CompanyEmployeesCount = employee.Company.Employees.Count()
            });

            Assert.All(result.Data, e => Assert.True(e.JoinDate > DateTime.Parse("1980-01-01") && e.CompanyAddress.Contains("Market")));
            Assert.True(result.Data.Zip(result.Data.Skip(1), (a, b) => a.Salary >= b.Salary).All(x => x),
                "The data is not sorted by Salary in descending order.");
        }

        [Fact]
        public void QGrid_JSON_Test_SearchMultipleProperties_ShouldReturnCorrectResults()
        {
            string json = @"{
                ""filters"": [
                    { ""property"": ""Age"", ""value"": ""30"", ""operand"": ""gt"", ""operator"": ""and"" }
                ],
                ""search"": {
                    ""term"": ""son"",
                    ""operand"": ""cn"",
                    ""properties"": [ ""Name"", ""Company.Name"", ""Company.Address.Street"" ]
                },
                ""pagination"": {
                    ""rows"": 10,
                    ""page"": 1
                }
            }";

            var query = _context.Employees.AsQueryable();
            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            DataStreamEvaluator<Employee> builder = new(query, filter);
            DataStreamResponse<EmployeeDTO> result = builder.Evaluate(employee => new EmployeeDTO
            {
                Name = employee.Name,
                Salary = employee.Salary,
                Age = employee.Age,
                CompanyName = employee.Company.Name,
                CompanyAddress = employee.Company.Address.Street,
                CompanyAverageSalary = employee.Company.Employees.Average(e => e.Salary),
                CompanyEmployeesCount = employee.Company.Employees.Count()
            });

            Assert.All(result.Data, e => Assert.True(e.Age > 30));
            Assert.All(result.Data, e => Assert.True(
                e.Name.Contains("son", StringComparison.OrdinalIgnoreCase) ||
                e.CompanyName.Contains("son", StringComparison.OrdinalIgnoreCase) ||
                e.CompanyAddress.Contains("son", StringComparison.OrdinalIgnoreCase)));
            Assert.True(result.Data.Count() <= 10, "Pagination should limit to 10 rows.");
        }

        [Fact]
        public void QGrid_JSON_Test_MultipleSorts_ShouldReturnCorrectResults()
        {
            string json = @"{
                ""filters"": [
                    { ""property"": ""IsActive"", ""value"": ""true"", ""operand"": ""eq"", ""operator"": ""and"" }
                ],
                ""sort"": [
                    { ""property"": ""Company.Name"", ""ascending"": true },
                    { ""property"": ""Salary"", ""ascending"": false },
                    { ""property"": ""Age"", ""ascending"": true }
                ]
            }";

            var query = _context.Employees.AsQueryable();
            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            DataStreamFilter filter = JsonSerializer.Deserialize<DataStreamFilter>(ref reader)!;

            DataStreamEvaluator<Employee> builder = new(query, filter);
            DataStreamResponse<EmployeeDTO> result = builder.Evaluate(employee => new EmployeeDTO
            {
                Name = employee.Name,
                Salary = employee.Salary,
                Age = employee.Age,
                CompanyName = employee.Company.Name,
                CompanyAddress = employee.Company.Address.Street,
                CompanyAverageSalary = employee.Company.Employees.Average(e => e.Salary),
                CompanyEmployeesCount = employee.Company.Employees.Count(),
                IsActive = true
            });

            Assert.All(result.Data, e => Assert.True(e.IsActive));
            var sortedData = result.Data.ToList();
            for (int i = 0; i < sortedData.Count - 1; i++)
            {
                var a = sortedData[i];
                var b = sortedData[i + 1];
                int companyCompare = string.Compare(a.CompanyName, b.CompanyName, StringComparison.Ordinal);
                Assert.True(companyCompare <= 0, $"CompanyName not sorted ascending at {i}: {a.CompanyName} vs {b.CompanyName}");
                if (companyCompare == 0)
                {
                    Assert.True(a.Salary >= b.Salary, $"Salary not sorted descending at {i}: {a.Salary} vs {b.Salary}");
                    if (a.Salary == b.Salary)
                    {
                        Assert.True(a.Age <= b.Age, $"Age not sorted ascending at {i}: {a.Age} vs {b.Age}");
                    }
                }
            }
        }
    }
}
