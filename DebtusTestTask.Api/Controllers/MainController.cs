using DebtusTestTask.Application.Services;
using DebtusTestTask.Contracts.Input;
using DebtusTestTask.Contracts.Output;
using DebtusTestTask.Integrations.OrangeHRM;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DebtusTestTask.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class MainApiController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IOrderService _orderService;

    public MainApiController(IEmployeeService employeeService, IOrderService orderService)
    {
        _employeeService = employeeService;
        _orderService = orderService;
    }

    [Obsolete]
    [HttpPost]
    public async Task<IActionResult> CreateEmployeeOld([FromBody] EmployeeCreateBody body)
    {
        var result = await _employeeService.CreateEmployeeAsync(body);

        if (!result.IsSuccessfull || result.Result is null) return BadRequest(new ErrorResponse() { Success = result.IsSuccessfull, ErrorMessage = result.Messages.FirstOrDefault() });

        return Ok(new SuccessEmployeeResponse() { Success = result.IsSuccessfull, EmployeeId = result.Result.Id });
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromBody] EmployeeCreateBody body)
    {
        var empl = await _employeeService.CreateEmployeeAsync(body);

        return Ok(empl);
    }

    [HttpPost]
    public async Task<IActionResult> CreteOrder([FromBody] OrderCreateBody body)
    {
        var result = await _orderService.CreateOrderAsync(body);

        if (!result.IsSuccessfull || result.Result is null) return BadRequest(new ErrorResponse() { Success = result.IsSuccessfull, ErrorMessage = result.Messages.FirstOrDefault() });

        return Ok(new SuccessOrderResponse() { Success = result.IsSuccessfull, ReferenceId = result.Result.Id } );
    }

    [HttpPost]
    public async Task<IActionResult> TestAuth()
    {
        var client = new OrangeHttpClient();

        //var cookies = await client.GetAuthCookieAsync();

        //  https://opensource-demo.orangehrmlive.com/web/index.php/api/v2/admin/job-titles?limit=0

        //var sample = await client.CallApiAsync("/web/index.php/api/v2/admin/job-titles?limit=0", cookies);

        var b = new EmployeeCreateBody()
        {
            Id = "asd" + new Random().Next(0, 9999).ToString(),

            FirstName ="asd",
            MiddleName = "asd",
            LastName = "asd",
            Job = new JobCreateBody() { JobCategoryId = 1, EmploymentStatusId = 4, JobTitleId = 6, LocationId = 3, SubUnitId = 4, },
        };
        var empl = await _employeeService.CreateEmployeeAsync(b);

        return Ok(empl);
    }
}