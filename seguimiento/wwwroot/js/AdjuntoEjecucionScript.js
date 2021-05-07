//----------------------------- AJAXA POST PARA CARGAR ARCHIVOS -----------------------------
var root;

var funcAdjunto = {
    loadCreateAdjunto: function () {
        $('body').on('submit', '#formAdjuntoEjec', function (e) {
            e.preventDefault();
            var formulario = $(this).closest('form');
            var id = $("#idejecucion").val();
            // Serialize your form
            var formData = new FormData(formulario[0]);
            var time = funcAdjunto.getFormatDate();
            formData.append("time", time);


            $('#infoAdjuntoEjec').empty();
            $('#uploadingAdjuntoEjec').html('Cargando <img src="' + root + 'images/ajax-loader.gif">');
            $('#btnAdjuntoEjec').attr('disabled', 'disabled');

            $.ajax({
                type: "POST",
                url: formulario.attr('action'),
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response == '') {
                        $('#nameAdjuntoEjec').val('');
                        $('#fileAdjuntoEjec').val('');
                        $('#fileAdjuntoEjec').next('.custom-file-label').html('Seleccionar Archivo');

                        funcAdjunto.updateAdjuntos(id);
                    }
                    else {
                        $('#infoAdjuntoEjec').html(response);
                    }
                    $('#uploadingAdjuntoEjec').empty();
                    $('#btnAdjuntoEjec').removeAttr('disabled');
                },
                error: function (error) {
                    $('#uploadingAdjuntoEjec').empty();
                    $('#btnAdjuntoEjec').removeAttr('disabled');
                    $('#infoAdjuntoEjec').html("Error al cargar el archivo.");
                }
            });

        });
    },
    getFormatDate: function () {
        var dt = new Date();
        var day = dt.getDate();
        var month = dt.getMonth() + 1;
        var hour = dt.getHours();
        var minute = dt.getMinutes();
        var seconds = dt.getSeconds();
        if (day < 10) { day = "0" + day; }
        if (month < 10) { month = "0" + month; }
        if (hour < 10) { hour = "0" + hour; }
        if (minute < 10) { minute = "0" + minute; }
        if (seconds < 10) { seconds = "0" + seconds; }
        return dt.getFullYear() + "_" + month + "_" + day + "-" + hour + "_" + minute + "_" + seconds;
    },
    loadDeleteAdjunto: function () {
        $('body').on('click', '.confirDeleteAdj', function (e) {
            $('#ModalConfirm').modal('hide');
            var idAdjunto = $(this).data('idejec');
            var idEjec = $("#idejecucion").val();
            var fullurl = root + "EjecucionAdjunto/Delete/";
            $.post(fullurl, { id: idAdjunto },
                function (response) {
                    if (response == '') {
                        funcAdjunto.updateAdjuntos(idEjec);
                    }
                    else {
                        $('#infoAdjuntoEjec').html(response);
                    }

                }).fail(function () {
                    $('#infoAdjuntoEjec').html("No fue posible borrar el adjunto.");
                });
        });

    },
    updateAdjuntos: function (id) {
        $('#ejecAdjunto').html('<div class="loading"><img src="' + root + 'images/loadingcircle.gif" ></div>');
        var fullurl = root + "Ejecucions/EditPopAdjuntos/"
        $.get(fullurl, { 'idEjecucion': id })
            .done(function (data) {
                $('#ejecAdjunto').html(data);
            }).fail(function () {
                $('#ejecAdjunto').html("No fue posible recuperar los adjuntos de la ejecución.");
            });;
    },
    //Load functions
    loadConfirmDeleteAdjunto: function () {
        $('body').on('click', '.delete-adjunto', function (e) {
            var id = $(this).data('id');
            $('#ModalConfirText').html('¿Está seguro de que desea borrar este archivo adjunto?');

            $('#ModalConfirmButton').data('idejec', id)
                .addClass('confirDeleteAdj');

            $('#ModalConfirm').modal('show');
        });
    },
    init: function () {
        console.log("Adjunto Script load");
        root = $('#Root').val();
        funcAdjunto.loadCreateAdjunto();
        funcAdjunto.loadConfirmDeleteAdjunto();
        funcAdjunto.loadDeleteAdjunto();
    }
};


//************************************** ON READY **********************************************
$(function () {
    funcAdjunto.init();

    $('.modal').on("hidden.bs.modal", function (e) {
        if ($('.modal:visible').length) {
            $('body').addClass('modal-open');
        }
    });
});


