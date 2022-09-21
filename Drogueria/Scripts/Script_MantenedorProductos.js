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