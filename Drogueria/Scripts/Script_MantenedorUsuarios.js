var _idUsuario = 0;

$(function () {

    ObtenerRoles();
    ObtenerEstablecimientoDrogueria();
});

function ObtenerRoles() {

    $.ajax({
        url: window.urlObtenerRoles,
        type: 'POST',
        success: function (data) {

            $('#cmbRoles').empty();
            
            $.each(data,
                function (value, item) {
                    var texto = '<option value="' + item.Id + '">' + item.RolStr + '</option>';
                    $('#cmbRoles').append(texto);

                }
            );

        },
        error: function () {
            showMessage('body', 'danger', 'Ocurrió un error al listar las unidades.');

        }
    });
}
function ObtenerEstablecimientoDrogueria() {

    $.ajax({
        url: window.urlObtenerEstablecimientoDrogueria,
        type: 'POST',
        success: function (data) {

            $('#cmbEstablecimientos').empty();
            $('#cmbEstablecimientos').append('<option value="-1">[Seleccione un establecimiento]</option>')
            $.each(data,
                function (value, item) {
                    var texto = '<option value="' + item.Id + '">' + item.Descripcion + '</option>';
                    $('#cmbEstablecimientos').append(texto);

                }
            );

        },
        error: function () {
            showMessage('body', 'danger', 'Ocurrió un error al listar los establecimientos.');

        }
    });
}
function GuardaUsuario() {

    if (ValidaGuardar() == false)
        return;

    var arreglo = $("#cmbRoles").dropdown("get value");
    var largoArreglo = arreglo.length;

    var textoArreglo = '';
    for (i = 0; i < largoArreglo; i++) {
        textoArreglo = textoArreglo + ',' + arreglo[i];
    }

    $('#msjError').addClass("hidden");
    $('#btnGuardar').addClass('loading');
    $('#btnGuardar').addClass('disabled');
    $('#btnSalir').addClass('loading');
    $('#btnSalir').addClass('disabled');

    var strParams = {
        Id: _idUsuario,
        Nombre: $('#txtNombre').val(),
        Username: $('#txtusuario').val(),
        Password: $('#txtContraseña').val(),
        Correo: $('#txtCorreo').val(),
        Est_id: $('#cmbEstablecimientos').val(),
    };
    $.ajax({
        url: window.urlGuardarUsuario,
        type: 'POST',
        data: { entity: strParams, idsRoles: textoArreglo },
        success: function (data) {
            if (data === 'exito') {
                /*$('#DivMessajeErrorGeneral').addClass("hidden");*/
                $('#msjExito').removeClass("hidden");
                setTimeout(() => { location.reload(); }, 2000);
            }
            else {
                $('#btnGuardar').removeClass('loading');
                $('#btnGuardar').removeClass('disabled');
                $('#btnSalir').removeClass('loading');
                $('#btnSalir').removeClass('disabled');
                $('#msjError').removeClass("hidden");
            }
        },

        error: function () {
            showMessage('#divMensajePublicacionViaje', 'danger', 'Ocurrió un error al guardar el usuario. Por favor intente nuevamente.');
            //hideLoading();
        }
    });

}
function ObtenerUsuario(id) {
    $('#idUsuario').val(id);

    id = $('#idUsuario').val();
    $.ajax({
        url: window.urlObtenerUsuario,
        type: 'POST',
        data: { id: id },
        success: function (data) {
            $('#txtNombre').val(data.Nombre);
            $('#txtCorreo').val(data.Correo);
            $('#txtusuario').val(data.Username);
            $('#txtContraseña').val(data.Password);
            $("#cmbEstablecimientos").dropdown('set selected', data.Est_id);

            data.ListaRoles.forEach(function (element) {
                $("#cmbRoles").dropdown('set selected', element.Id);
            });

        },

        error: function () {
            alert('Error al cargar el usuario seleccionado');
        }
    });

    _idUsuario = id;
}
function LimpiaEstilos() {
    //Limpio el estilo Error antes de validar
    $('#divtxtNombre').removeClass("error");
    $('#divtxtCorreo').removeClass("error");
    $('#divtxtusuario').removeClass("error");
    $('#divtxtContraseña').removeClass("error");
    $('#divcmbEstablecimientos').removeClass('error');
    $('#divcmbRoles').removeClass('error');
}
function ValidaGuardar() {
    var esValido = true;
    var errores = [];

    LimpiaEstilos();


   
    
    if ($('#txtNombre').val() === '') {
        $('#divtxtNombre').addClass("error");
        errores.push('Debe indicar el nombre');
    }
    if ($('#txtCorreo').val() === '') {
        $('#divtxtCorreo').addClass("error");
        errores.push('Debe indicar el correo');
    }
    if ($('#txtusuario').val() === '') {
        $('#divtxtusuario').addClass("error");
        errores.push('Debe indicar el usuario');
    }
    if ($('#txtContraseña').val() === '') {
        $('#divtxtContraseña').addClass("error");
        errores.push('Debe indicar la contraseña');
    }
    if ($('#cmbEstablecimientos').val() < 1) {
        $('#divcmbEstablecimientos').addClass("error");
        errores.push('Debe seleccionar al menos un establecimiento');
    }
    if ($('#cmbRoles').val() < 1) {
        $('#divcmbRoles').addClass("error");
        errores.push('Debe seleccionar al menos un rol');
    }

    if (errores.length > 0) {
        var mensaje = '';
        $('#DivMessajeErrorGeneral').removeClass("hidden");

        for (i = 0; i < errores.length; i++) {
            mensaje += '<li>' + errores[i] + '</li>';
        }

        mensaje += '</ul>';
        $('#listMessajeError').empty();

        $('#listMessajeError').prepend(mensaje);

        // showMessage('#divMensajeNuevoCamion', 'danger', mensaje);
        return false;
    }
    else {
        return true;
    }


}
function PreparaEliminarUsuario(id) {
    $('#idUsuario').val(id);

}
function EliminaUsuario() {
    $('#btnEliminar').addClass('loading');
    $('#btnEliminar').addClass('disabled');

    id = $('#idUsuario').val();
    $.ajax({
        url: window.urlEliminarUsuario,
        type: 'POST',
        data: { id: id },
        success: function (data) {
            if (data === 'exito') {
                $('#divConsultaElimina').addClass("hidden");
                $('#divExitoElimina').removeClass("hidden");
                setTimeout(() => { location.reload(); }, 2000);
            }
        },
        error: function (data) {
            console.log(data);
            showMessage('body', 'danger', 'Ocurrió un error al eliminar el usuario seleccionado.' + data);
        }
    });
}