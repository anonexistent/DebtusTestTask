using AutoMapper;
using BodyDto = DebtusTestTask.Contracts.Input.JobCreateBody;
using DebtusTestTask.Integrations.OrangeHRM.Contracts.Input;

namespace DebtusTestTask.Integrations.OrangeHRM.Services.Profiles;

public class JobCreateBodyProfile : Profile
{
    public JobCreateBodyProfile()
    {
        CreateMap<BodyDto, JobCreateBody>();
    }
}
