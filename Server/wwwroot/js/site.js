// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
var stripe = Stripe('pk_test_crE47RSmMiLYgYyeV8Ysukd300uyVhuoDk');
var elements = stripe.elements();

var card = elements.create('card', {
    iconStyle: 'solid',
    style: {
        base: {
            iconColor: '#8898AA',
            color: 'white',
            lineHeight: '36px',
            fontWeight: 300,
            fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
            fontSize: '19px',

            '::placeholder': {
                color: '#8898AA',
            },
        },
        invalid: {
            iconColor: '#e85746',
            color: '#e85746',
        }
    },
    classes: {
        focus: 'is-focused',
        empty: 'is-empty',
    },
});
card.mount('#card-element');

var inputs = document.querySelectorAll('input.field');
Array.prototype.forEach.call(inputs, function (input) {
    input.addEventListener('focus', function () {
        input.classList.add('is-focused');
    });
    input.addEventListener('blur', function () {
        input.classList.remove('is-focused');
    });
    input.addEventListener('keyup', function () {
        if (input.value.length === 0) {
            input.classList.add('is-empty');
        } else {
            input.classList.remove('is-empty');
        }
    });
});

function setOutcome(result) {
    var successElement = document.querySelector('.success');
    var errorElement = document.querySelector('.error');
    successElement.classList.remove('visible');
    errorElement.classList.remove('visible');

    if (result.token) {
        // Use the token to create a charge or a customer
        // https://stripe.com/docs/payments/charges-api
        successElement.querySelector('.token').textContent = result.token.id;
        successElement.classList.add('visible');

        confirmPayment();
    } else if (result.error) {
        errorElement.textContent = result.error.message;
        errorElement.classList.add('visible');
    }
}

function confirmPayment() {
    var clientSecret = document.getElementById("payment-intent-client-secret").value;
    stripe
        .confirmCardPayment(clientSecret, {
            payment_method: {
                card: card,
                billing_details: {
                    name: 'TedtUser',
                },
            },
        })
        .then(function (result) {
            // Handle result.error or result.paymentIntent
        });

    
}

function stripeTokenHandler(token) {
    // Insert the token ID into the form so it gets submitted to the server
    var form = document.getElementById('payment-form');
    var hiddenInput = document.createElement('input');
    hiddenInput.setAttribute('type', 'hidden');
    hiddenInput.setAttribute('name', 'stripeToken');
    hiddenInput.setAttribute('value', token.id);
    form.appendChild(hiddenInput);

    // Submit the form
    form.submit();
}

card.on('change', function (event) {
    setOutcome(event);
});

document.querySelector('form').addEventListener('submit', function (e) {
    e.preventDefault();
    var form = document.querySelector('form');
    var extraDetails = {
        name: form.querySelector('input[name=cardholder-name]').value,
    };
    stripe.createToken(card, extraDetails).then(setOutcome);
});

