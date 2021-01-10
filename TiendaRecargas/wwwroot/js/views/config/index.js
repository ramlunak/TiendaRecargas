
$(function () {

});

function UpdatePassword() {

    var updatePassword = {
        idCuenta: $('#inputIdcuenta').val(),
        Usuario: $('#inputUsuario').val(),
        Password: $('#inputPassword').val(),
        ConfirmPassword: $('#inputConfirmPassword').val()
    }

    if (updatePassword.Password !== updatePassword.ConfirmPassword) return;

    $.ajax({
        type: "POST",
        url: "/Config/UpdatePassword",
        traditional: true,
        data: JSON.stringify(updatePassword),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            console.log(data);
            if (!data.error) {
                $('#modalCambiarPass').modal("hide");
                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                    title: 'Su contraseña ha sido cambiada correctamente.',
                    showConfirmButton: false,
                    timer: 1500
                })
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'No se pudo actualizar la contraseña, entre en contacto con soporte técnico.'
                })
            }

        },
        failure: function (response) {
            console.log('failure', response);

        },
        error: function (response) {
            console.log('error', response);

        }
    });
}