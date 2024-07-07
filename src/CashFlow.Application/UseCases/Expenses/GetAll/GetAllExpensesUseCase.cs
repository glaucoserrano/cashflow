using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Application.UseCases.Expenses.GetAll;
internal class GetAllExpensesUseCase : IGetAllExpensesUseCase
{
    private readonly IExpensesReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    public GetAllExpensesUseCase(IExpensesReadOnlyRepository respository,IMapper mapper)
    {
        _repository = respository;
        _mapper = mapper;
    }
    public async Task<ResponseExpensesJson> Execute()
    {
        var result = await _repository.GetAll();

        return new ResponseExpensesJson
        {
            Expenses = _mapper.Map<List<ResponseShortExpenseJson>>(result)
        };

    }
}
