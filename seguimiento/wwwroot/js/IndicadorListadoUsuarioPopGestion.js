//--------------------------------------------------ESCUCHADORES
$('.periodoaccion').click(function () {

    loadEjecucion($(this).attr("data-IdEjecucion"), "", "")
});
$('#EjecucionContenido').on('click', '.EjecucionEdit', function () {
    displayEditEjecucion($(this).attr("data-id"));
});

$('#EjecucionContenido').on('click', '.EjecucionSaveEdit', function () {
    EjecucionAlmacenarEdicion();
});

$('.button-open-indicador').click(function () {

    displayIndicadorDetails($(this).attr("data-id"));
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


function displayEditEjecucion(id) {
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
function displayIndicadorDetails(id) {
    $('#EjecucionContenido').empty();
    $('#myModalEjecucion').modal('show');
    $('#EjecucionContenido').append('<div class="loading"><img src="/images/loadingcircle.gif" ></div>');
    $.get("../Indicadors/DetailsPop", { 'id': id })
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

//===================================== GESTION DE NOTAS=================================
$('#EjecucionContenido').on('submit', '#uploadFileFormEjecucion', function (d) {
    d.preventDefault();
});


$(document).ready(function () {
    var categoria = 0;

    $(".AbrirNotas").click(function () {
        // alert($(this).data("categoria"));
        loadNotas($(this).data("categoria"));
    });
    $('#myModalNotas').on('click', '.NotasVolver', function () {
        //alert($(this).data("categoria"));
        loadNotas($(this).data("categoria"));
    });

    $('#myModalNotas').on('click', '.NotaEdit', function () {
        $('#notas').empty();
        $('#notas').append('<div class="loading"><img src="../images/loadingcircle.gif" ></div>');
        $('#myModalNotas').modal('show');
        $.get("../Notas/Editpop/", { id: $(this).attr("data-id"), idcategoria: $(this).attr("data-categoria") })
            .done(function (data) {
                //   alert(data);
                $('#notas').empty();
                $('#notas').append(data);




            });

    });

    //-------------------------------- AJAX PARA ALMACENAR LA EDICION DE NOTAS
    $('#myModalNotas').on('click', '.NotaSaveEdit', function () {
        //alert("click");
        var categ = $(this).attr("data-categoria");
        $.post("../Notas/Editpop", $('#NotaEditForm').serialize())
            .done(function (data) {
                //alert(data);
                $('#notas').empty();
                if (data === true) {

                    loadNotas(categ, "exito", "Nota editada exitosamente");
                }
                else {

                    loadNotas(categ, "error", "No se pudo actualizar la nota");
                }

            });

    });
    // ------------------------ AJAX GET FORMULARIO NOTA NUEVA
    $('#myModalNotas').on('click', '.NotaCreate', function () {
        $('#notas').empty();
        $('#notas').append('<div class="loading"><img src="../images/loadingcircle.gif" ></div>');
        $('#myModalNotas').modal('show');

        $.get("../Notas/Createpop/" + $(this).attr("data-Categoria"))
            .done(function (data) {
                //   alert(data);
                $('#notas').empty();
                $('#notas').append(data);
            });

    });


    //-----------------------------AJAXA POST PARA ALMACENAR LAS NUEVAS NOTAS CREADAS
    $('#myModalNotas').on('click', '.NotaCreateButton', function () {
        //alert("click");
        var categ = $(this).attr("data-categoria");
        $.post("../Notas/Createpop", $('#NotaCreateForm').serialize())
            .done(function (data) {
                //alert(data);
                $('#notas').empty();
                if (data === true) {

                    loadNotas(categ, "exito", "Nota creada con exito");
                }
                else {

                    loadNotas(categ, "error", "No se pudo Crear la nota");
                }

            });

    });

    // ------------------------ AJAX GET DETALLE NOTA
    $('#myModalNotas').on('click', '.NotaDetalle', function () {
        $('#notas').empty();
        $('#notas').append('<div class="loading"><img src="../images/loadingcircle.gif" ></div>');
        $('#myModalNotas').modal('show');

        $.get("../Notas/Detailspop/" + $(this).attr("data-id"))
            .done(function (data) {
                //   alert(data);
                $('#notas').empty();
                $('#notas').append(data);
            });

    });

    // ------------------------ AJAX GET DELETE NOTA
    $('#myModalNotas').on('click', '.NotaBorrar', function () {
        $('#notas').empty();
        $('#notas').append('<div class="loading"><img src="../imaages/loadingcircle.gif" ></div>');
        $('#myModalNotas').modal('show');

        $.get("../Notas/Deletepop/" + $(this).attr("data-id"))
            .done(function (data) {
                //   alert(data);
                $('#notas').empty();
                $('#notas').append(data);
            });

    });

    //-----------------------------AJAXA POST PARA ELIMINAR NOTAS CREADAS
    $('#myModalNotas').on('click', '.NotaDelete', function () {
        //alert("click");
        var categ = $(this).attr("data-categoria");
        $.post("../Notas/Deletepop/" + $(this).attr("data-id"), $('#NotaDeleteForm').serialize())
            .done(function (data) {
                //alert(data);
                $('#notas').empty();
                if (data === true) {

                    loadNotas(categ, "exito", "Nota eliminada");
                }
                else {

                    loadNotas(categ, "error", "No se pudo eliminar la nota");
                }

            });

    });

});

function loadNotas(id, tipo, mensaje) {
    //alert("click");
    $('#notas').empty();
    $('#notas').append('<div class="loading"><img src="../images/loadingcircle.gif" ></div>');
    $('#myModalNotas').modal('show');
    $.get("../Notas/Categoriapop", { 'Categoriaid': id, 'tipo': tipo, 'mensaje': mensaje })
        .done(function (data) {
            //alert(tipo);
            //alert(mensaje);

            $('#notas').empty();
            $('#notas').append(data);

        });


}



