using AutoMapper;
using BodyDto = DebtusTestTask.Contracts.Input.OrderCreateBody;
using DebtusTestTask.Integrations.OrangeHRM.Contracts.Input;

namespace DebtusTestTask.Integrations.OrangeHRM.Services.Profiles;

public class OrderCreateBodyProfile : Profile
{
    public OrderCreateBodyProfile()
    {
        CreateMap<BodyDto, OrderCreateBody>();
    }
}
