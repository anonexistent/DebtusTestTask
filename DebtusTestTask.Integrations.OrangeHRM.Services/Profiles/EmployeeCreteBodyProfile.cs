using AutoMapper;
using BodyDto = DebtusTestTask.Contracts.Input.EmployeeCreateBody;
using DebtusTestTask.Integrations.OrangeHRM.Contracts.Input;

namespace DebtusTestTask.Integrations.OrangeHRM.Services.Profiles;

public class EmployeeCreteBodyProfile : Profile
{
    public EmployeeCreteBodyProfile()
    {
        CreateMap<BodyDto, EmployeeCreateBody>();
    }
}
