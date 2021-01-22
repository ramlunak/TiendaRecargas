// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.
var timeout = 0;
var sugundosparacerrar = 10;
var timeoutModalOpen = false;
var modalTimeout;
$(function () {

    setInterval(function () {
        timeout++;

        if (!timeoutModalOpen && (timeout >= 3)) {
            timeoutModalOpen = true;
            sugundosparacerrar = 10;

            modalTimeout = Swal.fire({
                confirmButtonText: 'Cancelar',
                customClass: {
                    confirmButton: 'btn btn-danger',
                },
                buttonsStyling: false,
                html: '<div>Su sesion se cerrará en <b id="sugundosparacerrar"></b> segundos</div>'


            }).then((result) => {
                timeout = 0;
                timeoutModalOpen = false;
            })
        }

        if (timeoutModalOpen && sugundosparacerrar < 1) {
            timeoutModalOpen = false;
            location.href = "/login/Salir";
        }

    }, 1000);

    setInterval(function () {
        sugundosparacerrar--;
        if (sugundosparacerrar >= 0)
            $('#sugundosparacerrar').html(sugundosparacerrar);
    }, 1000);

    $(document).mousemove(function () {
        console.log(timeout);

        timeout = 0;
    });

});

//const tick = () => {
//    const now = new Date();
//    const h = now.getHours();
//    const m = now.getMinutes();
//    const s = now.getSeconds();

//    const html = `
//    <span>${h}</span> :
//    <span>${m}</span> :
//    <span>${s}</span>
//`;

//    $('#clock').html = html;
//    console.log(h, m, s);
//};

//setInterval(tick, 1000);