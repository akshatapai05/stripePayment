namespace StripePay.stripe
{
    using Stripe;
    using System;

    public class Orchestrator
    {
        private readonly ChargeCreateOptions chargeOptions;

        private CreditCardOptions cardOptions;

        private Customer customer;

        public Orchestrator(CreditCardOptions cardOptions, Customer customer)
        {
            this.cardOptions = cardOptions;
            this.customer = customer;
            this.chargeOptions = CreateCharge();
        }

        public Customer CustomerData => this.customer;

        private ChargeCreateOptions CreateCharge()
        {
            //Create Charge Object with details of Charge  
            return new ChargeCreateOptions
            {
                Amount = 150,
                Currency = "USD",
                ReceiptEmail = "siriusblack711@gmail.com",
                Customer = this.customer.Id,
                Description = "Test transaction", //Optional  
            };
        }

        public Charge CreateChargeOnStripe()
        {
            //and Create Method of this object is doing the payment execution.  
            var service = new ChargeService();
            return service.Create(this.chargeOptions); // This will do the Payment
        }
    }
}
