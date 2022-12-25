var _id = 0;

$(function () {
   
    ObtenerProductos();
    ObtenerProductoExterno();
});

function ObtenerProductos() {

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
            alert('Error al cargar los productos existentes');
        }
    });

}
function ObtenerProductoExterno() {

    $.ajax({
        url: window.urlObtenerProductoExterno,
        type: 'POST',
        success: function (data) {
            $('#cmbProdExterno').dropdown('clear');
            $('#cmbProdExterno').empty();
            $('#cmbProdExterno').append('<option value="-1">[Seleccione producto]</option>');
            var contador = 0;
            $.each(data,
                function (value, item) {
                    var texto = '<option value="' + item.Id_Externo + '">' + item.Descripcion + " " + "(ID: " + (item.Id_Externo) + ")" + '</option>';
                    $('#cmbProdExterno').append(texto);
                }
            );
        },
        error: function () {
            alert('Error al cargar los productos del establecimiento existentes');
        }
    });

}
function GuardarRelacionProducto() {

    //if (ValidaGuardar() === false) {
    //    //alert('no valido');
    //    return;
    //}

    $('#btnGuardar').addClass('loading');
    $('#btnGuardar').addClass('disabled');

    var strParams = {
        Id: _id,
        Prod_Id_Drogueria: $('#cmbProdDrogueria').val(),
        Descripcion_Drogueria: $('#cmbProdDrogueria').dropdown('get text'),
        Prod_Id_Externo: $('#cmbProdExterno').val(),
        Descripcion_Externa: $('#cmbProdExterno').dropdown('get text'),
    };

    $.ajax({
        url: window.urlInsertarRelacionProducto,
        type: 'POST',
        data: { entity: strParams },
        success: function (data) {
            if (data === 'exito') {
                $('#DivMessajeErrorGeneral').addClass("hidden");
                $('#msjExito').removeClass("hidden");
                setTimeout(() => { window.location.href = '/MantenedorRelacionProductos' }, 1000);
            }
            //if (data === 'error') {
            //    $('#divErroLogin').removeClass("hidden");
            //}

        },
        error: function (ex) {
            alert('Error al guardar el producto');
        }
    });

}

function ObtenerProductosRelacionados(id) {
    $('#idRelacion').val(id);

    id = $('#idRelacion').val();
    $.ajax({
        url: window.urlObtenerProductosRelacionados,
        type: 'POST',
        data: { id: id },
        success: function (data) {
            $("#cmbProdDrogueria").dropdown('set selected', data.Prod_Id_Drogueria);
            $("#cmbProdExterno").dropdown('set selected', data.Prod_Id_Externo);
            
        },

        error: function () {
            alert('Error al cargar el usuario seleccionado');
        }
    });
    _id = id;
}

