
$(function () {

    $('[data-toggle="tooltip"]').tooltip();

    var modal = Swal.fire({
        title: 'Informe el valor que desea aumentar!!',
        input: 'text',
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
                    IdCuenta: 1
                })
            }).then(response => {
                if (login == "") {
                    throw new Error("informe un valor mayor que $ 0.00 USD")
                } else
                    if (parseFloat(login) == 0) {
                        throw new Error("informe un valor mayor que $ 0.00 USD")
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
                            `informe un valor mayor que $ 0.00 USD`
                        )
                    } else {
                        Swal.showValidationMessage(
                            `Usted no tiene fondos suficientes para completar esta acción`
                        )
                    }

                })
        },
        allowOutsideClick: () => !Swal.isLoading()
    }).then((result) => {
        Swal.fire(
            'Acción completada!',
            'El crédito de la cuenta se ha modificado.',
            'success'
        )
    })

    $(".swal2-input").mask("###0.00", { reverse: true });
    // $(".swal2-input").val(45);

});