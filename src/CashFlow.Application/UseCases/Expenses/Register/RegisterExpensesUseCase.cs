using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Register;
public class RegisterExpensesUseCase : IRegisterExpensesUseCase
{
    private readonly IExpensesWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitofWork;
    private readonly IMapper _mapper;
    public RegisterExpensesUseCase(IExpensesWriteOnlyRepository repository,IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitofWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<ResponsesRegisterExpenseJson> Execute(RequestExpenseJson request)
    {
        Validate(request);


        var entity = _mapper.Map<Expense>(request);

        await _repository.Add(entity);

        await _unitofWork.Commit();

        return _mapper.Map<ResponsesRegisterExpenseJson>(entity);
    }
    private static void Validate(RequestExpenseJson request)
    {
        
        var validator = new RegisterExpenseValidator();

        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            throw new ErrorOnValidateException(errorMessages);
        }

    }
}
