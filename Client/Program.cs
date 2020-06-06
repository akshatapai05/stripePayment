namespace StripePay
{
    using Stripe;
    using System.Linq;
    using StripePay.stripe;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter seret key: ");
            StripeConfiguration.ApiKey = Console.ReadLine();

            Console.Write("Enter customer email: ");
            string email = Console.ReadLine();

            // Create a customer
            var customer = Helper.CreateCustomer("Someemail@gmail.com");

            // Get customer by id
            var service = new CustomerService();
            var existingCustomer = service.Get("cus_HNUUIzXddNpRE7");

            var paymentMethodService = new PaymentMethodService();
            var paymentIntentService = new PaymentIntentService();

            Console.WriteLine(@"Create Transaction\Payment Intent:");

            long amount = 0;
            Console.Write("Enter Amount (lowest denominator of currency):");
            while (!long.TryParse(Console.ReadLine(), out amount))
            {
                Console.WriteLine("Entered amount value cannot be converted to numeric. Please Try again.");
                Console.Write("Enter Amount (lowest denominator of currency):");
            }

            Console.Write("Enter Currency (3 Letter ISO):");
            string currency = Console.ReadLine();

            //Create a paymentIntent/TransactionIntent for the customer
            var intent = Helper.CreatePaymentIntent(amount, currency, existingCustomer, paymentIntentService);

            Console.WriteLine($"Intent has been create with client secret: '{intent.ClientSecret}'.\nTo verify please check the Stripe payments dashboard.");
            Console.Write("You can exit the program now. Use the web server to complete the request via Stripe Elements. Or if you want to add the payment method here, you can press any key to coninue.");
            Console.ReadLine();

            Console.WriteLine("Do you want to enter a payment method? (y/n)");
            char response = Console.ReadLine().Single();
            if (response == 'y' || response == 'Y')
            {
                Console.WriteLine("Enter Card Details:");
                Console.Write("Enter Card Number: ");
                string cardNumber = Console.ReadLine();

                Console.Write("CVC: ");
                string cvc = Console.ReadLine();

                Console.Write("Expiry (month):");
                long expMonth = long.Parse(Console.ReadLine());

                Console.Write("Expiry (year):");
                long expYear = long.Parse(Console.ReadLine());

                // Create and attach payment method to customer
                var cardPaymentMethod = Helper.CreateCardPaymentMethod(cardNumber, expMonth, expYear, cvc, paymentMethodService);
                Helper.AttachPaymentMethod(existingCustomer, cardPaymentMethod, paymentMethodService);

                // Retrieve the paymentintent/transactionIntent for a customer
                var paymentIntents = paymentIntentService.List(
                    new PaymentIntentListOptions
                    {
                        Customer = existingCustomer.Id,
                    })
                    .OrderBy(x => x.Created).ToList();

                // Confirm the transaction
                var confirmOptions = new PaymentIntentConfirmOptions
                {
                    PaymentMethod = cardPaymentMethod.Id,
                };

                paymentIntentService.Confirm(intent.Id, confirmOptions);
                //paymentIntentService.Confirm(paymentIntents.Last().Id, confirmOptions);
            }

            return;
        }
    }
}
