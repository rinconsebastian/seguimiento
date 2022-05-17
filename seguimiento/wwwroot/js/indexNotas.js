//NOTAS INDICADORES

$(".AbrirNotasIndicador").click(function () {
    //alert("open");

    loadNotasIndicador($(this).attr("data-indicador"), "", "", $(this).attr("data-nombre"));
});
$('body').on('click', '.NotasIndicadorVolver', function () {
    $('#myModalNotas').modal('hide');
    $('#notas').empty();
});

$('body').on('click', '.NotaIndicadorEdit', function () {
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
$('body').on('click', '.NotaIndicadorSaveEdit', function () {
    //alert("click");
    categ = $(this).attr("data-indicador");
    $.post("../NotasIndicador/Editpop", $('#NotaIndicadorEditForm').serialize())
        .done(function (data) {
            //alert(data);
            $('#notasIndicado').empty();
            if (data == true) {
                $('#myModalNotas').modal('hide');
                $('#notas').empty();
                location.reload();
            }
            else {

                $('#myModalNotas').modal('hide');
                $('#notas').empty();
                location.reload();
            }

        });

});
// ------------------------ AJAX GET FORMULARIO NOTA NUEVA
$('body').on('click', '.NotaIndicadorCreate', function () {
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
$('body').on('click', '.NotaIndicadorCreateButton', function () {
    categ = $(this).attr("data-indicador");
    //alert("click");
    $.post("../NotasIndicador/Createpop", $('#NotaIndicadorCreateForm').serialize())
        .done(function (data) {
            alert(data);
            $('#notas').empty();
            if (data == true) {

                $('#myModalNotas').modal('hide');
                $('#notas').empty();
            }
            else {

                $('#myModalNotas').modal('hide');
                $('#notas').empty();
            }

        });

});

// ------------------------ AJAX GET DETALLE NOTA 
$('body').on('click', '.NotaIndicadorDetalle', function () {
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
$('body').on('click', '.NotaIndicadorBorrar', function () {
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
$('body').on('click', '.NotaIndicadorDelete', function () {
    //alert("click");
    categ = $(this).attr("data-indicador");
    $.post("../NotasIndicador/Deletepop/" + $(this).attr("data-id"), $('#NotaDeleteForm').serialize())
        .done(function (data) {
            //alert(data);
            $('#notas').empty();
            if (data == true) {

                $('#myModalNotas').modal('hide');
                $('#notas').empty();
                location.reload();
            }
            else {

                $('#myModalNotas').modal('hide');
                $('#notas').empty();
                location.reload();
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

