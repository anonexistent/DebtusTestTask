using AutoMapper;
using DebtusTestTask.Application.Repositories;
using DebtusTestTask.Application.Services;
using DebtusTestTask.Contracts.Input;
using DebtusTestTask.Models;
using DebtusTestTask.Utils;
using System.Text.Json;
using BodyDto = DebtusTestTask.Integrations.OrangeHRM.Contracts.Input.EmployeeCreateBody;
using JobBodyDto = DebtusTestTask.Integrations.OrangeHRM.Contracts.Input.JobCreateBody;
using OrangeResult = DebtusTestTask.Integrations.OrangeHRM.Contracts.Output.SuccessEmployeeResponse;
using OrangeJobResult = DebtusTestTask.Integrations.OrangeHRM.Contracts.Output.SuccessJobResponse;

namespace DebtusTestTask.Integrations.OrangeHRM.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICurrencieRepository _currencieRepository;

    private readonly OrangeHttpClient _orangeHttpClient;

    private readonly IMapper _mapper;

    public EmployeeService(
        ICurrencieRepository currencieRepository,
        IEmployeeRepository employeeRepository,
        OrangeHttpClient orangeHttpClient,
        IMapper mapper
        )
    {
        _employeeRepository = employeeRepository;
        _currencieRepository = currencieRepository;

        _orangeHttpClient = orangeHttpClient;

        _mapper = mapper;
    }

    public async Task<ServiceResult<Employee>> CreateEmployeeAsync(EmployeeCreateBody request)
    {
        return await CreateEmployeeWithJobAsync(request);
    }

    private async Task<ServiceResult<Employee>> CreateEmplyeeEntityAsync(EmployeeCreateBody request)
    {
        var b = _mapper.Map<BodyDto>(request);
        var jb = _mapper.Map<JobBodyDto>(request.Job);

        var (httpCode, message) = await _orangeHttpClient.EmployeePostAsync(b);

        var orangeResult = JsonSerializer.Deserialize<OrangeResult>(message)
            ?? throw new Exception("orange response parsing error");

        if (httpCode is System.Net.HttpStatusCode.OK)
        {
            var employeeResult = await CommitEmployeeAsync(orangeResult.Data.EmpNumber, request);

            return employeeResult;
        }

        return new ServiceResult<Employee>()
        {
            IsSuccessfull = false,
            Messages = [message]
        };
    }

    public async Task<ServiceResult<Employee>> CreateEmployeeWithJobAsync(EmployeeCreateBody request)
    {
        var employeeResult = await CreateEmplyeeEntityAsync(request);
        if(!employeeResult.IsSuccessfull)
        {
            return new ServiceResult<Employee>()
            {
                IsSuccessfull = false,
                Messages = employeeResult.Messages,
            };
        }

        var jobResult = await CreateJobAsync(employeeResult.Result, request.Job);
        if(!jobResult.IsSuccessfull)
        {
            return new ServiceResult<Employee>()
            {
                IsSuccessfull = false,
                Messages = jobResult.Messages,
            };
        }

        return employeeResult;
    }

    public async Task<ServiceResult<bool>> CreateJobAsync(Employee request, JobCreateBody body)
    {
        var jb = _mapper.Map<JobBodyDto>(body);

        var (httpCode, message) = await _orangeHttpClient.EmployeeJobPutAsync(request.EmpNumber.ToString(), jb);

        var orangeResult = JsonSerializer.Deserialize<OrangeJobResult>(message)
            ?? throw new Exception("orange response parsing error");

        if (httpCode is System.Net.HttpStatusCode.OK)
        {
            return new ServiceResult<bool>()
            {
                IsSuccessfull = true,
                Result = true,
            };
        }

        return new ServiceResult<bool>()
        {
            IsSuccessfull = false,
            Messages = [message],
        };
    }

    private async Task<ServiceResult<Employee>> CommitEmployeeAsync(int empNumber, EmployeeCreateBody request)
    {
        var currs = await _currencieRepository.GetAllAsync();

        var allEmps = await _employeeRepository.GetAllAsync();

        var isExistendId = await _employeeRepository.AnyByIdAsync(request.Id);

        if (isExistendId)
        {
            return new ServiceResult<Employee>()
            {
                IsSuccessfull = false,
                Messages = ["incorrect id"]
            };
        }

        var employee = new Employee
        {
            Id = request.Id,

            EmpNumber = empNumber,

            LastName = request.LastName,
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
        };

        await _employeeRepository.CreateAsync(employee);

        return new ServiceResult<Employee>()
        {
            IsSuccessfull = true,
            Result = employee,
        };
    }
}
