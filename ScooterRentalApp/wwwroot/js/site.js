// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var hideScooterFields = function () {
    var val = $("input[type='radio'][name='Type']:checked").val();

    if (val == 1) {
        $("#wheelSizeGroup").show();
    }
    else {
        $("#wheelSizeGroup").hide();
    }

    if (val == 2) {
        $("#batteryCapacityGroup").show();
    }
    else {
        $("#batteryCapacityGroup").hide();
    }
}

$("input[name='Type']").change(function () {
    var buttonVal = $("input[type='radio'][name='Type']:checked").val();
    alert(buttonVal);
    hideScooterFields();
});

hideScooterFields();