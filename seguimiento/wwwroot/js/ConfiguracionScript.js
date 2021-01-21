//-----------------------------AJAXA POST PARA CARGAR ARCHIVOS DESDEME MUENU CONFIGURACION
var currenthost;
$('#EjecucionContenidoConfig').on('change', '#fileuploadtextEjecucion', function () {

    var formulario = $(this).closest('form');

    // Serialize your form
    var formData = new FormData(formulario[0]);
    var dt = new Date();
    var idInd = $('#id').val() !== undefined ? "ind-" + $('#id').val() : "ind-0";
    var time = dt.getFullYear() + "-" + dt.getMonth() + "-" + dt.getDay() + "-" + dt.getHours() + "-" + dt.getMinutes() + "-" + dt.getSeconds() + idInd;
    formData.append("id", time);
    

    $('#urlAdjuntoEjecucion').empty();
    $('.uploading').html('Cargando <img src="/images/ajax-loader.gif">');

    //alert(formData.stringify());
    $.ajax({
        type: "POST",
        url: formulario.attr('action'),
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (response) {

            //var respuesta = JSON.parse(response);
            if (response.loaded == true) {
                $('#Adjunto').val(response.nombre);
                $('.uploading').empty();
                $('#urlAdjuntoEjecucion').attr("href", currenthost + "/UploadedFiles/" + response.nombre)
                $('#urlAdjuntoEjecucion').html(response.nombre);
            }
            else {
                $('.uploading').html("Imposible cargar el archivo.");
            }
        },
        error: function (error) {
            $('.uploading').html("Error al cargar el archivo.");
        }
    });

});


//BLOQUE DE LOS CAMPOS CATEGORIA E INDICADOR EN LA CREACION Y EDICION DE EVALUACIONES

$('#Contexto').change(function () {
    var contexto = $(this).val();
    //alert("loaded");
    lock(contexto);

});

$(document).ready(function () {

    currenthost = location.protocol+ '//' + location.host + '/';

    if ($('#Contexto').length > 0) {
        var contexto = $('#Contexto').val();
        //alert("loaded");
        lock(contexto);
    }
});

function lock(contexto) {
    //alert(contexto);
    if (contexto === "Global") {
        $('#Indicador').prop('disabled', true);
        $('#Categoria').prop('disabled', true);
    } else if (contexto === "Categoria") {
        $('#Indicador').prop('disabled', true);
        $('#Categoria').prop('disabled', false);
    } else if (contexto === "Indicador") {
        $('#Indicador').prop('disabled', false);
        $('#Categoria').prop('disabled', true);
    }
}