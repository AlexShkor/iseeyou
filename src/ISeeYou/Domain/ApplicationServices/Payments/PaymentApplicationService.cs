using System;
using System.Globalization;
using Braintree;
using ISeeYou.Views;

namespace ISeeYou.Domain.ApplicationServices.Payments
{
    public class PaymentApplicationService : IPaymentApplicationService
    {
        private readonly SiteSettings _settings;

        public PaymentApplicationService(SiteSettings settings)
        {
            _settings = settings;
        }

        public PaymentResult Process(UserView user, CreditCard creditCard)
        {
            var gateway = BraintreeGateway();
            var processResult = new PaymentResult();
            
            CreateCustomerIfNeeded(gateway, user);
            processResult.CustomerId = user.BraintreeCustomerId;

           /* var createCreditCardResult = AppendCreditCard(user, creditCard, gateway);

            if (!createCreditCardResult.IsSuccess())
            {
                processResult.Errors = new[] { createCreditCardResult.Message };
                return processResult;
            }*/
           

            var transactionResult = ProcessTransaction(user, gateway, creditCard);

            if (!transactionResult.IsSuccess())
            {
                processResult.Errors = new[] {transactionResult.Message};
                return processResult;
            }

            processResult.CreditCardId = transactionResult.Target.CreditCard.Token;

            var subscribtionResult = MakeSubscribtion(gateway, processResult.CreditCardId);
            if (!subscribtionResult.IsSuccess())
            {
                processResult.Errors = new[] {subscribtionResult.Message};
                return processResult;
            }
            processResult.SubscriptionId = subscribtionResult.Target.Id;
            processResult.Success = true;

            return processResult;
        }

        private static Result<Braintree.CreditCard> AppendCreditCard(UserView user, CreditCard creditCard, BraintreeGateway gateway)
        {
            var createCardRequest = new CreditCardRequest()
            {
                CustomerId = user.BraintreeCustomerId,
                Number = creditCard.Number,
                CVV = creditCard.Cvv,
                ExpirationMonth = creditCard.Month,
                ExpirationYear = creditCard.Year
            };

            var createCreditCardResult = gateway.CreditCard.Create(createCardRequest);
            return createCreditCardResult;
        }

        private Result<Transaction> ProcessTransaction(UserView user, BraintreeGateway gateway, CreditCard creditCard)
        {
            
            var request = new TransactionRequest()
            {
                Amount = _settings.BraintreeTransactionAmount,
                CustomerId = user.BraintreeCustomerId,
                CreditCard = new TransactionCreditCardRequest()
                {
                    Number = creditCard.Number,
                    ExpirationMonth = creditCard.Month,
                    ExpirationYear = creditCard.Year,
                    CVV = creditCard.Cvv
                },
                Options = new TransactionOptionsRequest()
                {
                    StoreInVault = true
                }
            };

            var result = gateway.Transaction.Sale(request);
            return result;
        }

        private Result<Subscription> MakeSubscribtion(BraintreeGateway gateway, string creditCardToken)
        {
            var firstBillingDate = DateTime.Now.AddMonths(1);
            var request = new SubscriptionRequest()
            {
                NeverExpires = true,
                FirstBillingDate = new DateTime(firstBillingDate.Year, firstBillingDate.Month, 1),
                PaymentMethodToken = creditCardToken,
                PlanId = _settings.BraintreeSubscribtionPlanId
            };
            var subresult = gateway.Subscription.Create(request);
            return subresult;
        }

        private void CreateCustomerIfNeeded(BraintreeGateway gateway, UserView user)
        {
            if (!string.IsNullOrEmpty(user.BraintreeCustomerId))
            {
                return;
            }
            var createCustomesRequest = new CustomerRequest()
            {
                CustomerId = user.Id,
                Email = user.Email,
            };

            var result = gateway.Customer.Create(createCustomesRequest);
            user.BraintreeCustomerId = result.IsSuccess() ? result.Target.Id : null;
        }

        private BraintreeGateway BraintreeGateway()
        {
            var gateway = new BraintreeGateway
            {
                Environment = _settings.BraintreeEnvironment,
                MerchantId = _settings.BraintreeMerchantId,
                PrivateKey = _settings.BraintreePrivateKey,
                PublicKey = _settings.BraintreePublicKey
            };
            return gateway;
        }
    }
}