using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using System.ComponentModel;

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
        CreateMap<RequestExpenseJson, Expense>();
    }
    private void EntityToResponse()
    {
        CreateMap<Expense, ResponsesRegisterExpenseJson>();
        CreateMap<Expense, ResponseShortExpenseJson>();
        CreateMap<Expense, ResponseExpenseJson>();
    }
}
