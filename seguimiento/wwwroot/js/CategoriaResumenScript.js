$(document).ready(function () {
    var categoria = 0;


    var ctx = document.getElementById('myChart').getContext('2d');
    var myBarChart = new Chart(ctx, {
        type: 'bar',
        data: data,

        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        suggestedMin: 0,
                        suggestedMax: 100
                    }
                }]
            }
        }

    });


    var ctxmodal = document.getElementById('myChartmodal').getContext('2d');
    var myChartmodal = new Chart(ctxmodal, {
        type: 'bar',
        data: data,

        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        suggestedMin: 0,
                        suggestedMax: 100
                    }
                }]
            }
        }

    });

    $("#myChart").click(function () {

        $('#myModal').modal('show');

    });

    $("#AbrirNotas").click(function () {
        loadNotas($(this).attr("data-categoria"));
    });
    $('#myModalNotas').on('click', '.NotasVolver', function () {
        loadNotas($(this).attr("data-categoria"));
    });

    $('#myModalNotas').on('click', '.NotaEdit', function () {
        $('#notas').empty();
        $('#notas').append('<div class="loading"><img src="images/loadingcircle.gif" ></div>');
        $('#myModalNotas').modal('show');
        $.get("../Notas/Editpop/", {
            id: $(this).attr("data-id"), idcategoria: $(this).attr("data-categoria")
        })
            .done(function (data) {
                //   alert(data);
                $('#notas').empty();
                $('#notas').append(data);




            });

    });

    //-------------------------------- AJAX PARA ALMACENAR LA EDICION DE NOTAS
    $('#myModalNotas').on('click', '.NotaSaveEdit', function () {
        //alert("click");
        categ = $(this).attr("data-categoria");
        $.post("../Notas/Editpop", $('#NotaEditForm').serialize())
            .done(function (data) {
                //alert(data);
                $('#notas').empty();
                if (data == true) {
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
        $('#notas').append('<div class="loading"><img src="images/loadingcircle.gif" ></div>');
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
        categ = $(this).attr("data-categoria");
        //alert("click");
        $.post("../Notas/Createpop", $('#NotaCreateForm').serialize())
            .done(function (data) {
                //alert(data);
                $('#notas').empty();
                if (data == true) {

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
        $('#notas').append('<div class="loading"><img src="images/loadingcircle.gif" ></div>');
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
        $('#notas').append('<div class="loading"><img src="images/loadingcircle.gif" ></div>');
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
        categ = $(this).attr("data-categoria");
        $.post("../Notas/Deletepop/" + $(this).attr("data-id"), $('#NotaDeleteForm').serialize())
            .done(function (data) {
                //alert(data);
                $('#notas').empty();
                if (data == true) {

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
    $.get("../Notas/Categoriapop", {
        'Categoriaid': id, 'tipo': tipo, 'mensaje': mensaje
    })
        .done(function (data) {
            //alert(tipo);
            //alert(mensaje);

            $('#notas').empty();
            $('#notas').append(data);

        });


}

//-----------------------------AJAXA POST PARA CARGAR ARCHIVOS
$('#myModalNotas').on('change', '#fileuploadtext', function () {
    /* // alert("change");
      $('#uploadFileForm').submit();
  });
    <img src="Content/images/ajax-loader.gif">
  $('#myModalNotas').on('submit', '#uploadFileForm', function (e) {

      e.preventDefault();
      */
    var formulario = $(this).closest('form');
    var dt = new Date();
    var time = dt.getFullYear() + "-" + dt.getMonth() + "-" + dt.getDay() + "-" + dt.getHours() + "-" + dt.getMinutes() + "-" + dt.getSeconds() + "n-";

    // Serialize your form
    var formData = new FormData(formulario[0]);
    formData.append("id", time);


    $('#urlAdjunto').empty();
    $('.uploading').empty();
    $('.uploading').append('cargando <img src="Content/images/ajax-loader.gif">');
    //alert(formData.stringify());
    $.ajax({
        type: "POST",
        url: 'Upload/UploadFile',
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (response) {

            //var respuesta = JSON.parse(response);
            $('#Adjunto').val(response.Nombre);
            $('#urlAdjunto').attr("href", "Upload/UploadedFiles/" + response.Nombre)
            $('#urlAdjunto').empty();
            $('.uploading').empty();
            $('#urlAdjunto').append(response.Nombre);

        },
        error: function (error) {
            // alert("errror al cargar el archivo");
            $('.uploading').empty();
            $('.uploading').append("Imposible cargar el archivo");
        }
    });

});