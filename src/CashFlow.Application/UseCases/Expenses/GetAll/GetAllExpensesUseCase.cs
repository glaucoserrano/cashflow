using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception.ExceptionsBase;
using CashFlow.Exception;

namespace CashFlow.Application.UseCases.Expenses.GetAll;
public class GetAllExpensesUseCase : IGetAllExpensesUseCase
{
    private readonly IExpensesReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    public GetAllExpensesUseCase(IExpensesReadOnlyRepository respository,
        IMapper mapper,
        ILoggedUser loggedUser)
    {
        _repository = respository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseExpensesJson> Execute()
    {
        var loggedUser = await _loggedUser.Get();
        var result = await _repository.GetAll(loggedUser);

        if (result.Count==0)
        {
            throw new NotFoundExcepption(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }

        return new ResponseExpensesJson
        {
            Expenses = _mapper.Map<List<ResponseShortExpenseJson>>(result)
        };

    }
}
