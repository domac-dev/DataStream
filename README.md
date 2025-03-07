### What's DataStream?

**DataStream** is a library designed to simplify server-side operations such as filtering, sorting and pagination by dynamically generating `LINQ` expressions making data manipulation easier for grid-like data views in APIs and services.

The idea is to enable client applications to send a request object specifying the data manipulation tasks, which are then evaluated on the server and returned as a structured response.

### Key Features

- **Multi-Field Operations**: Supports filtering and sorting across multiple fields and nested properties
- **Operands**: Comparison (`Equals`,`GreatherThan`..) and string operands (`StartsWith`, `Contains`..)
- **Operators**: Combine conditions using `AND` or `OR` operators
- **Search**: Search across multiple properties with operands (`StartsWith`, `Contains`..)
- **Pagination**: Provides paginated response with option for data mapping

### How does it work?

Let's say we have a simple database model with **Employee** and **Company**.

```csharp
public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public ICollection<Employee> Employees { get; set; } 
}

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public double Salary { get; set; }
    public int CompanyId { get; set; }
    public Company Company { get; set; }
}

```
To retrieve a filtered and paginated list of employees, we need to send a `DataStreamFilter` object to the API endpoint.

Here's the example in JSON format:

```json
{
  "filters": [
    {
      "property": "Salary",
      "value": "2500",
      "operand": "gt",
      "operator": "and"
    },
    {
      "property": "Company.Name",
      "value": "Global Company",
      "operand": "eq",
      "operator": "and"
    }
  ],
  "search" : {
    "term": "Jo",
    "operand": "sw",
    "properties": ["FirstName"]
  },
  "sort": [
    {
      "property": "FirstName",
      "ascending": true
    }
  ],
  "pagination": {
    "rows": 5,
    "page": 1
  }
}


```
By sending this filter we apply:
- `Salary > 2500` (employees earning more than 2500).
- `Company.Name == "Global Company"` (employees working for "Global Company").
- `FirstName` starts with "Jo".
- Sort by `FirstName` in ascending order.
- First page with maximum of 10 records per page.

In the `EmployeeController`, we accept a `QueryModel` from the request body and apply it to the `IQueryable<Employee>` query.

```csharp
[ApiController]
[Route("api/employees")]
public class EmployeeController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpPost]
    public IActionResult GetEmployees([FromBody] DataStreamFilter queryModel)
    {
        IQueryable<Employee> query = _context.Employees;
     
        DataStreamResponse<EmployeeDTO> result = query.Evaluate(queryModel, employee => new EmployeeDTO
        {
            Name = employee.Name,
            Salary = employee.Salary,
            CompanyName = employee.Company.Name
        });

        return Ok(result);
    }
}
```
To apply data operations from the request, we use `Evaluate` method.
The result is a mapped data response, complete with pagination details, wrapped in a `DataStreamResponse` object.

Example of the `DataStreamResponse` response in JSON format:

```json
{
  "pagination": {
    "currentPage": 1,
    "pageSize": 5,
    "totalCount": 8,
    "count": 5,
    "totalPages": 2,
    "hasNext": true,
    "hasPrevious": false,
    "firstItemIndex": 1,
    "lastItemIndex": 5,
    "isFirstPage": true,
    "isLastPage": false,
    "hasData": true
  },
  "data": [
    {
      "Name": "Johanna Harris",
      "Salary": 57000,
      "Age": 34,
      "CompanyName": "TechCorp"
    },
    {
      "Name": "Johnas Brown",
      "Salary": 60000,
      "Age": 38,
      "CompanyName": "TechCorp"
    },
    {
      "Name": "John Carter",
      "Salary": 55000,
      "Age": 32,
      "CompanyName": "TechCorp"
    },
    {
      "Name": "Jonny Moore",
      "Salary": 49000,
      "Age": 29,
      "CompanyName": "TechCorp"
    },
    {
      "Name": "Jori White",
      "Salary": 63000,
      "Age": 40,
      "CompanyName": "TechCorp"
    }
  ]
}
```


