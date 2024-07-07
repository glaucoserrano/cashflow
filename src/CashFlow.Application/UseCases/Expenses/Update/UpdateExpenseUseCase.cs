using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Update;
public class UpdateExpenseUseCase : IUpdateExpenseUseCase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExpensesUpdateOnlyRepository _repository;

    public UpdateExpenseUseCase(IMapper mapper, IUnitOfWork unitOfWork, IExpensesUpdateOnlyRepository repository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task Execute(long id, RequestExpenseJson request)
    {
        Validade(request);

        var expense = await _repository.GetById(id);

        if (expense is null) 
        {
            throw new NotFoundExcepption(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }

        _mapper.Map(request, expense);

        _repository.Update(expense);

        await _unitOfWork.Commit();
    }

    private static void  Validade(RequestExpenseJson request)
    {
        var validator = new RegisterExpenseValidator();

        var result = validator.Validate(request);

        if(result.IsValid is false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidateException(errorMessages);
        }
    }
}
