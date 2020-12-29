
$(function () {

    $('#tipoRecarga').change(function (e) {
        CargarValores();
    });

    CargarValores();
});

function CargarValores() {

    var tipoRecarga = $('#tipoRecarga option:selected').text();
    MostarMovilNauta(tipoRecarga.toLocaleLowerCase());

    $.ajax({
        type: "GET",
        url: "/Recargas/GetValores/" + tipoRecarga,
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            var isValorSeleccionado = $('#isValorSeleccionado').val();
            $("#idValorRecarga").empty();
            var arrayValores = JSON.parse(data.valores);
            $.each(arrayValores, function (index, item) {

                if (parseInt(isValorSeleccionado) === item.id) {
                    $("#idValorRecarga").append('<option selected value="' + item.id + '">' + item.valor.toFixed(2) + '</option>');
                } else {
                    $("#idValorRecarga").append('<option value="' + item.id + '">' + item.valor.toFixed(2) + '</option>');
                }
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

function DeleteRecarga(id) {

    Swal.fire({
        title: 'Está seguro que desea eliminar esta recarga?',
        showDenyButton: true,
        showCancelButton: false,
        confirmButtonText: `Eliminar`,
        denyButtonText: `Cancelar`,
    }).then((result) => {

        if (result.isConfirmed) {

            $.ajax({
                type: "GET",
                url: "/Recargas/Delete/" + id,
                traditional: true,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    console.log(data);
                    window.location.href = '/Recargas/Index/';
                },
                failure: function (response) {
                    console.log('failure', response);
                },
                error: function (response) {
                    console.log('error', response);
                }
            });

        } else if (result.isDenied) {

        }
    })
    return;

}

function MostarMovilNauta(tipo) {
    if (tipo === "movil") {
        $('#coreoNauta').hide();
        $('#numeroMovil').show();
    } else if (tipo === "nauta") {
        $('#numeroMovil').hide();
        $('#coreoNauta').show();
    }
}

function showLoading() {
    Swal.fire({
        showConfirmButton: false,
        allowOutsideClick: false,
        imageAlt: 'A tall image',
        html: "<div class='d-block justify-content-center'> <div class='spinner-border text-primary mr-3' role='status'></div > <div>El sistema esta procesando las recargas. Por favor espere...</div> </div >"
    })
}
