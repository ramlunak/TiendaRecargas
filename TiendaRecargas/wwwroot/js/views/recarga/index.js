﻿
$(function () {

    $('#tipoRecarga').change(function (e) {
        CargarValores();
    });

    CargarValores();
});

function CargarValores() {

    var tipoRecarga = $('#tipoRecarga option:selected').text();

    $.ajax({
        type: "GET",
        url: "/Recargas/GetValores/" + tipoRecarga,
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            $("#idValorRecarga").empty();
            var arrayValores = JSON.parse(data.valores);
            $.each(arrayValores, function (index, item) {

                $("#idValorRecarga").append('<option value="' + item.id + '">' + item.valor.toFixed(2) + '</option>');

            });
        },
        failure: function (response) {
            console.log('failure', response);
        },
        error: function (response) {
            console.log('error', response);
        }
    });

}