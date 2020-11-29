
$(function () {
    $("#inputPrecioRecarga").mask("###0.00", { reverse: true });
    $("#inputCredito").mask("###0.00", { reverse: true });
    $("#inputPorciento").mask("###0.00", { reverse: true });

    $("#inputPrecioRecarga").on("input", (e) => {

        var value = parseFloat($("#inputPrecioRecarga").val());

        //Calcular el % para una recarga con valor de 20 USD
        if (!isNaN(value)) {
            var porciento = value * 100 / 20;
            $("#InputShowPorcientoCalculado").val(porciento.toFixed(2).replace(',','.'));
            $("#inputPorciento").val(porciento.toFixed(2).replace(',', '.'));
        } else {
            $("#InputShowPorcientoCalculado").val(null);
            $("#inputPrecioRecarga").val(null);
        }
      

    });

});