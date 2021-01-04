//--------------------------------------------------ESCUCHADORES
$('.periodoaccion').click(function () {

    loadEjecucion($(this).attr("data-IdEjecucion"), "", "")
});
$('#EjecucionContenido').on('click', '.EjecucionEdit', function () {
    displayEditEjecucion($(this).attr("data-id"));
});

$('#EjecucionContenido').on('click', '.EjecucionVolver', function () {
    loadEjecucion($(this).attr("data-id"), "", "")
});

$('#EjecucionContenido').on('click', '.EjecucionSaveEdit', function () {
    EjecucionAlmacenarEdicion();
});

$('.button-open-indicador').click(function () {

    displayIndicadorDetails($(this).attr("data-id"), "", "");
});
$('#EjecucionContenido').on('click', '.IndicadorVolver', function () {

    displayIndicadorDetails($(this).attr("data-id"), "", "");
});

$('#EjecucionContenido').on('click', '.button-edit-indicador', function () {
    //alert("click");
    displayIndicadorEdit($(this).attr("data-id"));
});

$('#EjecucionContenido').on('click', '.IndicadorSaveEdit', function () {
    //alert("save");
    IndicadorAlmacenarEdicion();
});

//---------------------------------------------------- FUNCIONES
function loadEjecucion(id, tipo, mensaje) {
    $('#EjecucionContenido').empty();
    $('#myModalEjecucion').modal('show');
    $('#EjecucionContenido').append('<div class="loading"><img src="../iamge/loadingcircle.gif" ></div>');
    //alert("click");
    $.get("../ejecucions/DetailsPop", { 'id': id, 'tipo': tipo, 'mensaje': mensaje })
        .done(function (data) {
            //alert(tipo);
            //alert(mensaje);
            $('#EjecucionContenido').empty();
            $('#EjecucionContenido').append(data);

        });


}


function displayEditEjecucion(id, tipo, mensaje) {
    $('#EjecucionContenido').empty();
    $('#myModalEjecucion').modal('show');
    $('#EjecucionContenido').append('<div class="loading"><img src="../iamge/loadingcircle.gif" ></div>');
    $.get("../ejecucions/EditPop", { 'id': id })
        .done(function (data) {
            //alert(tipo);
            //alert(mensaje);

            $('#EjecucionContenido').empty();
            $('#EjecucionContenido').append(data);

        });


}


//-------------------------------- AJAX PARA ALMACENAR LA EDICION DE EJECUCION
function EjecucionAlmacenarEdicion() {
    //alert("click");
    var idEjecucion = $('#id').val();
    //alert(idEjecucion);
    $.post("../ejecucions/Editpop", $('#EjecucionEditForm').serialize())
        .done(function (data) {
            //alert(data);
            $('#notas').empty();
            if (data === "True") {
                loadEjecucion(idEjecucion, "exito", "Ejecución editada exitosamente");
            }
            else {

                loadEjecucion(idEjecucion, "error", "No se pudo actualizar la Ejecución");
            }

        });

}


//-----------------------------AJAXA POST PARA CARGAR ARCHIVOS
$('#EjecucionContenido').on('change', '#fileuploadtextEjecucion', function () {
    /*alert("change");
    $('#uploadFileFormEjecucion').submit();
});

$('#EjecucionContenido').on('submit', '#uploadFileFormEjecucion', function (d) {

    d.preventDefault();
    */
    var formulario = $(this).closest('form');

    // Serialize your form
    var formData = new FormData(formulario[0]);
    var dt = new Date();
    var time = dt.getFullYear() + "-" + dt.getMonth() + "-" + dt.getDay() + "-" + dt.getHours() + "-" + dt.getMinutes() + "-" + dt.getSeconds() + "e-" + $('#id').val();

    //alert(time);
    formData.append("id", time);


    $('#urlAdjuntoEjecucion').empty();
    $('.uploading').empty();
    $('.uploading').append('cargando <img src="../images/ajax-loader.gif">');

    //alert(formData.stringify());
    $.ajax({
        type: "POST",
        url: '../Upload/UploadFile',
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (response) {

            //var respuesta = JSON.parse(response);
            $('#adjunto').val(response.Nombre);
            $('#urlAdjuntoEjecucion').attr("href", "Upload/UploadedFiles/" + response.Nombre)
            $('.uploading').empty();
            $('#urlAdjuntoEjecucion').empty();
            $('#urlAdjuntoEjecucion').append(response.Nombre);

        },
        error: function (error) {
            $('.uploading').empty();
            $('.uploading').append("Imposible cargar el archivo");
            //alert("errror al cargar el archivo");
        }
    });

});

// detalles de un indicador
function displayIndicadorDetails(id, tipo, mensaje) {
    $('#EjecucionContenido').empty();
    $('#myModalEjecucion').modal('show');
    $('#EjecucionContenido').append('<div class="loading"><img src="../iamge/loadingcircle.gif" ></div>');
    $.get("../Indicadors/DetailsPop", { 'id': id, 'tipo': tipo, 'mensaje': mensaje })
        .done(function (data) {

            //alert(mensaje);

            $('#EjecucionContenido').empty();
            $('#EjecucionContenido').append(data);

        });


}

// Edicion de un indicador
function displayIndicadorEdit(id) {
    $('#EjecucionContenido').empty();
    $('#myModalEjecucion').modal('show');
    $('#EjecucionContenido').append('<div class="loading"><img src="../iamge/loadingcircle.gif" ></div>');
    $.get("../Indicadors/EditPop", { 'id': id })
        .done(function (data) {

            //alert(mensaje);

            $('#EjecucionContenido').empty();
            $('#EjecucionContenido').append(data);

        });


}
//-------------------------------- AJAX PARA ALMACENAR LA EDICION DE INDICADOR
function IndicadorAlmacenarEdicion() {
    //alert("save2");
    var idIndicador = $('#id').val();
    //alert(idEjecucion);
    $.post("../Indicadors/EditPop", $('#IndicadorEditForm').serialize())
        .done(function (data) {
            //alert(data);
            $('#notas').empty();
            if (data === "True") {

                displayIndicadorDetails(idIndicador, "exito", "Indicador editado exitosamente");

            }
            else {
                displayIndicadorDetails(idIndicador, "error", "No se pudo actualizar el Indicador");

            }

        });

}

