// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Credit card formatting
$('input[name="Input.CreditCardNo"]').on('input', function () {
    let value = $(this).val().replace(/\D/g, '');
    if (value.length > 16) {
        value = value.substr(0, 16);
    }
    $(this).val(value.replace(/(\d{4})(?=\d)/g, '$1 '));
});

// Phone number formatting
$('input[name="Input.MobileNo"]').on('input', function () {
    let value = $(this).val().replace(/\D/g, '');
    if (value.length > 10) {
        value = value.substr(0, 10);
    }
    if (value.length >= 6) {
        value = value.replace(/(\d{3})(\d{3})(\d{4})/, '$1-$2-$3');
    } else if (value.length >= 3) {
        value = value.replace(/(\d{3})(\d{0,3})/, '$1-$2');
    }
    $(this).val(value);
});
