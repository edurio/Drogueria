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
            
        },
        error: function () {
            // showMessage('body', 'danger', 'Ocurrió un error al listar las marcas de camiones.');
            //hideLoading();
        }
    });
}