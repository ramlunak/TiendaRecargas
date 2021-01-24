
$(function () {

    $('[data-toggle="tooltip"]').tooltip();

});

function EditarCredito(idCuenta) {

    var modal = Swal.fire({
        title: 'Informe el valor!!',
        input: 'text',
        html:
            '<input id="swal-input1" name="swal-input1"  type="checkbox"> Disminuir',
        inputAttributes: {
            autocapitalize: 'off'
        },
        showCancelButton: true,
        cancelButtonText: 'Cancelar',
        confirmButtonText: 'Confirmar',
        showLoaderOnConfirm: true,
        preConfirm: (login) => {
            return fetch('cuenta/AddCredito/', {
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                method: "POST",
                body: JSON.stringify({
                    IdCuenta: idCuenta,
                    Credito: parseFloat(login),
                    Activo: $('#swal-input1').is(":checked")
                })
            }).then(response => {
                if (login == "") {
                    throw new Error("informe un valor diferente de $ 0.00 USD")
                } else
                    if (parseFloat(login) == 0) {
                        throw new Error("informe un valor diferente de $ 0.00 USD")
                    }
                    else
                        if (!response.ok) {
                            throw new Error()
                        }
                return response.json()
            })
                .catch(error => {

                    if (parseFloat(login) == 0) {
                        Swal.showValidationMessage(
                            `informe un valor diferente de $ 0.00 USD`
                        )
                    } else {
                        if (!$('#swal-input1').is(":checked")) {
                            Swal.showValidationMessage(
                                `Usted no tiene fondos suficientes para completar esta acción`
                            )
                        } else {
                            Swal.showValidationMessage(
                                `La cuenta no tiene fondos suficientes para completar esta acción`
                            )
                        }
                    }

                })
        },
        allowOutsideClick: () => !Swal.isLoading()
    }).then((result) => {

        if (result.isConfirmed) {

            Swal.fire(
                'Acción completada!',
                'El crédito de la cuenta se ha modificado.',
                'success'
            ).then((re) => {
                location.reload(true);
            });

        }

    })

    $(".swal2-input").mask("###0.00", { reverse: true });

}


function LiberarCredito(idCuenta) {
    console.log(idCuenta);
    var modal = Swal.fire({
        title: 'Confirma liberar el crédito de esta cuenta?',
        showCancelButton: true,
        cancelButtonText: 'Cancelar',
        confirmButtonText: 'Confirmar',
        showLoaderOnConfirm: true,
        preConfirm: (login) => {
            return fetch('cuenta/LiberarCredito/' + idCuenta, {
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                method: "GET",
                //body: JSON.stringify({
                //    idCuenta: idCuenta
                //})
            }).then(response => {

                return response.json()
            })
                .catch(error => {

                    Swal.showValidationMessage(
                        `Ocurrió un error, contacte con soporte técnico`
                    )

                })
        },
        allowOutsideClick: () => !Swal.isLoading()
    }).then((result) => {

        if (result.isConfirmed) {

            Swal.fire(
                'Acción completada!',
            ).then((re) => {
                location.reload(true);
            });

        }

    })

    $(".swal2-input").mask("###0.00", { reverse: true });

}