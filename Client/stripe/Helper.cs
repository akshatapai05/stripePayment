namespace StripePay.stripe
{
    using Stripe;
    using System.Collections.Generic;

    public static class Helper
    {
        //public static CreditCardOptions CreateCard()
        //{
        //    return new CreditCardOptions
        //    {
        //        Name = "Jane DOe",
        //        Number = "4242424242424242",
        //        ExpYear = 2025,
        //        ExpMonth = 7,
        //        Cvc = "000"
        //    };
        //}

        public static Customer CreateCustomer(string email, CreditCardOptions cardOptions = null)
        {
            Token token = null;
            if (cardOptions != null)
            {
                token = CreateToken(cardOptions);
            }

            CustomerCreateOptions myCustomer = new CustomerCreateOptions
            {
                Email = email,
                Source = token?.Id,
            };

            var customerService = new CustomerService();
            return customerService.Create(myCustomer);
        }

        public static void AttachPaymentMethod(Customer existingCustomer, PaymentMethod paymentMethod, PaymentMethodService paymentMethodService)
        {
            

            // Attach a payment method to a customer
            var paymentMethodAttachOptions = new PaymentMethodAttachOptions { Customer = existingCustomer.Id };
            paymentMethodService.Attach(paymentMethod.Id, paymentMethodAttachOptions);
        }

        public static PaymentIntent CreatePaymentIntent(long amount, string currency, Customer customer, PaymentIntentService paymentIntentService)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = currency,
                PaymentMethodTypes = new List<string> { "card" },
                Customer = customer.Id,
                Metadata = new Dictionary<string, string>
                {
                  { "integration_check", "accept_a_payment" },
                },
            };

            return paymentIntentService.Create(options);
        }

        public static Token CreateToken(CreditCardOptions cardDetails = null)
        {
            TokenService serviceToken = new TokenService();
            TokenCreateOptions token = new TokenCreateOptions { Card = cardDetails };
            return serviceToken.Create(token);
        }

        public static StripeList<Customer> ListCustomers()
        {
            var options = new CustomerListOptions
            {
                Limit = 10,
            };

            var service = new CustomerService();
            return service.List(options);
        }

        public static Charge CreateCharge(long amount, string currencyIso, string email, string customerId, string description = null)
        {
            //Create Charge Object with details of Charge  
            var chargeOptions = new ChargeCreateOptions
            {
                Amount = amount,
                Currency = currencyIso,
                ReceiptEmail = email,
                Customer = customerId,
                Description = description,
            };

            var service = new ChargeService();
            return service.Create(chargeOptions); // This will do the Payment
        }

        public static PaymentMethod CreateCardPaymentMethod(string cardNumber, long expMonth, long expYear, string cvc, PaymentMethodService paymentMethodService)
        {
            // Create a payment method
            var options = new PaymentMethodCreateOptions
            {
                Type = "card",
                Card = new PaymentMethodCardCreateOptions
                {
                    Number = cardNumber,
                    ExpMonth = expMonth,
                    ExpYear = expYear,
                    Cvc = cvc,
                },
            };

            return paymentMethodService.Create(options);
        }
    }
}
