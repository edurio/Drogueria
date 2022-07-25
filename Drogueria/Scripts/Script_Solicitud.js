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

function AgregarProducto() {

    //if (ValidaAgregar() == false)

    //    return;

    var DetalleProducto = {
        Producto_Id: $('#cmbArticulo').val(),
        ProductoStr: $('#cmbArticulo').dropdown('get text'),
        Cantidad: $('#txtCantidad').val(),
        Observacion: $('#txtObservacionArticulo').val(),
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

function AgregarArticulo() {
    //var articulo = { idArticulo: $('#cmbArticulo').val(), descripcion: $('#cmbArticulo').dropdown('get text'), cantidad: $('#txtCantidad').val(), observacion: $('#txtObservacionArticulo').val() };  
    _arregloArticulos.push(DetalleProducto);

    

    
}

function ActualizaGrid() {
    var html = GridEncabezado();

    _arregloArticulos.forEach(function (element) {
        html = html + '<tr>';
        html = html + '<td></td>';
        html = html + '<td>' + element.ProductoStr + '</td>';
        html = html + '<td>' + element.Cantidad + '</td>';
        html = html + '<td>' + element.Observacion + '</td>';
        html = html + '</tr>';
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

    $('#btnLimpiar').addClass('loading');
    $('#btnLimpiar').addClass('disabled');

    var strParams = {
        Fecha_Ingreso: $('#txtFechaIngreso').val(),
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
                setTimeout(() => { window.location.href = '/Solicitudes' }, 2000);
            }
            if (data === 'error') {
                $('#msjError').removeClass("hidden");
                $('#btnAgregarProducto').addClass('disabled');
                $('#btnGeneraVenta').addClass('disabled');
                $('#btnVolver').removeClass('disabled');
                $('#btnVolver').removeClass('loading');
                $('#btnLimpiar').removeClass('disabled');
                $('#btnLimpiar').removeClass('loading');
            }

        },
        error: function (ex) {
            alert('Error al generar la solicitud');
        }
    });

}