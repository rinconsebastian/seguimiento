//-----------------------------AJAXA POST PARA CARGAR ARCHIVOS DESDEME MUENU CONFIGURACION
$('#EjecucionContenidoConfig').on('change', '#fileuploadtextEjecucion', function () {
    //alert("change");
    var formulario = $(this).closest('form');

    // Serialize your form
    var formData = new FormData(formulario[0]);
    var dt = new Date();
    var time = dt.getFullYear() + "-" + dt.getMonth() + "-" + dt.getDay() + "-" + dt.getHours() + "-" + dt.getMinutes() + "-" + dt.getSeconds() + "ind-" + $('#id').val();

    //alert(time);
    formData.append("id", time);

    $('#urlAdjuntoEjecucion').empty();
    $('.uploading').empty();
    $('.uploading').append('cargando <img src="../../Content/images/ajax-loader.gif">');


    //alert(formData.stringify());
    $.ajax({
        type: "POST",
        url: '../../Upload/UploadFile',
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (response) {

            //var respuesta = JSON.parse(response);
            $('#adjunto').val(response.Nombre);
            $('#urlAdjuntoEjecucion').attr("href", "../../Upload/UploadedFiles/" + response.Nombre)
            $('.uploading').empty();
            $('#urlAdjuntoEjecucion').empty();
            $('#urlAdjuntoEjecucion').append(response.Nombre);

        },
        error: function (error) {
            $('.uploading').empty();
            $('.uploading').append("Imposible cargar el archivo");
            //alert("errror al cargar el archivo");
            //alert("errror al cargar el archivo");
        }
    });

});

$('#submitEditIndicador').click(function () {
    $('#formIndicador').submit();

})

//BLOQUE DE LOS CAMPOS CATEGORIA E INDICADOR EN LA CREACION Y EDICION DE EVALUACIONES

$('#Contexto').change(function () {
    var contexto = $(this).val();
    //alert("loaded");
    lock(contexto);

});

$(document).ready(function () {

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