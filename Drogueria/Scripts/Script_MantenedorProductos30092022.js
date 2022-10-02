var _id = 0;

$(function () {
   
    ObtenerUnidades();
    ObtenerEstablecimiento();
});

function ObtenerUnidades() {

    $.ajax({
        url: window.urlObtenerUnidades,
        type: 'POST',
        success: function (data) {

            $('#cmbUnidad').empty();
            $('#cmbUnidad').append('<option value="-1">[Seleccione unidad]</option>')
            $.each(data,
                function (value, item) {
                    var texto = '<option value="' + item.Id + '">' + item.Descripcion + '</option>';
                    $('#cmbUnidad').append(texto);
                    
                }
            );
            
        },
        error: function () {
            showMessage('body', 'danger', 'Ocurrió un error al listar las unidades.');
            
        }
    });
}
function ObtenerEstablecimiento() {

    $.ajax({
        url: window.urlObtenerEstablecimiento,
        type: 'POST',
        success: function (data) {

            $('#cmbEstablecimiento').empty();
            $('#cmbEstablecimiento').append('<option value="-1">[Seleccione establecimiento]</option>')
            $.each(data,
                function (value, item) {
                    var texto = '<option value="' + item.Id + '">' + item.Descripcion + '</option>';
                    $('#cmbEstablecimiento').append(texto);

                }
            );

        },
        error: function () {
            showMessage('body', 'danger', 'Ocurrió un error al listar los establecimientos.');

        }
    });
}
function GuardarProductoExterno() {

    //if (ValidaGenerarSolicitud() == false)
    //    return;

    $('#btnGuardar').addClass('loading');
    $('#btnGuardar').addClass('disabled');

    $('#btnSalir').addClass('loading');
    $('#btnSalir').addClass('disabled');

    var strParams = {
        Id: _id,
        Id_Externo: $('#txtIdExterno').val(),
        Descripcion: $('#txtDescripcion').val(),
        FactorSeguridad: $('#txtFactorSeguridad').val(),
        Unid_Id: $('#cmbUnidad').val(),
    };

    $.ajax({
        url: window.urlInsertarProductoExterno,
        type: 'POST',
        data: { entity: strParams },
        success: function (data) {
            if (data === 'ok') {
                $('#msjExito').removeClass("hidden");
                setTimeout(() => { location.reload(); }, 1000);
            }
            if (data === 'error') {
                $('#msjError').removeClass("hidden");

                $('#btnGuardar').removeClass('loading');
                $('#btnGuardar').removeClass('disabled');

                $('#btnSalir').removeClass('loading');
                $('#btnSalir').removeClass('disabled');
            }

        },
        error: function (ex) {
            alert('Error al guardar');
        }
    });

}

function RecuperaProducto(id) {
    $('#idProducto').val(id);

    id = $('#idProducto').val();
    $.ajax({
        url: window.urlRecuperaProducto,
        type: 'POST',
        data: { id: id },
        success: function (data) {
            $('#txtIdExterno').val(data.Id_Externo);
            $('#txtDescripcion').val(data.Descripcion);
            $("#cmbUnidad").dropdown('set selected', data.Unid_Id);

        },

        error: function () {
            alert('Error al cargar el usuario seleccionado');
        }
    });

    _id = id;
}

