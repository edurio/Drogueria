var _arregloArticulos = [];

$(function () {
    CargaProductos();
    CargaPrioridad();
});

function CargaPrioridad() {

    $.ajax({
        url: window.urlObtenerPrioridad,
        type: 'POST',
        success: function (data) {
            $('#cmbArticulo').empty();
            $('#cmbArticulo').append('<option value="-1">[Seleccione prioridad]</option>')

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
            $('#cmbArticulo').empty();
            $('#cmbArticulo').append('<option value="-1">[Seleccione artículo]</option>')

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
            // showMessage('body', 'danger', 'Ocurrió un error al listar las marcas de camiones.');
            //hideLoading();
        }
    });
}


function AgregarArticulo() {
    var articulo = { idArticulo: $('#cmbArticulo').val(), descripcion: $('#cmbArticulo').dropdown('get text'), cantidad: $('#txtCantidad').val(), observacion: $('#txtObservacionArticulo').val() };  
    _arregloArticulos.push(articulo);

    ActualizaGrid();

    $('#grdArticulos').dropdown('clear');
    $('#txtCantidad').val('0');
    $('#txtObservacionArticulo').val('');
}

function ActualizaGrid() {
    var html = GridEncabezado();

    _arregloArticulos.forEach(function (element) {
        html = html + '<tr>';
        html = html + '<td></td>';
        html = html + '<td>' + element.descripcion + '</td>';                
        html = html + '<td>' + element.cantidad + '</td>';       
        html = html + '<td>' + element.observacion + '</td>';                
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