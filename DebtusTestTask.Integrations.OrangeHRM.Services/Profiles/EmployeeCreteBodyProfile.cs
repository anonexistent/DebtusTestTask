using AutoMapper;
using DebtusTestTask.Contracts.Input;
using BodyDto = DebtusTestTask.Integrations.OrangeHRM.Contracts.Input.EmployeeCreateBody;

namespace DebtusTestTask.Integrations.OrangeHRM.Services.Profiles;

public class EmployeeCreteBodyProfile : Profile
{
    public EmployeeCreteBodyProfile()
    {
        CreateMap<EmployeeCreateBody, BodyDto>();
    }
}
