var _arregloArticulos = [];

$(function () {
    var input = document.getElementById("txtNumeroEtiqueta");

    
    input.addEventListener("keypress", function (event) {    
        if (event.key === "Enter") {            
            event.preventDefault();            
            ObtenerEtiqueta();
        }
    });
});



function ObtenerEtiqueta() {

    var numero = $('#txtNumeroEtiqueta').val();

    $.ajax({
        url: window.urlObtenerEtiqueta,
        data: { numero: numero },
        type: 'POST',
        success: function (data) {
            $('#txtArticulo').val(data.Articulo);
            $('#txtLote').val(data.Lote);
            $('#txtCantidad').val(data.Cantidad);

            var txtCantidadSolicitada = "#txt_cantidad_" + data.ProdId;
            var txtPistoleado = "#txt_pistoleado_" + data.ProdId;
            var txtSaldo = "#txt_saldo_" + data.ProdId;

            var cantidadSolicitada = parseInt($(txtCantidadSolicitada).html());
            var cantidadPistoleada = $(txtPistoleado).html();
            var nuevaCantidad = parseInt(cantidadPistoleada) + data.Cantidad;
            var cantidadRestante = cantidadSolicitada - nuevaCantidad;
            
            
            $(txtPistoleado).html(nuevaCantidad);
            $(txtSaldo).html(cantidadRestante);

            _arregloArticulos.push(data);
            ActualizaGrid();

            //Limpiar
            $('#txtNumeroEtiqueta').val("");
            $('#txtArticulo').val("");
            $('#txtLote').val("");
            $('#txtCantidad').val("");
            document.getElementById("txtNumeroEtiqueta").focus();
            
        },
        error: function () {
            // showMessage('body', 'danger', 'Ocurrió un error al listar las marcas de camiones.');
            //hideLoading();
        }
    });
}



function ActualizaGrid() {
    var html = GridEncabezado();
    var indice = 0;
    _arregloArticulos.forEach(function (element) {
        html = html + '<tr>';
        html = html + '<td>' + element.Articulo + '</td>';
        html = html + '<td>' + element.Numero + '</td>';
        html = html + '<td>' + element.Lote + '</td>';
        html = html + '<td>' + element.FechaVencimientoStr + '</td>';
        html = html + '<td>' + element.Cantidad + '</td>';
        html = html + '</tr>';
        indice++;
    });

    html = html + GridPie();
    $('#divLote').html(html);
}
function GridEncabezado() {
    var encabezado = '<table id="grdDatos" class="ui inverted table table-striped table-bordered">';
    encabezado = encabezado + '<thead><th style="width:50%">Artículo</th><th style="width:10%">Etiqueta</th><th style="width:10%">Lote</th><th style="width:10%">Vencimiento</th><th style="width:10%">Cantidad</th></tr></thead>';
    return encabezado;
}
function GridPie() {
    var pie = '</table>';
    return pie;
}