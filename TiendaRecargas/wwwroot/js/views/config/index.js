
$(function () {

});

function UpdatePassword() {

    var updatePassword = {
        idCuenta: $('#inputIdcuenta').val(),
        Usuario: $('#inputUsuario').val(),
        Password: $('#inputPassword').val(),
        ConfirmPassword: $('#inputConfirmPassword').val()
    }

    console.log(updatePassword);
}