﻿using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;

namespace CashFlow.Application.AutoMapper;
public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityToResponse();
    }
    private void RequestToEntity()
    { 
        CreateMap<RequestRegisterJson, User>().
            ForMember(dest => dest.Password,config => config.Ignore());

        CreateMap<RequestExpenseJson, Expense>()
            .ForMember(dest => dest.Tags, config => config.MapFrom(source => source.Tags.Distinct()));

        CreateMap<Communication.Enums.Tag, Tag>()
            .ForMember(dest => dest.Value, config => config.MapFrom(source => source));
    }
    private void EntityToResponse()
    {
        CreateMap<Expense, ResponsesRegisterExpenseJson>();
        CreateMap<Expense, ResponseShortExpenseJson>();
        
        CreateMap<Expense, ResponseExpenseJson>()
            .ForMember(dest => dest.Tags, 
            config => config.MapFrom(source => source.Tags.Select(tag => tag.Value)));
        
        CreateMap<User, ResponseUserProfileJson>();
    }
}
