using ISeeYou.Views;

namespace ISeeYou.Domain.ApplicationServices.Payments
{
    public interface IPaymentApplicationService
    {
        PaymentResult Process(UserView user, CreditCard creditCard); 
    }
}