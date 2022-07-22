$(function () {
});


function Login() {

    var strParams = {

        Nombre: $('#txtEmail').val(),
        Password: $('#txtPassword').val(),
        GuardarLogin: $('#chkGuardarLogin').is(":checked")
    }
    $.ajax({
        url: window.urlValidaUsuario,
        async: false,
        cache: false,
        type: 'POST',
        data: { entity: strParams },
        success: function (data) {
            if (data == false) {
                $('#divErrorLogin').removeClass("hidden");
            }
            if (data == true) {
                window.location.href = "/Home";
            }



        },
        error: function () {
          //  showMessage('#divMensajePublicacionViaje', 'danger', 'Ocurrió un error al guardar la información. Por favor intente nuevamente.');
            //hideLoading();
        }
    });

};
