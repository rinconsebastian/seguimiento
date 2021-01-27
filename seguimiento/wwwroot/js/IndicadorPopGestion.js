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
    $('#EjecucionContenido').append('<div class="loading"><img src="../images/loadingcircle.gif" ></div>');
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
    $('#EjecucionContenido').append('<div class="loading"><img src="../images/loadingcircle.gif" ></div>');
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
            
            if (data === true) {
               
                loadEjecucion(idEjecucion, "exito", "Ejecución editada exitosamente");
            }
            else {
               
                loadEjecucion(idEjecucion, "error", "No se pudo actualizar la Ejecución");
            }

        });

}


// detalles de un indicador
function displayIndicadorDetails(id, tipo, mensaje) {
    $('#EjecucionContenido').empty();
    $('#myModalEjecucion').modal('show');
    $('#EjecucionContenido').append('<div class="loading"><img src="../images/loadingcircle.gif" ></div>');
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
    $('#EjecucionContenido').append('<div class="loading"><img src="/images/loadingcircle.gif" ></div>');
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
            if (data === true) {

                displayIndicadorDetails(idIndicador, "exito", "Indicador editado exitosamente");

            }
            else {
                displayIndicadorDetails(idIndicador, "error", "No se pudo actualizar el Indicador");

            }

        });

}

