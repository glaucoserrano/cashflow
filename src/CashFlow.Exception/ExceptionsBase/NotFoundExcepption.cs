using System.Net;

namespace CashFlow.Exception.ExceptionsBase;
public  class NotFoundExcepption : CashFlowException
{
    public NotFoundExcepption(string message) : base(message)
    {
        
    }

    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public override List<string> GetErrors() => new List<string>(){
            Message
        };
}
