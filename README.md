# Stripe Payment (Client-Server Mocks)

Client Mockup:
Steps:
1. Run the Project '\stripePayment\ClientStripePay.csproj'
2. Enter the secret key for the Stripe account.
3. Enter email for the new customer. Press enter.
4. The customer will be generated.
5. Next, you will be prompted to enter the amount and currency for the Payment Intent.
6. After that you will shown the client_secret for the PI object.
7. Copy the secret for use in the Server mockup project.

Server Mockup:
1. Open the project '\stripePayment\Server\stripePayWeb.csproj'
2. You will be prompted for the PublishableKEy for the same Stripe account.
3. Next you will be presented with the Stripe elements page where you can enter:
- The payment intent client secret you captured in the last step of the Cliest steps.
- The card details
4. Press pay to attempt the transaction.