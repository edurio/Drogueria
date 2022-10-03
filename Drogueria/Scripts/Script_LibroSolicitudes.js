var _id = 0;
var _arregloArticulos = [];

$(function () {
   // CargaProductos();
    /*CargaPrioridad();*/
    /*ObtenerEstado();*/
    CargaTipo();
    ObtenerClase();
    ObtenerProductosCombo();

    $('#cmbClase').change(function () { CargaProductos();  });
});


function CargaTipo() {
    $('#cmbTipo').empty();
    $('#cmbTipo').append('<option value="-1">[tipo]</option>');
    $('#cmbTipo').append('<option value="1">Mensual</option>');
    $('#cmbTipo').append('<option value="2">Crítico</option>');

    
}


function ObtenerClase() {

    $.ajax({
        url: window.urlObtenerClases,
        type: 'POST',
        success: function (data) {
            $('#cmbClase').empty();
            $('#cmbClase').append('<option value="-1">[Seleccione clase]</option>')

            var contador = 0;
            $.each(data,
                function (value, item) {
                    var texto = '<option value="' + item.Id + '">' + item.Descripcion + '</option>';
                    $('#cmbClase').append(texto);
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

function LimpiarSolicitud() {

    $.ajax({
        url: window.urlLimpiarSolicitud,
        type: 'POST',
        success: function (data) {
            location.reload();
        },
        error: function () {
            // showMessage('body', 'danger', 'Ocurrió un error al listar las marcas de camiones.');
            //hideLoading();
        }
    });
}

//function ObtenerEstado() {

//    $.ajax({
//        url: window.urlObtenerEstado,
//        type: 'POST',
//        success: function (data) {
//            $('#cmbEstado').empty();
//            $('#cmbEstado').append('<option value="-1">[seleccione estado]</option>')

//            var contador = 0;
//            $.each(data,
//                function (value, item) {
//                    var texto = '<option value="' + item.Id + '">' + item.Estado_Solicitud + '</option>';
//                    $('#cmbEstado').append(texto);
//                    contador++;
//                }
//            );


//            //alert(contador);
//            // hideLoading();
//        },
//        error: function () {
//            // showMessage('body', 'danger', 'Ocurrió un error al listar las marcas de camiones.');
//            //hideLoading();
//        }
//    });
//}

//function CargaPrioridad() {

//    $.ajax({
//        url: window.urlObtenerPrioridad,
//        type: 'POST',
//        success: function (data) {
//            $('#cmbPrioridad').empty();
//            $('#cmbPrioridad').append('<option value="-1">[prioridad]</option>')

//            var contador = 0;
//            $.each(data,
//                function (value, item) {
//                    var texto = '<option value="' + item.Id + '">' + item.Descripcion + '</option>';
//                    $('#cmbPrioridad').append(texto);
//                    contador++;
//                }
//            );


//            //alert(contador);
//            // hideLoading();
//        },
//        error: function () {
//            // showMessage('body', 'danger', 'Ocurrió un error al listar las marcas de camiones.');
//            //hideLoading();
//        }
//    });
//}

function CargaProductos() {
    $('#dimmerCargando').modal('show');
    var id = $('#cmbClase').dropdown('get value');

    $.ajax({
        url: window.urlObtenerProductos,
        type: 'POST',
        data: { id: id },
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
            $('#dimmerCargando').modal('hide');
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
            $("#cmbEstado").dropdown('set selected', data.Estado_Id);
            $('#txtObservacion').val(data.Observacion_Solicitud);
            ObtenerDetalleSolicitud();

            if (data.Estado_Id == 1) {
                $("#btnGuardar").addClass("disabled");
                
            }
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

    if (ValidaGenerarSolicitud() == false)
        return;

    $('#btnGuardar').addClass('loading');
    $('#btnGuardar').addClass('disabled');

    $('#btnSalir').addClass('loading');
    $('#btnSalir').addClass('disabled');

    var strParams = {
        Id: _id,
        Fecha_Ingreso: $('#txtFechaIngreso').val(),
        Folio: $('#txtFolio').val(),
        Tipo_Id: $('#cmbTipo').val(),
        /*Estado_Id: $('#cmbEstado').val(),*/
        Observacion_Solicitud: $('#txtObservacion').val(),
    };

    $.ajax({
        url: window.urlInsertarSolicitud,
        type: 'POST',
        data: { entity: strParams },
        success: function (data) {
            if (data != 'error') {
                $('#msjExito').removeClass("hidden");
                window.open(data, "_blank");
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

   

    if (ValidaAgregar() == false)
        return;

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
function ValidaAgregar() {
    var errores = [];

    //LIMPIO LOS ESTILOS
    $('#divcmbClase').removeClass("error");
    $('#divcmbArticulo').removeClass("error");
    $('#divtxtCantidad').removeClass("error");
   
    //DIVS GENERICOS
    $('#DivMessajeErrorProducto').addClass("hidden");

    var cla = $('#cmbClase').val();
    if ($('#cmbClase').val() == null || $('#cmbClase').val() == -1) {
        $('#divcmbClase').addClass("error");
        errores.push('Debe indicar la clase de artículo');       
    }

    var art = $('#cmbArticulo').val();
    if ($('#cmbArticulo').val() == null || $('#cmbArticulo').val() == -1) {
        $('#divcmbArticulo').addClass("error");
        errores.push('Debe indicar el artículo');
    }

    var cant = $('#txtCantidad').val();
    if ($('#txtCantidad').val() <= 0) {
        $('#divtxtCantidad').addClass("error");
        errores.push('Debe indicar la cantidad a solicitar');
    }

    if (errores.length > 0) {
        var mensaje = '';
        //mensaje = mensaje + '<ul>';
        $('#DivMessajeErrorProducto').removeClass("hidden");
        //$.each(errores, function (index, item) {
        //    mensaje += '<li>' + item + '</li>';
        //});

        for (i = 0; i < errores.length; i++) {
            mensaje += '<li>' + errores[i] + '</li>';
        }

        mensaje += '</ul>';
        $('#listMessajeErrorProducto').empty();
        $('#listMessajeErrorProducto').prepend(mensaje);

        // showMessage('#divMensajeNuevoCamion', 'danger', mensaje);
        return false;
    }
    else {
        return true;
    }
}
function ValidaGenerarSolicitud() {
    var errores = [];

    //LIMPIO LOS ESTILOS
    $('#divcmbTipo').removeClass("error");
    /*$('#cmbPrioridad').removeClass("error");*/
    $('#divcmbEstado').removeClass("error");
    
   
    //DIVS GENERICOS
    $('#DivMessajeErrorGeneral').addClass("hidden");

   
    if ($('#cmbTipo').val() == null || $('#cmbTipo').val() == -1) {
        $('#divcmbTipo').addClass("error");
        errores.push('Debe indicar el tipo de solicitud');
    }

    //if ($('#cmbPrioridad').val() == null || $('#cmbPrioridad').val() == -1) {
    //    $('#divcmbPrioridad').addClass("error");
    //    errores.push('Debe indicar la prioridad solicitud');
    //}

    //if ($('#cmbEstado').val() == null || $('#cmbEstado').val() == -1) {
    //    $('#divcmbEstado').addClass("error");
    //    errores.push('Debe indicar el estado de la  solicitud');
    //}

    //if (_arregloArticulos == null || _arregloArticulos.length == 0) {      
    //    errores.push('Debe indicar agregar por lo menos un artículo');
    //}

    

    if (errores.length > 0) {
        var mensaje = '';
        //mensaje = mensaje + '<ul>';
        $('#DivMessajeErrorGeneral').removeClass("hidden");
        //$.each(errores, function (index, item) {
        //    mensaje += '<li>' + item + '</li>';
        //});

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
function PreparaEnviaSolicitud(id) {
    $('#idSolicitudSeleccionada').val(id);
}
function EnviaSolicitud() {

    $('#btnEnviar').addClass('loading');
    $('#btnEnviar').addClass('disabled');

    var idSolicitud = $('#idSolicitudSeleccionada').val();

    $.ajax({
        url: window.urlEnviarSolicitud,
        type: 'POST',
        data: { idSolicitud: idSolicitud },
        dataType: "json",
        success: function (resultado) {
            if (resultado == 'ok') {
                {
                    $('#divConsultaEnviar').addClass("hidden");
                    $('#divExitoEnviar').removeClass("hidden");
                    setTimeout(() => { location.reload(); }, 2000);
                }
            }
        },

        error: function () {
            showMessage('#divMensajePublicacionViaje', 'danger', 'Ocurrió un error al guardar la información. Por favor intente nuevamente.');
            //hideLoading();
        }
    });

}


function ObtenerProductosCombo() {

    $.ajax({
        url: window.urlObtenerProductos,
        type: 'POST',
        success: function (data) {
            $('#cmbProdDrogueria').dropdown('clear');
            $('#cmbProdDrogueria').empty();
            $('#cmbProdDrogueria').append('<option value="-1">[Seleccione producto]</option>');
            var contador = 0;
            $.each(data,
                function (value, item) {
                    var texto = '<option value="' + item.Id + '">' + item.Descripcion + " " + "(ID: " + (item.Id) + ")" + '</option>';
                    $('#cmbProdDrogueria').append(texto);
                }
            );
        },
        error: function () {
            alert(error.mensaje);
        }
    });

}

function PreparaRelacion(id, producto) {
    $("#idProductoSeleccionado").val(id);
    $("#txtNombreExterno").val(producto);
}


function GuardarRelacion() {

    var strParams = {
        Prod_Id_Drogueria: $('#cmbProdDrogueria').val(),
        Prod_Id_Externo: $('#idProductoSeleccionado').val(),
        Descripcion_Drogueria: $('#cmbProdDrogueria').dropdown('get text'),
        Descripcion_Externa: $("#txtNombreExterno").val()
    };

    $.ajax({
        url: window.urlGuardarRelacion,
        type: 'POST',
        data: { entity: strParams },
        success: function (data) {
            location.reload();

        },
        error: function (ex) {
            alert('Error al generar la solicitud');
        }
    });
}