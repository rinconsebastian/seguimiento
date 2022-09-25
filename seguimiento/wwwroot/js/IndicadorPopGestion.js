categ = "";
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
    $('#myModalEjecucion .modal-dialog').removeClass('modal-700');
    $('#myModalEjecucion .modal-dialog').addClass('modal-md');
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
    $('#myModalEjecucion .modal-dialog').removeClass('modal-md');
    $('#myModalEjecucion .modal-dialog').addClass('modal-700');
    $('#EjecucionContenido').append('<div class="loading"><img src="../images/loadingcircle.gif" ></div>');
    $.get("../ejecucions/EditPop", { 'id': id })
        .done(function (data) {
            //alert(tipo);
            //alert(mensaje);
            $('#EjecucionContenido').html(data);
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

//NOTAS INDICADORES

$(".AbrirNotasIndicador").click(function () {
    //alert("open");

    loadNotasIndicador($(this).attr("data-indicador"), "", "", $(this).attr("data-nombre"));
});
$('#myModalNotas').on('click', '.NotasIndicadorVolver', function () {
    loadNotasIndicador($(this).attr("data-indicador"));
});

$('#myModalNotas').on('click', '.NotaIndicadorEdit', function () {
    // alert("clic");
    $('#notas').empty();
    $('#notas').append('<div class="loading"><img src="images/loadingcircle.gif" ></div>');
    $('#myModalNotas').modal('show');
    $.get("../NotasIndicador/Editpop/", {
        id: $(this).attr("data-id"), idindicador: $(this).attr("data-indicador")
    })
        .done(function (data) {
            //   alert(data);
            $('#notas').empty();
            $('#notas').append(data);




        });

});

//-------------------------------- AJAX PARA ALMACENAR LA EDICION DE NOTAS
$('#myModalNotas').on('click', '.NotaIndicadorSaveEdit', function () {
    //alert("click");
    categ = $(this).attr("data-indicador");
    $.post("../NotasIndicador/Editpop", $('#NotaIndicadorEditForm').serialize())
        .done(function (data) {
            //alert(data);
            $('#notasIndicado').empty();
            if (data == true) {
                loadNotasIndicador(categ, "exito", "Nota editada exitosamente");
            }
            else {

                loadNotasIndicador(categ, "error", "No se pudo actualizar la nota");
            }

        });

});
// ------------------------ AJAX GET FORMULARIO NOTA NUEVA
$('#myModalNotas').on('click', '.NotaIndicadorCreate', function () {
    $('#notas').empty();
    $('#notas').append('<div class="loading"><img src="images/loadingcircle.gif" ></div>');
    $('#myModalNotas').modal('show');

    $.get("../NotasIndicador/Createpop/" + $(this).attr("data-indicador"))
        .done(function (data) {
            //   alert(data);
            $('#notas').empty();
            $('#notas').append(data);
        });

});


//-----------------------------AJAXA POST PARA ALMACENAR LAS NUEVAS NOTAS CREADAS
$('#myModalNotas').on('click', '.NotaIndicadorCreateButton', function () {
    categ = $(this).attr("data-indicador");
    //alert("click");
    $("#idIndicadorNotas").val(categ);
    $.post("../NotasIndicador/Createpop", $('#NotaIndicadorCreateForm').serialize())
        .done(function (data) {
            //alert(data);
            $('#notas').empty();
            if (data == true) {
              //  alert($("#idIndicadorNotas").val());
                loadNotasIndicador($("#idIndicadorNotas").val(), "exito", "Nota creada con exito");
            }
            else {

                loadNotasIndicador($("#idIndicadorNotas").val(), "error", "No se pudo Crear la nota");
            }

        });

});

// ------------------------ AJAX GET DETALLE NOTA 
$('#myModalNotas').on('click', '.NotaIndicadorDetalle', function () {
    $('#notas').empty();
    $('#notas').append('<div class="loading"><img src="images/loadingcircle.gif" ></div>');
    $('#myModalNotas').modal('show');

    $.get("../NotasIndicador/Detailspop/" + $(this).attr("data-id"))
        .done(function (data) {
            //   alert(data);
            $('#notas').empty();
            $('#notas').append(data);
        });

});

// ------------------------ AJAX GET DELETE NOTA 
$('#myModalNotas').on('click', '.NotaIndicadorBorrar', function () {
    $('#notas').empty();
    $('#notas').append('<div class="loading"><img src="images/loadingcircle.gif" ></div>');
    $('#myModalNotas').modal('show');

    $.get("../NotasIndicador/Deletepop/" + $(this).attr("data-id"))
        .done(function (data) {
            //   alert(data);
            $('#notas').empty();
            $('#notas').append(data);
        });

});

//-----------------------------AJAXA POST PARA ELIMINAR NOTAS CREADAS
$('#myModalNotas').on('click', '.NotaIndicadorDelete', function () {
    //alert("click");
    categ = $(this).attr("data-indicador");
    $.post("../NotasIndicador/Deletepop/" + $(this).attr("data-id"), $('#NotaDeleteForm').serialize())
        .done(function (data) {
            //alert(data);
            $('#notas').empty();
            if (data == true) {

                loadNotasIndicador(categ, "exito", "Nota eliminada");
            }
            else {

                loadNotasIndicador(categ, "error", "No se pudo eliminar la nota");
            }

        });

});



function loadNotasIndicador(id, tipo, mensaje, nombre) {
    //alert(nombre);
    $('#myModalLabelNotas').empty();
    $('#myModalLabelNotas').append(nombre);

    $('#notas').empty();
    $('#notas').append('<div class="loading"><img src="../images/loadingcircle.gif" ></div>');
    $('#myModalNotas').modal('show');
    $.get("../NotasIndicador/Indicadorpop", {
        'Indicadorid': id, 'tipo': tipo, 'mensaje': mensaje
    })
        .done(function (data) {
            //alert(tipo);
            //alert(mensaje);


            $('#notas').empty();
            $('#notas').append(data);

        });


}

