var _id = 0;
var _arregloArticulos = [];

$(function () {
    CargaProductos();
    CargaPrioridad();
    ObtenerEstado();
});

function ObtenerEstado() {

    $.ajax({
        url: window.urlObtenerEstado,
        type: 'POST',
        success: function (data) {
            $('#cmbEstado').empty();
            $('#cmbEstado').append('<option value="-1">[Seleccione estado]</option>')

            var contador = 0;
            $.each(data,
                function (value, item) {
                    var texto = '<option value="' + item.Id + '">' + item.Estado_Solicitud + '</option>';
                    $('#cmbEstado').append(texto);
                    contador++;
                }
            );


            //alert(contador);
            // hideLoading();
        },
        error: function () {
            // showMessage('body', 'danger', 'Ocurrió un error al listar las marcas de camiones.');
            //hideLoading();
        }
    });
}
function CargaPrioridad() {

    $.ajax({
        url: window.urlObtenerPrioridad,
        type: 'POST',
        success: function (data) {
            $('#cmbPrioridad').empty();
            $('#cmbPrioridad').append('<option value="-1">[Seleccione prioridad]</option>')

            var contador = 0;
            $.each(data,
                function (value, item) {
                    var texto = '<option value="' + item.Id + '">' + item.Descripcion + '</option>';
                    $('#cmbPrioridad').append(texto);
                    contador++;
                }
            );


            //alert(contador);
            // hideLoading();
        },
        error: function () {
            // showMessage('body', 'danger', 'Ocurrió un error al listar las marcas de camiones.');
            //hideLoading();
        }
    });
}
function CargaProductos() {

    $.ajax({
        url: window.urlObtenerProductos,
        type: 'POST',
        success: function (data) {
            $('#cmbArticulo').dropdown('clear');
            $('#cmbArticulo').empty();
            $('#cmbArticulo').append('<option value="-1">[Seleccione artículo]</option>');
            var contador = 0;
            $.each(data,
                function (value, item) {
                    var texto = '<option value="' + item.Id + '">' + item.Descripcion + '</option>';
                    $('#cmbArticulo').append(texto);
                    contador++;
                }
            );


            //alert(contador);
            // hideLoading();
        },
        error: function () {
            alert('Error al cargar los productos existentes');
        }
    });

}
function ObtenerSolicitud(id) {
    $('#idSolicitudSeleccionada').val(id);

    id = $('#idSolicitudSeleccionada').val();
    $.ajax({
        url: window.urlObtenerSolicitud,
        type: 'POST',
        data: { id: id },
        success: function (data) {
            $('#txtFechaIngreso').val(data.FechaMostrar)
            $('#txtFolio').val(data.Folio);
            $("#cmbPrioridad").dropdown('set selected', data.Prioridad_Id);
            $("#cmbEstado").dropdown('set selected', data.Estado_Id);
            $('#txtObservacion').val(data.Observacion_Solicitud);
            ObtenerDetalleSolicitud();
        },

        error: function () {
            alert('Error al cargar la solicitud seleccionada');
        }
    });
    _id = id;
}
function ObtenerDetalleSolicitud() {


    var entity = {
        Solicitud_Id: _id,
    }

    

    $.ajax({
        url: window.urlObtenerDetalleSolicitud,
        type: 'POST',
        data: { entity: entity },
        success: function (data) {
            _arregloArticulos = data;
           
            ActualizaGrid();
        },
        error: function () {
            showMessage('#divMensajePublicacionViaje', 'danger', 'Ocurrió un error al guardar la información. Por favor intente nuevamente.');
            //hideLoading();
        }
    });

}
function Eliminar(indice, Producto_Id) {

    var DetalleProducto = {
        Producto_Id: Producto_Id,
        Indice: indice,
    };

    $.ajax({
        url: window.urlQuitarProducto,
        type: 'POST',
        data: { entity: DetalleProducto },
        success: function (data) {
            _arregloArticulos = data;
            ActualizaGrid();

        },
        error: function (ex) {
            alert('Error al eliminar el producto');
        }
    });
}
function ActualizaGrid() {
    var html = GridEncabezado();
    var indice = 0;
    _arregloArticulos.forEach(function (element) {
        html = html + '<tr>';
        html = html + '<td><button class="ui red icon button" onclick="Eliminar(' + indice + ',' + element.Producto_Id + ');" ><i class="trash icon"></i></button></td>';
        html = html + '<td>' + element.ProductoStr + '</td>';
        html = html + '<td>' + element.Cantidad + '</td>';
        html = html + '<td>' + element.Observacion + '</td>';
        html = html + '</tr>';
        indice++;
    });

    html = html + GridPie();
    $('#grdArticulos').html(html);
}
function GridEncabezado() {
    var encabezado = '<table id="grdDatos" class="ui inverted table table-striped table-bordered">';
    encabezado = encabezado + '<thead><tr><th style="width:10%">Op</th><th style="width:50%">Artículo</th><th style="width:10%">Cantidad</th><th style="width:30%">Observacion</th></tr></thead>';
    return encabezado;
}
function GridPie() {
    var pie = '</table>';
    return pie;
}
function GuardarSolicitud() {

    //if (ValidaGeneraVenta() == false)

    //    return;

    $('#btnGuardar').addClass('loading');
    $('#btnGuardar').addClass('disabled');

    $('#btnSalir').addClass('loading');
    $('#btnSalir').addClass('disabled');

    var strParams = {
        Id: _id,
        Fecha_Ingreso: $('#txtFechaIngreso').val(),
        Folio: $('#txtFolio').val(),
        Prioridad_Id: $('#cmbPrioridad').val(),
        Estado_Id: $('#cmbEstado').val(),
        Observacion_Solicitud: $('#txtObservacion').val(),
    };

    $.ajax({
        url: window.urlInsertarSolicitud,
        type: 'POST',
        data: { entity: strParams },
        success: function (data) {
            if (data != 'error') {
                $('#msjExito').removeClass("hidden");
                window.location.href = data;
                setTimeout(() => { window.location.href = '/LibroSolicitudes?limpiar=1' }, 2000);
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
            alert('Error al generar la solicitud');
        }
    });

}
function AgregarProducto() {

    //if (ValidaAgregar() == false)

    //    return;

    var DetalleProducto = {
        Producto_Id: $('#cmbArticulo').val(),
        ProductoStr: $('#cmbArticulo').dropdown('get text'),
        Cantidad: $('#txtCantidad').val(),
        Observacion: $('#txtObservacionArticulo').val(),
        ProductoNuevo: true,
    };

    $.ajax({
        url: window.urlAgregarProducto,
        type: 'POST',
        data: { entity: DetalleProducto },
        success: function (data) {
            _arregloArticulos.push(DetalleProducto);
            ActualizaGrid();

            CargaProductos();
            $('#txtCantidad').val('0');
            $('#txtObservacionArticulo').val('');
        },
        error: function (ex) {
            alert('Error al guardar el producto');
        }
    });

}
function BusquedaFiltro() {
    $('#btnBuscarFiltro').addClass("loading");
    $('#btnBuscarFiltro').addClass("disabled");

    $('#dimmer').dimmer('show');

    var entity = {
        Desde: $('#txtFiltroDesde').val(),
        Hasta: $('#txtFiltroHasta').val(),
    }
    $.ajax({
        url: window.urlBusquedaFiltro,
        type: 'POST',
        data: { entity: entity },
        success: function (data) {

            window.location.href = '/LibroSolicitudes';

        },
        error: function () {
            showMessage('#divMensajePublicacionViaje', 'danger', 'Ocurrió un error al guardar la información. Por favor intente nuevamente.');
            //hideLoading();
        }
    });

}